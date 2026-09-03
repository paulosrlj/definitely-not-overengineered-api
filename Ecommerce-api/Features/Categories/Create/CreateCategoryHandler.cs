using Ecommerce_api.Data;
using Ecommerce_api.Domain;

namespace Ecommerce_api.Features.Categories.Create;

public class CreateCategoryHandler
{
    private readonly AppDbContext _context;

    public CreateCategoryHandler(AppDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<CreateCategoryResponse> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = new Category()
        {
            Name = request.Name,
            Description = request.Description,
            IconIdentifier = request.IconIdentifier,
        };

        _context.Categories.Add(category);

        await _context.SaveChangesAsync(cancellationToken);

        return new CreateCategoryResponse(
            category.Id,
            category.Name,
            category.Description,
            category.IconIdentifier,
            category.CreatedAt,
            category.UpdatedAt
        );
    }
}