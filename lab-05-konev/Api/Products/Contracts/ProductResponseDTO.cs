namespace Lab05Konev.Api.Products.Contracts;

public class ProductResponseDTO
{
    public Guid Guid { get; init; }
    public int Id { get; init; }
    public required string Name { get; init; }
    public int Price { get; init; }
}
