using Ecommerce_api.Common;

namespace Ecommerce_api.Dtos.Response;

public record PaginationMeta(int Page, int Limit, int Total, int TotalPages);

// A interface existe apenas para que possamos usar ela, sem precisar declarar o T
// Verificar no filtro WrapResponseFilter
public class PaginatedResponse<T> : IPaginatedResponse
{
    public List<T> Data { get; }
    public PaginationMeta Meta { get; }

    public PaginatedResponse(List<T> data, int total, int page, int limit)
    {
        Data = data;
        Meta = new PaginationMeta(page, limit, total, (int)Math.Ceiling(total / (double)limit));
    }
}