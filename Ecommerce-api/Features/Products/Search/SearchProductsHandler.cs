using Ecommerce_api.Common.Pagination;
using Ecommerce_api.Data;
using Ecommerce_api.Infrastructure.FileStorage;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace Ecommerce_api.Features.Products.Search;

public class SearchProductsHandler
{
    private readonly AppDbContext _context;
    private readonly IFileUrlGenerator _fileUrlGenerator;

    public SearchProductsHandler(AppDbContext context, IFileUrlGenerator fileUrlGenerator)
    {
        _context = context;
        _fileUrlGenerator = fileUrlGenerator;
    }

    public async Task<PaginatedResponse<SearchProductsResponse>> Handle(
        SearchProductsRequest request,
        CancellationToken cancellationToken)
    {
        var skip = (request.Page - 1) * request.Limit;

        var query = _context.Products
            .AsNoTracking()
            .Where(p => EF.Property<NpgsqlTsVector>(p, "SearchVector")
                .Matches(EF.Functions.WebSearchToTsQuery("portuguese", request.Search)));

        var total = await query.CountAsync(cancellationToken);

        var productsResponse = await query
            .OrderByDescending(p =>
                EF.Property<NpgsqlTsVector>(p, "SearchVector")
                    .Rank(EF.Functions.WebSearchToTsQuery("portuguese", request.Search)))
            .Skip(skip)
            .Take(request.Limit)
            .Select(product => new SearchProductsResponse(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Stock,
                product.Categories,
                product.Files
                    .OrderBy(pf => pf.SortOrder)
                    .Select(pf => new SearchProductFileResponse(
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
            .ToListAsync<SearchProductsResponse>(cancellationToken);

        return new PaginatedResponse<SearchProductsResponse>(
            productsResponse,
            total,
            request.Page,
            request.Limit
        );
    }
}