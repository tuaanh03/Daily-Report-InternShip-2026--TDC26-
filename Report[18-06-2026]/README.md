# Research .NET Core 8

## Nội dung nghiên cứu hôm nay

Keyword: **.NET Core 8**  
Ngày research: **18/06/2026**  
Góc tiếp cận: **Developer migrate từ Spring Boot Java sang .NET 8**

Các nội dung đã tìm hiểu:

- Tổng quan .NET 8 và vai trò của ASP.NET Core trong backend.
- So sánh các khái niệm quen thuộc trong Spring Boot với .NET 8.
- Cách tổ chức project/folder khi chuyển từ Java Spring Boot sang C# .NET.
- File cấu hình trong .NET thay cho `application.yaml`.
- Công cụ build, package và dependency management thay cho Maven/Gradle.
- Cách viết và tổ chức test trong .NET.
- ORM và cách tương tác database bằng Entity Framework Core.
- Cách chia layer trong project .NET.
- Cách build, publish và chạy artifact sau khi build.
- Những nội dung cần học tiếp để migrate sang .NET hiệu quả.

## Mapping nhanh Spring Boot sang .NET 8

| Spring Boot Java | .NET 8 / ASP.NET Core |
| --- | --- |
| Java | C# |
| Spring Boot | ASP.NET Core |
| `application.yaml` | `appsettings.json` |
| Maven / Gradle | .NET CLI, MSBuild |
| `pom.xml` / `build.gradle` | `.csproj` |
| Multi-module project | `.sln` + nhiều `.csproj` |
| Spring DI | Built-in Dependency Injection |
| Controller | Controller hoặc Minimal API |
| Hibernate / JPA | Entity Framework Core |
| JUnit / Mockito | xUnit, NUnit, MSTest / Moq |
| `.jar` | `.dll`, `.exe`, publish folder, container image |

## Một số điểm cần lưu ý khi migrate

- .NET 8 là nền tảng runtime/SDK, còn ASP.NET Core mới là framework web gần giống Spring Boot.
- .NET mặc định dùng `appsettings.json`, không dùng `application.yaml`.
- Dependency được quản lý trong `.csproj` thông qua NuGet.
- Test thường được tách thành project riêng, ví dụ `MyApp.Tests`.
- EF Core không giống Hibernate hoàn toàn; cần học thêm `DbContext`, LINQ, tracking, migration và relationship mapping.
- Không nên bê nguyên cấu trúc Spring Boot sang .NET; nên chia layer theo convention của .NET và quy mô project.
- Khi deploy, .NET không chỉ tạo một file `.jar`; output có thể là `.dll`, `.exe`, publish folder, single-file hoặc Docker image.

## Demo đã tìm hiểu

Demo nhỏ sử dụng Minimal API để minh họa:

- `Program.cs` để cấu hình app, routing và dependency injection.
- `appsettings.json` để lưu cấu hình.
- Service layer đơn giản với `ProductService`.
- Endpoint `/products` để trả danh sách dữ liệu mẫu.

## File trong repository

- `bao-cao-dotnet-core-8.md`: báo cáo chi tiết về .NET Core 8 dưới góc nhìn migrate từ Spring Boot.
- `demo-small-api/`: demo Minimal API đơn giản bằng .NET 8, có kèm `Dockerfile`.
- `README.md`: tóm tắt nội dung nghiên cứu hôm nay.

## Hướng nghiên cứu tiếp theo

- ASP.NET Core Web API.
- Dependency Injection trong .NET.
- Configuration và Options Pattern.
- Entity Framework Core.
- Authentication/Authorization với JWT.
- Testing trong .NET.
- Build, publish và deploy ứng dụng .NET.

## Tài liệu tham khảo

- Microsoft Learn - Introduction to .NET: https://learn.microsoft.com/en-us/dotnet/core/introduction
- Microsoft Learn - .NET CLI overview: https://learn.microsoft.com/en-us/dotnet/core/tools/
- Microsoft Learn - ASP.NET Core documentation: https://learn.microsoft.com/en-us/aspnet/core/
- Microsoft Learn - Entity Framework Core: https://learn.microsoft.com/en-us/ef/core/
