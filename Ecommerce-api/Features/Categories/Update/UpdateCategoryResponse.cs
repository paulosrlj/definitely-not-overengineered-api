using Ecommerce_api.Domain;

namespace Ecommerce_api.Features.Categories.Update;

public record UpdateCategoryResponse(int Id, string Name, string Description, string IconIdentifier)
{
    public static UpdateCategoryResponse FromEntity(Category category)
    {
        return new UpdateCategoryResponse(category.Id, category.Name, category.Description, category.IconIdentifier);
    }
}