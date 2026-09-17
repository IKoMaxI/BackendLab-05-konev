# lab-05-konev

Лабораторная работа №5: маршрутизация в ASP.NET Core Web API.

Проект создан на основе `lab-03-konev` и расширен контроллерами для ресурсов:

- Students
- Products
- Orders

## Запуск

1. Открыть `lab-05-konev.sln` в Visual Studio 2022.
2. Восстановить NuGet-пакеты.
3. Запустить профиль `https`.
4. Swagger откроется по адресу `https://localhost:7087/swagger`.

Если Visual Studio назначит другой порт, используйте адрес из `Properties/launchSettings.json`.

## Основные маршруты

| URL | HTTP | Метод |
|---|---|---|
| `/api/students` | GET | GetAllStudents |
| `/api/students/1` | GET | GetStudent |
| `/api/students` | POST | CreateStudent |
| `/api/students/1` | PUT | UpdateStudent |
| `/api/students/1` | DELETE | DeleteStudent |
| `/api/students/optional` | GET | GetStudentOptional, id не задан |
| `/api/students/optional/1` | GET | GetStudentOptional, id задан |
| `/api/students/1/courses` | GET | GetStudentCourses |
| `/api/students/group?group=1` | GET | GetStudentsByGroup |
| `/api/students/search?page=1&pageSize=10&sort=name` | GET | SearchStudents |
| `/api/products/1` | GET | GetProduct, `{id:int}` |
| `/api/products/11111111-1111-1111-1111-111111111111` | GET | GetProductByGuid, `{guid:guid}` |
| `/api/products/by-name/phone` | GET | GetProductsByName, `minlength(3)` |
| `/api/products/by-slug/phone` | GET | GetProductsBySlug, `{slug:minlength(3)}` |
| `/api/orders/year/2024` | GET | GetOrdersByYear, `{year:int}` |
| `/api/orders/date/2024-03-10` | GET | GetOrdersByDate, `{date:datetime}` |
| `/api/orders/recent` | GET | GetRecentOrders, `days=7` по умолчанию |

Полный набор запросов находится в `lab-05-konev.http`.
