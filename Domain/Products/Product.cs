
using Flunt.Validations;
using IWantApp.Domain.Orders;

namespace IWantApp.Domain.Products;

public class Product : BaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }
    public bool HasStock { get; private set; }
    public bool Active { get; private set; } = true;
    public double Price { get; private set; }
    public List<Order> Orders { get; private set; }

    private Product() { }

    public Product(string name, Category category, string description, bool hasStock, double price, string createdBy)
    {
        Name = name;
        Category = category;
        Description = description;
        HasStock = hasStock;
        Price = price;
        
        CreatedBy = createdBy;
        EditedBy = createdBy;
        CreatedDate = DateTime.Now;
        EditedDate = DateTime.Now;

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Product>()
            .IsNotNullOrEmpty(Name, "Name")
            .IsGreaterOrEqualsThan(Name, 3, "Name")
            .IsNotNull(Category, "Category", "Category not found")
            .IsNotNullOrEmpty(Description, "Description")
            .IsGreaterOrEqualsThan(Description, 3, "Description")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
            .IsNotNullOrEmpty(EditedBy, "EditedBy")
            .IsNotNull(Price, "Price")
            .IsGreaterOrEqualsThan(Price, 0, "Price");

        AddNotifications(contract);
    }
}

