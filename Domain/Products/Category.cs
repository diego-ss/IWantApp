
using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Category : BaseEntity
{
    public string Name { get; private set; }
    public bool Active { get; private set; } = true;

    public Category(string name, string createdBy, string editedBy)
    {
        Name = name;
        CreatedBy = createdBy;
        EditedBy = editedBy;
        CreatedDate = DateTime.Now;
        EditedDate = DateTime.Now;

        CreateValidationContract();
    }

    private void CreateValidationContract()
    {
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(Name, "Name")
            .IsGreaterOrEqualsThan(Name, 3, "Name")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
            .IsNotNullOrEmpty(EditedBy, "EditedBy");
        AddNotifications(contract);
    }

    public void EditInfo(string name, bool active, string editedBy)
    {
        Active = active;
        Name = name;
        EditedBy = editedBy;
        EditedDate = DateTime.Now;

        CreateValidationContract();
    }
}
