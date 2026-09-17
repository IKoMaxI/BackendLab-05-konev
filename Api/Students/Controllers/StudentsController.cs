using Lab05Konev.Api.Students.Contracts;
using Lab05Konev.Api.Students.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab05Konev.Api.Students.Controllers;

// Базовый маршрут контроллера: api/students
[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private static readonly object SyncRoot = new();
    // GET api/students
    [HttpGet]
    public ActionResult<StudentResponseDTO[]> GetAllStudents()
    {
        lock (SyncRoot)
        {
            return Ok(_students.Select(ToResponse).ToArray());
        }
    }

    // GET api/students/1
    // Ограничение {id:int}: принимаются только целые числа.
    [HttpGet("{id:int}")]
    public ActionResult<StudentResponseDTO> GetStudent(int id)
    {
        lock (SyncRoot)
        {
            var student = _students.FirstOrDefault(x => x.Id == id);
            if (student is null)
                return NotFound("Студент не найден.");

            return Ok(ToResponse(student));
        }
    }

    // Демонстрация необязательного параметра маршрута {id?}.
    // GET api/students/optional       -> все студенты
    // GET api/students/optional/1     -> студент с id = 1
    [HttpGet("optional/{id:int?}")]
    public ActionResult GetStudentOptional(int? id = null)
    {
        lock (SyncRoot)
        {
            if (id is null)
                return Ok(_students.Select(ToResponse).ToArray());

            var student = _students.FirstOrDefault(x => x.Id == id.Value);
            if (student is null)
                return NotFound("Студент не найден.");

            return Ok(ToResponse(student));
        }
    }

    // Вложенный маршрут: GET api/students/1/courses
    [HttpGet("{id:int}/courses")]
    public ActionResult<List<Course>> GetStudentCourses(int id)
    {
        lock (SyncRoot)
        {
            var student = _students.FirstOrDefault(x => x.Id == id);
            if (student is null)
                return NotFound("Студент не найден.");

            return Ok(student.Courses);
        }
    }

    // Query-параметр как в отчёте:
    // GET api/students/group?group=2
    [HttpGet("group")]
    public ActionResult<StudentResponseDTO[]> GetStudentsByGroup([FromQuery] int? group = null)
    {
        lock (SyncRoot)
        {
            var students = _students
                .Where(x => group is null || x.Group == group.Value)
                .Select(ToResponse)
                .ToArray();

            return students.Length > 0
                ? Ok(students)
                : NotFound("Студенты не найдены.");
        }
    }

    // Query-параметры фильтрации/пагинации/сортировки из задания:
    // GET api/students/search?page=1&pageSize=10&sort=name
    [HttpGet("search")]
    public ActionResult<StudentResponseDTO[]> SearchStudents(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sort = "name",
        [FromQuery] int? group = null)
    {
        lock (SyncRoot)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("page должен быть >= 1, pageSize — от 1 до 100.");

            IEnumerable<Student> query = _students;

            if (group is not null)
                query = query.Where(x => x.Group == group.Value);

            query = sort.ToLowerInvariant() switch
            {
                "id" => query.OrderBy(x => x.Id),
                "group" => query.OrderBy(x => x.Group).ThenBy(x => x.Name),
                "name" => query.OrderBy(x => x.Name),
                _ => query.OrderBy(x => x.Name)
            };

            var result = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToResponse)
                .ToArray();

            return Ok(result);
        }
    }

    // POST api/students
    [HttpPost]
    public ActionResult<StudentResponseDTO> CreateStudent([FromBody] StudentRequestDTO contract)
    {
        lock (SyncRoot)
        {
            var id = _students.Count == 0 ? 1 : _students.Max(x => x.Id) + 1;

            var student = new Student
            {
                Id = id,
                Name = contract.Name,
                Group = contract.Group,
                Specialization = contract.Specialization,
                Courses = contract.Courses
            };

            _students.Add(student);

            return CreatedAtAction(
                nameof(GetStudent),
                new { id = student.Id },
                ToResponse(student));
        }
    }

    // PUT api/students/1
    [HttpPut("{id:int}")]
    public ActionResult UpdateStudent(int id, [FromBody] StudentRequestDTO contract)
    {
        lock (SyncRoot)
        {
            var student = _students.FirstOrDefault(x => x.Id == id);
            if (student is null)
                return NotFound("Студент не найден.");

            student.Name = contract.Name;
            student.Group = contract.Group;
            student.Specialization = contract.Specialization;
            student.Courses = contract.Courses;

            return NoContent();
        }
    }

    // DELETE api/students/1
    [HttpDelete("{id:int}")]
    public ActionResult DeleteStudent(int id)
    {
        lock (SyncRoot)
        {
            var index = _students.FindIndex(x => x.Id == id);
            if (index < 0)
                return NotFound("Студент не найден.");

            _students.RemoveAt(index);
            return NoContent();
        }
    }

    private static StudentResponseDTO ToResponse(Student student) => new()
    {
        Id = student.Id,
        Name = student.Name,
        Group = student.Group,
        Specialization = student.Specialization,
        Courses = student.Courses
    };

    // Временное хранилище данных (имитация базы данных).
    private static readonly List<Student> _students = new()
    {
        new Student
        {
            Id = 1,
            Name = "Student Alpha",
            Group = 1,
            Specialization = "Backend",
            Courses = new List<Course>
            {
                new() { Id = 1, Title = "ASP.NET Core" },
                new() { Id = 2, Title = "Databases" }
            }
        },
        new Student
        {
            Id = 2,
            Name = "Student Beta",
            Group = 2,
            Specialization = "Web",
            Courses = new List<Course>
            {
                new() { Id = 3, Title = "REST API" }
            }
        }
    };
}
