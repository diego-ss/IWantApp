using IWantApp.Domain.Products;

namespace IWantApp.Domain;

public abstract class BaseEntity
{
    public BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string EditedBy { get; set; }
    public DateTime EditedDate { get; set; }
}
