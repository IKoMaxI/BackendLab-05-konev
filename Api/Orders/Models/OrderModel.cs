namespace Lab05Konev.Api.Orders.Models;

public class Order
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int Price { get; set; }
}
