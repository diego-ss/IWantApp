using Flunt.Notifications;
using IWantApp.Domain.Products;
using System.Runtime.InteropServices;

namespace IWantApp.Domain;

public abstract class BaseEntity : Notifiable<Notification>
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
