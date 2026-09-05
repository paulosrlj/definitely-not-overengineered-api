using Ecommerce_api.Common.Pagination;
using Ecommerce_api.Data;
using Ecommerce_api.Infrastructure.FileStorage;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_api.Features.Products.FindAll;

public class FindAllProductsHandler
{
    private readonly AppDbContext _context;
    private readonly IFileUrlGenerator _fileUrlGenerator;

    public FindAllProductsHandler(AppDbContext context, IFileUrlGenerator fileUrlGenerator)
    {
        _context = context;
        _fileUrlGenerator = fileUrlGenerator;
    }

    public async Task<PaginatedResponse<FindAllProductsResponse>> Handle(
        FindAllProductsRequest request,
        CancellationToken cancellationToken)
    {
        var skip = (request.Page - 1) * request.Limit;
        var query = _context.Products.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(u =>
                EF.Functions.ILike(u.Name, $"%{request.Search}%"));
        }

        var total = await query.CountAsync(cancellationToken);
        var products = await query
            .Include(product => product.Categories)
            .Include(product => product.Files)
            .ThenInclude(productFile => productFile.File)
            .Skip(skip)
            .Take(request.Limit)
            .ToListAsync(cancellationToken);

        var productsResponse = products
            .Select(product => new FindAllProductsResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.Categories,
                product.Files
                    .OrderBy(pf => pf.SortOrder)
                    .Select(pf => new ProductFileResponse(
                        pf.File.Id,
                        pf.File.FileName,
                        pf.File.ContentType,
                        pf.File.Size,
                        _fileUrlGenerator.Generate(
                            pf.File.StorageKey),
                        pf.SortOrder
                    ))
                    .ToList()
            ))
            .ToList();

        return new PaginatedResponse<FindAllProductsResponse>(productsResponse.ToList(), total, request.Page,
            request.Limit);
    }
}