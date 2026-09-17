using Lab05Konev.Api.Orders.Contracts;
using Lab05Konev.Api.Orders.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab05Konev.Api.Orders.Controllers;

// Базовый маршрут контроллера: api/orders
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private static readonly object SyncRoot = new();
    // GET api/orders
    [HttpGet]
    public ActionResult<OrderResponseDTO[]> GetOrders()
    {
        lock (SyncRoot)
        {
            return Ok(_orders.Select(ToResponse).ToArray());
        }
    }

    // GET api/orders/year/2024
    // Параметр {year:int} принимает только целое число.
    [HttpGet("year/{year:int}")]
    public ActionResult<OrderResponseDTO[]> GetOrdersByYear(int year)
    {
        lock (SyncRoot)
        {
            var orders = _orders
                .Where(x => x.Date.Year == year)
                .Select(ToResponse)
                .ToArray();

            return orders.Length > 0
                ? Ok(orders)
                : NotFound("Заказы за этот год не найдены.");
        }
    }

    // GET api/orders/date/2024-03-10
    // Ограничение {date:datetime}: значение должно быть корректной датой.
    [HttpGet("date/{date:datetime}")]
    public ActionResult<OrderResponseDTO[]> GetOrdersByDate(DateTime date)
    {
        lock (SyncRoot)
        {
            var orders = _orders
                .Where(x => x.Date.Date == date.Date)
                .Select(ToResponse)
                .ToArray();

            return orders.Length > 0
                ? Ok(orders)
                : NotFound("Заказы на эту дату не найдены.");
        }
    }

    // Пример значения маршрута по умолчанию.
    // GET api/orders/recent     -> days = 7
    // GET api/orders/recent/30  -> days = 30
    [HttpGet("recent/{days:int=7}")]
    public ActionResult<OrderResponseDTO[]> GetRecentOrders(int days)
    {
        lock (SyncRoot)
        {
            if (days < 1 || days > (DateTime.Today - DateTime.MinValue).Days)
                return BadRequest("Количество дней должно быть больше нуля.");

            var fromDate = DateTime.Today.AddDays(-days);
            var result = _orders
                .Where(x => x.Date.Date >= fromDate)
                .Select(ToResponse)
                .ToArray();

            return Ok(result);
        }
    }

    // GET api/orders/1
    [HttpGet("{id:int}")]
    public ActionResult<OrderResponseDTO> GetOrder(int id)
    {
        lock (SyncRoot)
        {
            var order = _orders.FirstOrDefault(x => x.Id == id);
            if (order is null)
                return NotFound("Заказ не найден.");

            return Ok(ToResponse(order));
        }
    }

    // POST api/orders
    [HttpPost]
    public ActionResult<OrderResponseDTO> CreateOrder([FromBody] OrderRequestDTO contract)
    {
        lock (SyncRoot)
        {
            var id = _orders.Count == 0 ? 1 : _orders.Max(x => x.Id) + 1;

            var order = new Order
            {
                Id = id,
                Title = contract.Title,
                Date = contract.Date,
                Price = contract.Price
            };

            _orders.Add(order);

            return CreatedAtAction(
                nameof(GetOrder),
                new { id = order.Id },
                ToResponse(order));
        }
    }

    // PUT api/orders/1
    [HttpPut("{id:int}")]
    public ActionResult UpdateOrder(int id, [FromBody] OrderRequestDTO contract)
    {
        lock (SyncRoot)
        {
            var order = _orders.FirstOrDefault(x => x.Id == id);
            if (order is null)
                return NotFound("Заказ не найден.");

            order.Title = contract.Title;
            order.Date = contract.Date;
            order.Price = contract.Price;

            return NoContent();
        }
    }

    // DELETE api/orders/1
    [HttpDelete("{id:int}")]
    public ActionResult DeleteOrder(int id)
    {
        lock (SyncRoot)
        {
            var index = _orders.FindIndex(x => x.Id == id);
            if (index < 0)
                return NotFound("Заказ не найден.");

            _orders.RemoveAt(index);
            return NoContent();
        }
    }

    private static OrderResponseDTO ToResponse(Order order) => new()
    {
        Id = order.Id,
        Title = order.Title,
        Date = order.Date.ToString("dd.MM.yyyy HH:mm"),
        Price = order.Price
    };

    private static readonly List<Order> _orders = new()
    {
        new Order
        {
            Id = 1,
            Title = "Order Alpha",
            Date = new DateTime(2024, 3, 10, 12, 0, 0),
            Price = 1200
        },
        new Order
        {
            Id = 2,
            Title = "Order Beta",
            Date = new DateTime(2025, 5, 15, 18, 30, 0),
            Price = 2500
        }
    };
}
