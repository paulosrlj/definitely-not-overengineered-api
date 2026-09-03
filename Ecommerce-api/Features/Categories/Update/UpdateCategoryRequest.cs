using System.ComponentModel.DataAnnotations;

namespace Ecommerce_api.Features.Categories.Update;

public record UpdateCategoryRequest(
    [property: MinLength(3)]
    [property: MaxLength(50)]
    string? Name = null,

    [property: MinLength(3)]
    [property: MaxLength(50)]
    string? Description = null,
    
    [property: MinLength(3)]
    [property: MaxLength(50)]
    string? IconIdentifier = null
);