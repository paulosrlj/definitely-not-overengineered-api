namespace Ecommerce_api.Features.Categories.Create;

public record CreateCategoryResponse(int Id, string Name, string Description, 
    string IconIdentifier, DateTime CreatedAt, DateTime UpdatedAt);