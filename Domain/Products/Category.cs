
using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public bool Active { get; set; } = true;

    public Category(string name)
    {
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(name, "Name");
        AddNotifications(contract);

        Name = name;
    }
}
