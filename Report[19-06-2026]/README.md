# Research ASP.NET Core Web API

## Nội dung nghiên cứu hôm nay

Keyword: **ASP.NET Core Web API**  
Ngày research: **19/06/2026**  
Phạm vi: **Tìm hiểu cơ bản về ASP.NET Core Web API**

Mục tiêu của ngày research là nắm được các khái niệm nền tảng của ASP.NET Core Web API, biết Web API dùng để làm gì, project có cấu trúc cơ bản ra sao, request được xử lý như thế nào và các keyword quan trọng cần ghi nhớ.

## Kế hoạch tìm hiểu trong ngày

| Thứ tự | Nội dung tìm hiểu | Kết quả cần đạt |
| --- | --- | --- |
| 1 | Tổng quan ASP.NET Core Web API | Hiểu ASP.NET Core Web API là gì và dùng trong trường hợp nào |
| 2 | Cấu trúc project cơ bản | Biết vai trò của `Program.cs`, `Controllers`, `appsettings.json`, `.csproj` |
| 3 | Controller và endpoint | Hiểu controller là nơi định nghĩa API endpoint |
| 4 | Routing và HTTP method | Biết cách API nhận request qua URL và các method GET, POST, PUT, DELETE |
| 5 | Request và response | Hiểu API nhận dữ liệu từ client và trả về kết quả dạng JSON |
| 6 | Middleware pipeline | Hiểu request đi qua các thành phần xử lý trước khi đến endpoint |
| 7 | Dependency Injection cơ bản | Biết cách đăng ký và sử dụng service trong ASP.NET Core |
| 8 | Swagger/OpenAPI | Biết Swagger dùng để mô tả và test API |
| 9 | Tổng kết keyword | Hệ thống lại các từ khóa quan trọng để đưa vào báo cáo |

## Các nội dung đã tìm hiểu

- ASP.NET Core Web API là framework dùng để xây dựng HTTP API trên nền tảng .NET.
- Web API thường được dùng để cung cấp dữ liệu cho frontend web, mobile app, desktop app hoặc service khác.
- Project Web API có các file/thành phần cơ bản như `Program.cs`, `.csproj`, `appsettings.json`, `Controllers`.
- ASP.NET Core hỗ trợ hai cách xây dựng API: Minimal API và controller-based API.
- Controller-based API dùng class kế thừa `ControllerBase`, kết hợp các attribute như `[ApiController]`, `[Route]`, `[HttpGet]`, `[HttpPost]`.
- Routing dùng để ánh xạ URL và HTTP method đến endpoint phù hợp.
- Middleware pipeline là chuỗi xử lý request/response của ứng dụng.
- Dependency Injection được tích hợp sẵn trong ASP.NET Core.
- Swagger/OpenAPI giúp mô tả, xem và test API trực tiếp trên trình duyệt.
- Entity Framework Core là ORM của .NET, có thể dùng theo hướng Database First để sinh code từ database có sẵn.
- Với người quen Spring Boot, EF Core có thể hiểu gần giống Hibernate/JPA, `DbContext` gần với `EntityManager`, `DbSet<T>` gần với tập entity/repository.

## Keyword quan trọng

| Keyword | Ý nghĩa ngắn gọn |
| --- | --- |
| ASP.NET Core | Framework web hiện đại, cross-platform của .NET |
| Web API | Ứng dụng cung cấp chức năng/dữ liệu qua HTTP |
| REST API | Kiểu thiết kế API dựa trên tài nguyên và HTTP method |
| Controller | Class chứa các action xử lý request |
| Endpoint | Địa chỉ API cụ thể mà client có thể gọi |
| Routing | Cơ chế ánh xạ URL đến endpoint |
| Middleware | Thành phần nằm trong pipeline xử lý request/response |
| `Program.cs` | File cấu hình và khởi động ứng dụng |
| `appsettings.json` | File cấu hình ứng dụng |
| Dependency Injection | Cơ chế tiêm dependency vào class |
| Swagger/OpenAPI | Công cụ mô tả và test API |
| EF Core | ORM của .NET dùng để làm việc với database |
| Database First | Cách tạo entity và DbContext từ database có sẵn |
| DbContext | Class trung tâm quản lý truy vấn và thay đổi dữ liệu |
| DbSet | Đại diện cho một bảng hoặc tập entity trong EF Core |

## File trong repository

- `bao-cao-asp-net-core-web-api.md`: báo cáo chi tiết về keyword ASP.NET Core Web API.
- `bao-cao-linq.md`: báo cáo nhỏ về LINQ cho người mới học ASP.NET Core, có liên hệ với Spring Boot.
- `bao-cao-entity-framework-core-db-first.md`: báo cáo nhỏ về Entity Framework Core theo hướng Database First, có liên hệ với Spring Boot/Hibernate/JPA.
- `daily-report-19-06-2026.md`: daily report tóm tắt nội dung đã tìm hiểu trong ngày.
- `README.md`: tóm tắt nội dung research.

## Tài liệu tham khảo

- Microsoft Learn - APIs overview: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis
- Microsoft Learn - Tutorial: Create a controller-based web API with ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api
- Microsoft Learn - Routing in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing
- Microsoft Learn - Dependency injection in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
