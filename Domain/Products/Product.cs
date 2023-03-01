namespace IWantApp.Domain.Products;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public bool HasStock { get; set; }
}

