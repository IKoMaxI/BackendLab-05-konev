using Lab05Konev.Api.Students.Models;

namespace Lab05Konev.Api.Students.Contracts;

public class StudentResponseDTO
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Specialization { get; init; }
    public int Group { get; init; }
    public List<Course> Courses { get; init; } = new();
}
