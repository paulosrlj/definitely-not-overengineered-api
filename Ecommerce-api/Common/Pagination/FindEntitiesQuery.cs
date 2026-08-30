using System.ComponentModel.DataAnnotations;

namespace Ecommerce_api.Dtos.Request;

public class FindEntitiesQuery
{
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int Limit { get; set; } = 10;

    public string? Search { get; set; }
}