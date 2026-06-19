# Research ASP.NET Core Web API

## Nội dung nghiên cứu hôm nay

Keyword: **ASP.NET Core Web API**  
Ngày research: **19/06/2026**  
Phạm vi: **Tìm hiểu cơ bản về ASP.NET Core Web API**

Mục tiêu của ngày research là nắm được các khái niệm nền tảng của ASP.NET Core Web API, biết Web API dùng để làm gì, project có cấu trúc cơ bản ra sao, request được xử lý như thế nào và các keyword quan trọng cần ghi nhớ.

## Các nội dung đã tìm hiểu

- ASP.NET Core Web API là framework dùng để xây dựng HTTP API trên nền tảng .NET.
- Web API thường được dùng để cung cấp dữ liệu cho frontend web, mobile app, desktop app hoặc service khác.
- Project Web API có các file/thành phần cơ bản như `Program.cs`, `.csproj`, `appsettings.json`, `Controllers`.
- ASP.NET Core hỗ trợ hai cách xây dựng API: Minimal API và controller-based API.
- Controller-based API dùng class kế thừa `ControllerBase`, kết hợp các attribute như `[ApiController]`, `[Route]`, `[HttpGet]`, `[HttpPost]`.
- Routing dùng để ánh xạ URL và HTTP method đến endpoint phù hợp.
- Middleware pipeline là chuỗi xử lý request/response của ứng dụng.
- Entity Framework Core là ORM của .NET, có thể dùng theo hướng Database First để sinh code từ database có sẵn.
- Với người quen Spring Boot, EF Core có thể hiểu gần giống Hibernate/JPA, `DbContext` gần với `EntityManager`, `DbSet<T>` gần với tập entity/repository.

## Tài liệu tham khảo

- Microsoft Learn - APIs overview: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis
- Microsoft Learn - Tutorial: Create a controller-based web API with ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api
- Microsoft Learn - Routing in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/routing
- Microsoft Learn - Dependency injection in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
