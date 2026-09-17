using System.ComponentModel.DataAnnotations;

namespace Lab05Konev.Api.Products.Contracts;

public class ProductRequestDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public required string Name { get; init; }

    [Range(1, 1_000_000)]
    public int Price { get; init; }
}
