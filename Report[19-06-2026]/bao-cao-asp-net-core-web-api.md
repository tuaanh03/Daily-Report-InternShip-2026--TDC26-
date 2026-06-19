# Báo cáo research: ASP.NET Core Web API

## 1. Thông tin chung

- Keyword: ASP.NET Core Web API
- Ngày research: 19/06/2026
- Phạm vi: Tìm hiểu cơ bản trong ngày
- Mục tiêu: Nắm được ASP.NET Core Web API là gì, dùng để làm gì, cấu trúc project cơ bản, controller, endpoint, routing, middleware, dependency injection và Swagger/OpenAPI.

> Ghi chú: Trong báo cáo này, keyword "ASP.NET" được hiểu theo hướng ASP.NET Core Web API, là framework hiện đại của .NET để xây dựng HTTP API.

## 2. Tóm tắt ngắn gọn

ASP.NET Core Web API là một phần của ASP.NET Core, dùng để xây dựng các dịch vụ HTTP API. API này cho phép các ứng dụng khác gọi đến bằng HTTP và nhận dữ liệu trả về, thường là JSON.

Web API được dùng phổ biến trong các hệ thống:

- Website frontend cần lấy dữ liệu từ backend.
- Mobile app cần đăng nhập, lấy danh sách, gửi form hoặc cập nhật dữ liệu.
- Desktop app cần giao tiếp với server.
- Các service nội bộ cần trao đổi dữ liệu với nhau.

ASP.NET Core Web API có thể được xây dựng theo hai hướng chính:

- Minimal API: khai báo endpoint trực tiếp trong `Program.cs`, phù hợp với ứng dụng nhỏ, demo hoặc API gọn.
- Controller-based API: khai báo controller và action, phù hợp khi muốn tổ chức code rõ ràng hơn.

Trong phạm vi research hôm nay, nội dung tập trung vào controller-based API vì cách này dễ nhìn thấy các thành phần quen thuộc của một Web API: route, controller, action, request, response và status code.

## 3. ASP.NET Core Web API là gì?

ASP.NET Core là framework web hiện đại của .NET, có thể chạy trên Windows, Linux và macOS. ASP.NET Core Web API là cách sử dụng ASP.NET Core để xây dựng API qua giao thức HTTP.

Một Web API thường không trả về giao diện HTML hoàn chỉnh. Thay vào đó, nó trả về dữ liệu, thường ở dạng JSON. Client sẽ nhận dữ liệu này và hiển thị theo nhu cầu riêng.

Ví dụ:

- `GET /api/products`: lấy danh sách sản phẩm.
- `GET /api/products/1`: lấy chi tiết sản phẩm có id bằng 1.
- `POST /api/products`: tạo sản phẩm mới.
- `PUT /api/products/1`: cập nhật sản phẩm.
- `DELETE /api/products/1`: xóa sản phẩm.

## 4. Cấu trúc project ASP.NET Core Web API cơ bản

Một project ASP.NET Core Web API đơn giản thường có cấu trúc:

```text
MyWebApi
|-- Controllers
|   |-- WeatherForecastController.cs
|-- Properties
|   |-- launchSettings.json
|-- appsettings.json
|-- appsettings.Development.json
|-- Program.cs
|-- MyWebApi.csproj
```

Ý nghĩa các thành phần:

| Thành phần | Vai trò |
| --- | --- |
| `Program.cs` | File khởi động ứng dụng, cấu hình service, middleware và routing |
| `Controllers` | Thư mục chứa các controller xử lý API request |
| `appsettings.json` | File cấu hình chung của ứng dụng |
| `appsettings.Development.json` | File cấu hình riêng cho môi trường development |
| `.csproj` | File project, khai báo framework, package và cấu hình build |
| `launchSettings.json` | Cấu hình chạy local khi debug/dev |

## 5. Program.cs

`Program.cs` là file trung tâm khi ứng dụng ASP.NET Core khởi động. File này thường làm các việc:

- Tạo builder cho ứng dụng.
- Đăng ký service vào Dependency Injection container.
- Cấu hình Swagger/OpenAPI nếu cần.
- Build ứng dụng.
- Cấu hình middleware pipeline.
- Map controller hoặc endpoint.
- Chạy ứng dụng.

Ví dụ `Program.cs` cơ bản:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
```

Một số dòng quan trọng:

| Dòng code | Ý nghĩa |
| --- | --- |
| `WebApplication.CreateBuilder(args)` | Tạo đối tượng builder để cấu hình app |
| `builder.Services.AddControllers()` | Đăng ký controller support |
| `AddSwaggerGen()` | Đăng ký Swagger generator |
| `app.UseSwagger()` | Bật middleware sinh file Swagger |
| `app.UseSwaggerUI()` | Bật giao diện Swagger UI |
| `app.UseHttpsRedirection()` | Tự động chuyển request HTTP sang HTTPS |
| `app.MapControllers()` | Map các controller endpoint vào routing |
| `app.Run()` | Chạy ứng dụng |

## 6. Controller và endpoint

Controller là class xử lý request của client. Trong ASP.NET Core Web API, controller thường:

- Kế thừa `ControllerBase`.
- Có attribute `[ApiController]`.
- Có attribute `[Route(...)]` để định nghĩa URL.
- Có các action method ứng với HTTP method.

Ví dụ controller đơn giản:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace MyWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var products = new[]
        {
            new { Id = 1, Name = "Laptop", Price = 1500 },
            new { Id = 2, Name = "Mouse", Price = 20 }
        };

        return Ok(products);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = new { Id = id, Name = "Laptop", Price = 1500 };
        return Ok(product);
    }
}
```

Với controller trên:

| Request | Action được gọi | Ý nghĩa |
| --- | --- | --- |
| `GET /api/products` | `GetAll()` | Lấy danh sách sản phẩm |
| `GET /api/products/1` | `GetById(1)` | Lấy sản phẩm theo id |

## 7. Routing và HTTP method

Routing là cơ chế xác định request URL nào sẽ được xử lý bởi endpoint nào.

Trong controller-based API, routing thường được khai báo bằng attribute:

```csharp
[Route("api/[controller]")]
```

Nếu class tên là `ProductsController`, `[controller]` sẽ được hiểu là `products`. Khi đó route gốc của controller là:

```text
/api/products
```

Một số HTTP method cơ bản:

| HTTP method | Attribute | Mục đích thường dùng |
| --- | --- | --- |
| GET | `[HttpGet]` | Lấy dữ liệu |
| POST | `[HttpPost]` | Tạo mới dữ liệu |
| PUT | `[HttpPut]` | Cập nhật toàn bộ dữ liệu |
| PATCH | `[HttpPatch]` | Cập nhật một phần dữ liệu |
| DELETE | `[HttpDelete]` | Xóa dữ liệu |

## 8. Request, response và status code

Web API nhận request từ client và trả response về client.

Request thường bao gồm:

- URL.
- HTTP method.
- Header.
- Query string.
- Route parameter.
- Body, nếu cần gửi dữ liệu.

Response thường bao gồm:

- Status code.
- Header.
- Body, thường là JSON.

Một số status code cơ bản:

| Status code | Ý nghĩa |
| --- | --- |
| 200 OK | Request thành công |
| 201 Created | Tạo mới thành công |
| 204 No Content | Thành công nhưng không có body trả về |
| 400 Bad Request | Request sai hoặc dữ liệu không hợp lệ |
| 401 Unauthorized | Chưa xác thực |
| 403 Forbidden | Không có quyền |
| 404 Not Found | Không tìm thấy tài nguyên |
| 500 Internal Server Error | Lỗi phía server |

Ví dụ nhận body từ request:

```csharp
public record CreateProductRequest(string Name, decimal Price);

[HttpPost]
public IActionResult Create(CreateProductRequest request)
{
    var product = new
    {
        Id = 3,
        request.Name,
        request.Price
    };

    return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
}
```

Trong ví dụ trên, ASP.NET Core tự động bind JSON body từ request vào object `CreateProductRequest`.

## 9. Middleware pipeline

Middleware là các thành phần xử lý request và response trong ASP.NET Core.

Request khi vào ứng dụng sẽ đi qua nhiều middleware trước khi đến endpoint. Sau khi endpoint xử lý xong, response có thể đi ngược lại qua pipeline để trả về client.

Ví dụ một số middleware thường gặp:

| Middleware | Vai trò |
| --- | --- |
| `UseHttpsRedirection()` | Chuyển HTTP sang HTTPS |
| `UseSwagger()` | Sinh tài liệu Swagger/OpenAPI |
| `UseSwaggerUI()` | Hiển thị giao diện test API |
| `UseAuthentication()` | Xử lý xác thực |
| `UseAuthorization()` | Xử lý phân quyền |

Trong phạm vi research cơ bản, cần nhớ middleware là chuỗi xử lý request/response của ASP.NET Core.

## 10. Dependency Injection cơ bản

Dependency Injection là cơ chế giúp một class nhận dependency từ bên ngoài thay vì tự tạo dependency bên trong.

ASP.NET Core có sẵn DI container. Service được đăng ký trong `Program.cs`, sau đó controller có thể nhận service qua constructor.

Ví dụ service:

```csharp
public interface IProductService
{
    IEnumerable<object> GetProducts();
}

public class ProductService : IProductService
{
    public IEnumerable<object> GetProducts()
    {
        return new[]
        {
            new { Id = 1, Name = "Laptop", Price = 1500 },
            new { Id = 2, Name = "Mouse", Price = 20 }
        };
    }
}
```

Đăng ký service trong `Program.cs`:

```csharp
builder.Services.AddScoped<IProductService, ProductService>();
```

Dùng service trong controller:

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_productService.GetProducts());
    }
}
```

Ba kiểu lifetime cơ bản:

| Lifetime | Ý nghĩa |
| --- | --- |
| Singleton | Tạo một instance duy nhất cho toàn bộ vòng đời ứng dụng |
| Scoped | Tạo một instance cho mỗi request |
| Transient | Tạo instance mới mỗi lần được yêu cầu |

## 11. Swagger/OpenAPI

Swagger/OpenAPI giúp mô tả các API endpoint của ứng dụng. Khi chạy project, developer có thể mở Swagger UI trên trình duyệt để:

- Xem danh sách API endpoint.
- Xem method, route, request body và response.
- Test API trực tiếp.
- Kiểm tra nhanh API có hoạt động không.

Với người mới học ASP.NET Core Web API, Swagger là công cụ rất hữu ích vì không cần dùng Postman ngay từ đầu vẫn có thể test API.

## 12. Luồng xử lý request cơ bản

Luồng xử lý request trong ASP.NET Core Web API có thể hiểu đơn giản:

```text
Client
  -> Gửi HTTP request
  -> ASP.NET Core app nhận request
  -> Request đi qua middleware pipeline
  -> Routing xác định endpoint phù hợp
  -> Controller action xử lý
  -> Tạo response
  -> Trả response về client
```

Ví dụ:

```text
GET /api/products/1
```

Luồng xử lý:

1. Client gửi request `GET /api/products/1`.
2. ASP.NET Core nhận request.
3. Middleware pipeline xử lý các bước cần thiết.
4. Routing tìm controller/action phù hợp.
5. `ProductsController.GetById(1)` được gọi.
6. Action trả về dữ liệu sản phẩm.
7. ASP.NET Core serialize object thành JSON.
8. Client nhận response.

## 13. Kiến thức rút ra

Sau khi tìm hiểu keyword ASP.NET Core Web API, có thể rút ra:

- ASP.NET Core Web API dùng để xây dựng backend API qua HTTP.
- API thường trả dữ liệu dạng JSON thay vì trả giao diện HTML.
- `Program.cs` là nơi cấu hình và khởi động ứng dụng.
- Controller là nơi định nghĩa các endpoint API.
- Routing giúp ánh xạ URL và HTTP method đến action phù hợp.
- Middleware pipeline là một đặc trưng quan trọng trong cách ASP.NET Core xử lý request.
- Dependency Injection được tích hợp sẵn và là cách phù hợp để quản lý service.
- Swagger/OpenAPI giúp xem và test API nhanh trong quá trình phát triển.

## 14. Kết luận

ASP.NET Core Web API là một công nghệ quan trọng trong hệ sinh thái .NET, dùng để xây dựng các API hiện đại, có khả năng phục vụ nhiều loại client khác nhau. Ở mức cơ bản, người học cần nắm được các thành phần chính gồm `Program.cs`, controller, endpoint, routing, HTTP method, request/response, middleware, Dependency Injection và Swagger.

Phạm vi research hôm nay chỉ dừng ở mức nền tảng, chưa đi sâu vào database, authentication, authorization, testing hay deployment. Những kiến thức trên là nền móng cần thiết để có thể tiếp tục học và thực hành xây dựng API trong ASP.NET Core.

## 15. Tài liệu tham khảo

- Microsoft Learn - APIs overview: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis
- Microsoft Learn - Tutorial: Create a controller-based web API with ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api
- Microsoft Learn - Routing in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing
- Microsoft Learn - Dependency injection in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
