using System.Text;
using Amazon.Runtime;
using Amazon.S3;
using Ecommerce_api.Common;
using Ecommerce_api.Data;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Features.Auth;
using Ecommerce_api.Features.Categories;
using Ecommerce_api.Features.Products;
using Ecommerce_api.Features.Users;
using Ecommerce_api.Infrastructure.Auth;
using Ecommerce_api.Infrastructure.Cache;
using Ecommerce_api.Infrastructure.FileStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<WrapResponseFilter>();
});
// builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ecommerce API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    
    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "SampleInstance:";
});

// DI
builder.Services.AddScoped<ICacheService, RedisService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAuthFeature();
builder.Services.AddUserFeature();
builder.Services.AddCategoryFeature();
builder.Services.AddProductFeature();


// JWT //
// Binda a seção "Jwt" do appsettings, à classe JwtSettings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_myAllowSpecificOrigins",
        policy  =>
        {
            policy.WithOrigins(builder.Configuration.GetValue<string>("Frontend:Url")!)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Custom exception
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .SelectMany(e => e.Value!.Errors.Select(err => err.ErrorMessage))
            .ToList();

        return new BadRequestObjectResult(new
        {
            statusCode = 400,
            message = errors,
            error = "Bad Request"
        });
    };
});

///////////// AWS //////////////////
var awsSettings = builder.Configuration.GetSection("AWS").Get<AwsSettings>();
builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AWS"));

var credentials = new BasicAWSCredentials(awsSettings!.S3.AccessKey, awsSettings.S3.SecretKey);
builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var config = new AmazonS3Config()
    {
        RegionEndpoint = Amazon.RegionEndpoint.USEast1,
    };

    // Floci
    // Configurei meu AWS pra usar o floci em ambiente DEV
    if (!string.IsNullOrEmpty(awsSettings.S3.ServiceUrl))
    {
        config.ServiceURL = awsSettings.S3.ServiceUrl;
        config.ForcePathStyle = true;
        config.UseHttp = true;
    }
    
    return new AmazonS3Client(credentials, config);
});

///////// AWS ///////////

builder.Services.AddScoped<IFileUrlGenerator, CloudFrontFileUrlGenerator>();

// builder.Services.Configure<S3Settings>(builder.Configuration.GetSection("AWS:S3"));
// builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
// builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddScoped<IFileStorage, S3FileStorage>();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>(); 
app.UseCors("_myAllowSpecificOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // sempre antes de UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
