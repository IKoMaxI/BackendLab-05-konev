namespace Lab05Konev.Api.Orders.Contracts;

public class OrderResponseDTO
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Date { get; init; }
    public int Price { get; init; }
}
