namespace Ecommerce_api.Domain;

public class Product : IAuditable
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; }  = string.Empty;
    public int Price { get; set; }
    public int Stock { get; set; }
    
    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }  =  DateTime.UtcNow;
    
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
