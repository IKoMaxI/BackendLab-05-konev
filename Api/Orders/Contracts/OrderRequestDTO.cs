using System.ComponentModel.DataAnnotations;

namespace Lab05Konev.Api.Orders.Contracts;

public class OrderRequestDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public required string Title { get; init; }

    [Required]
    public DateTime Date { get; init; }

    [Range(1, 1_000_000)]
    public int Price { get; init; }
}
