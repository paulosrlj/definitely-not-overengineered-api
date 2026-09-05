namespace Ecommerce_api.Domain;

public class File : IAuditable
{
    public int Id { get; set; }
    public string FileName  { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long Size { get; set; } 
    public string StorageKey { get; set; }  = string.Empty;
    public StorageProvider Provider { get; set; }

    public DateTime CreatedAt { get; set; } =  DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }  =  DateTime.UtcNow;
    
}
