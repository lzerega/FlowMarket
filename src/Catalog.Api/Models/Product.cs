using System.Security.Claims;

namespace Catalog.Api.Models;

public class Product { 
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public Product(int id, string name, decimal price)
    {
        this.Id = id;
        this.Name = name;
        this.Price = price;
    }

    public Product()
    {
    }
}