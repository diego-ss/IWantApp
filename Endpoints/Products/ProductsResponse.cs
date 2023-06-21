namespace IWantApp.Endpoints.Products;

public record ProductsResponse(Guid Id, string Name, string CategoryName, string Description, double Price, bool HasStock, bool Active);
