using System.ComponentModel.DataAnnotations;

namespace Ecommerce_api.Features.Products.Update;

public class UpdateProductRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0, int.MaxValue)]
    public int Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Required]
    public int[] CategoryIds { get; set; } = [];
}