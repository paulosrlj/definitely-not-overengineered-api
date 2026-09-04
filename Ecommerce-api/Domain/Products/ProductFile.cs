namespace Ecommerce_api.Domain.Products;

public class ProductFile : IAuditable
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int FileId { get; set; }
    public File File { get; set; } = null!;

    public int SortOrder { get; set; }
    
    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }  =  DateTime.UtcNow;
}