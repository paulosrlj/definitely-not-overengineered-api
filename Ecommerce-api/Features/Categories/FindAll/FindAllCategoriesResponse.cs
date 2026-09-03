using Ecommerce_api.Domain;

namespace Ecommerce_api.Features.Categories.FindAll;

public record FindAllCategoriesResponse(int Id, string Name, string Description, string IconIdentifier, DateTime CreatedAt, DateTime UpdatedAt)
{
    public static FindAllCategoriesResponse FromEntity(Category category)
    {
        return new FindAllCategoriesResponse(category.Id, category.Name, category.Description, 
            category.IconIdentifier,  category.CreatedAt, category.UpdatedAt);
    }
}