using Lab05Konev.Api.Products.Contracts;
using Lab05Konev.Api.Products.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab05Konev.Api.Products.Controllers;

// Базовый маршрут контроллера: api/products
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    // GET api/products
    [HttpGet]
    public ActionResult<ProductResponseDTO[]> GetProducts()
    {
        return Ok(_products.Select(ToResponse).ToArray());
    }

    // GET api/products/by-name/phone
    // Ограничение minlength(3): минимум три символа.
    [HttpGet("by-name/{name:minlength(3)}")]
    public ActionResult<ProductResponseDTO[]> GetProductsByName(string name)
    {
        var products = _products
            .Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .Select(ToResponse)
            .ToArray();

        return products.Length > 0
            ? Ok(products)
            : NotFound("Продукты не найдены.");
    }

    // Дополнительный пример ограничения из задания: {slug:minlength(3)}.
    [HttpGet("by-slug/{slug:minlength(3)}")]
    public ActionResult<ProductResponseDTO[]> GetProductsBySlug(string slug)
    {
        var normalized = slug.Replace('-', ' ');
        var products = _products
            .Where(x => x.Name.Contains(normalized, StringComparison.OrdinalIgnoreCase))
            .Select(ToResponse)
            .ToArray();

        return products.Length > 0
            ? Ok(products)
            : NotFound("Продукты не найдены.");
    }

    // GET api/products/{guid}
    // Ограничение {guid:guid}: только корректный GUID.
    [HttpGet("{guid:guid}")]
    public ActionResult<ProductResponseDTO> GetProductByGuid(Guid guid)
    {
        var product = _products.FirstOrDefault(x => x.Guid == guid);
        if (product is null)
            return NotFound("Продукт не найден.");

        return Ok(ToResponse(product));
    }

    // GET api/products/1
    // Ограничение {id:int}: только целое число.
    [HttpGet("{id:int}")]
    public ActionResult<ProductResponseDTO> GetProduct(int id)
    {
        var product = _products.FirstOrDefault(x => x.Id == id);
        if (product is null)
            return NotFound("Продукт не найден.");

        return Ok(ToResponse(product));
    }

    // POST api/products
    [HttpPost]
    public ActionResult<ProductResponseDTO> CreateProduct([FromBody] ProductRequestDTO contract)
    {
        var id = _products.Count == 0 ? 1 : _products.Max(x => x.Id) + 1;

        var product = new Product
        {
            Guid = Guid.NewGuid(),
            Id = id,
            Name = contract.Name,
            Price = contract.Price
        };

        _products.Add(product);

        return CreatedAtAction(
            nameof(GetProduct),
            new { id = product.Id },
            ToResponse(product));
    }

    // PUT api/products/1
    [HttpPut("{id:int}")]
    public ActionResult UpdateProduct(int id, [FromBody] ProductRequestDTO contract)
    {
        var product = _products.FirstOrDefault(x => x.Id == id);
        if (product is null)
            return NotFound("Продукт не найден.");

        product.Name = contract.Name;
        product.Price = contract.Price;

        return NoContent();
    }

    // DELETE api/products/1
    [HttpDelete("{id:int}")]
    public ActionResult DeleteProduct(int id)
    {
        var index = _products.FindIndex(x => x.Id == id);
        if (index < 0)
            return NotFound("Продукт не найден.");

        _products.RemoveAt(index);
        return NoContent();
    }

    private static ProductResponseDTO ToResponse(Product product) => new()
    {
        Guid = product.Guid,
        Id = product.Id,
        Name = product.Name,
        Price = product.Price
    };

    private static readonly List<Product> _products = new()
    {
        new Product
        {
            Guid = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Id = 1,
            Name = "Phone",
            Price = 50000
        },
        new Product
        {
            Guid = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Id = 2,
            Name = "Laptop",
            Price = 90000
        }
    };
}
