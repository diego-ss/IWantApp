namespace IWantApp.Domain.Products;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public bool HasStock { get; set; }
    public bool Active { get; set; } = true;
}

