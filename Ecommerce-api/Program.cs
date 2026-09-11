using Ecommerce_api.Common;
using Ecommerce_api.Config;
using Ecommerce_api.Exceptions;
using Ecommerce_api.Features;
using Ecommerce_api.Infrastructure.Auth;
using Ecommerce_api.Infrastructure.Cache;
using Ecommerce_api.Infrastructure.FileStorage;
using Ecommerce_api.Middleware;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<WrapResponseFilter>();
});
// builder.Services.AddEndpointsApiExplorer();
builder.Services.SetupSwagger();
builder.Services.AddRedisService(builder.Configuration);

// DI
builder.Services.AddScoped<ICacheService, RedisService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddFeatures();


// JWT //
builder.Services.SetupSecurity(builder.Configuration);

// Custom exception
builder.Services.AddModelValidation();
///////////// AWS //////////////////
builder.Services.AddAwsService(builder.Configuration);
///////// AWS ///////////

builder.Services.AddScoped<IFileUrlGenerator, CloudFrontFileUrlGenerator>();

builder.Services.AddScoped<IFileStorage, S3FileStorage>();

var app = builder.Build();
app.UseHttpsRedirection();

// Middlewares
app.AddMiddleware();

app.UseCors(SecurityConfig.CorsPolicyName);

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
