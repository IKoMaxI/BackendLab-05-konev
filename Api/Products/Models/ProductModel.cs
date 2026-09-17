namespace Lab05Konev.Api.Products.Models;

public class Product
{
    public Guid Guid { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Price { get; set; }
}
