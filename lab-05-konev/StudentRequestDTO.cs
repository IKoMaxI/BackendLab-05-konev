using System.ComponentModel.DataAnnotations;
using Lab05Konev.Api.Students.Models;

namespace Lab05Konev.Api.Students.Contracts;

public class StudentRequestDTO
{
    [Required]
    [MaxLength(40)]
    [MinLength(2)]
    [RegularExpression(@"^([A-ZА-Я][a-zа-я]*[ ]*)+$")]
    public required string Name { get; init; }

    [Required]
    [Range(1, 100)]
    public int Group { get; init; }

    [Required]
    [MinLength(2)]
    [MaxLength(100)]
    public required string Specialization { get; init; }

    public List<Course> Courses { get; init; } = new();
}
