using Ecommerce_api.Domain;

namespace Ecommerce_api.Features.Categories.FindOne;

public record FindOneCategoryResponse(int Id, string Name, string Description, string IconIdentifier, DateTime CreatedAt,  DateTime UpdatedAt)
{
    public static FindOneCategoryResponse FromEntity(Category category)
    {
        return new FindOneCategoryResponse(category.Id, category.Name, category.Description, 
            category.IconIdentifier, category.CreatedAt, category.UpdatedAt);
    }
}