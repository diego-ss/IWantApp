
using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Category : BaseEntity
{
    public string Name { get; set; }
    public bool Active { get; set; } = true;

    public Category(string name, string createdBy, string editedBy)
    {
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(name, "Name")
            .IsGreaterOrEqualsThan(name, 3, "Name")
            .IsNotNullOrEmpty(createdBy, "CreatedBy")
            .IsNotNullOrEmpty(editedBy, "EditedBy");
        AddNotifications(contract);

        Name = name;
        CreatedBy = createdBy;
        EditedBy = editedBy;
        CreatedDate = DateTime.Now;
        EditedDate = DateTime.Now;
    }
}
