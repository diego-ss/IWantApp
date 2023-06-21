using Flunt.Validations;
using IWantApp.Domain.Products;
using Microsoft.AspNetCore.Mvc;

namespace IWantApp.Domain.Orders;

public class Order : BaseEntity
{
    public string ClientId { get; private set; }
    public List<Product> Products { get; private set; }
    public double Total { get; private set; }
    public string DeliveryAddress { get; set; }

    private Order() { }

    public Order(string clientId, string clientName, List<Product> products, string deliveryAddress)
    {
        ClientId = clientId;
        Products = products;
        DeliveryAddress = deliveryAddress;
        CreatedBy = clientName;
        EditedBy = clientName;
        CreatedDate = DateTime.UtcNow;
        EditedDate = DateTime.UtcNow;

        Total = 0;
        Total = products.Sum(p => p.Price);

        Validate();
    }

    private void Validate()
    {
        var contract = new Contract<Order>()
            .IsNotNull(ClientId, "Client")
            .IsNotNull(Products, "Products");
        AddNotifications(contract);
    }
}
