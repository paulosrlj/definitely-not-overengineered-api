using System.ComponentModel.DataAnnotations;

namespace Ecommerce_api.Features.Categories.Create;

public class CreateCategoryRequest
{
    [Required, MinLength(3), MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required, MinLength(3), MaxLength(50)]
    public string Description { get; set; } = string.Empty;

    [Required, MinLength(3), MaxLength(20)]
    public string IconIdentifier { get; set; } = string.Empty;
}