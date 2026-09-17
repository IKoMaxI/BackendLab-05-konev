var builder = WebApplication.CreateBuilder(args);

// Подключаем контроллеры.
builder.Services.AddControllers();

// Подключаем Swagger / OpenAPI для просмотра и тестирования маршрутов.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

// Маршруты контроллеров с атрибутами [Route] и [HttpGet]/[HttpPost]/...
app.MapControllers();

app.Run();
