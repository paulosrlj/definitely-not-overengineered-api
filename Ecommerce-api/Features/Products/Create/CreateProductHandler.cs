using Ecommerce_api.Data;
using Ecommerce_api.Domain;
using Ecommerce_api.Domain.Products;
using Ecommerce_api.Infrastructure.FileStorage;
using Microsoft.EntityFrameworkCore;
using File = Ecommerce_api.Domain.File;

namespace Ecommerce_api.Features.Products.Create;

public class CreateProductHandler
{
    private readonly AppDbContext _context;
    private readonly IFileStorage _fileStorage;

    public CreateProductHandler(AppDbContext dbContext, IFileStorage fileStorage)
    {
        _context = dbContext;
        _fileStorage = fileStorage;
    }

    public async Task<CreateProductResponse> Handle(
        CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        var product = new Product()
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock
        };

        // Associate categories to products
        var categories = await _context.Categories
            .Where(c => request.CategoryIds.AsEnumerable().Contains(c.Id))
            .ToListAsync(cancellationToken);

        foreach (var category in categories)
        {
            product.Categories.Add(category);
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        var uploadedKeys = new List<string>();

        try
        {
            foreach (var image in request.Images)
            {
                var extension = Path
                    .GetExtension(image.FileName)
                    .ToLowerInvariant();

                var key =
                    $"products/{product.Id}/{Guid.NewGuid()}{extension}";

                await using var stream = image.OpenReadStream();

                await _fileStorage.UploadAsync(
                    stream,
                    key,
                    image.ContentType,
                    cancellationToken
                );

                uploadedKeys.Add(key);

                var file = new File()
                {
                    StorageKey = key,
                    ContentType = image.ContentType,
                    FileName = image.FileName,
                    Size = image.Length,
                    Provider = StorageProvider.S3
                };
                
                product.Files.Add(new ProductFile()
                {
                    File =  file,
                    ProductId = product.Id
                });
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch(Exception error)
        {
            Console.WriteLine(error);
            foreach (var key in uploadedKeys)
            {
                await _fileStorage.DeleteAsync(
                    key,
                    cancellationToken);
            }

            throw;
        }

        return new CreateProductResponse(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.Files
                .Select(productFile => productFile.File.StorageKey)
                .ToList()
        );
    }
}