# Báo cáo research: .NET Core 8 dưới góc nhìn chuyển từ Spring Boot

## 1. Thông tin chung

- Keyword: .NET Core 8
- Tên gọi chính thức: .NET 8
- Ngày research: 18/06/2026
- Góc tiếp cận: So sánh với Spring Boot để người đã biết Java backend có thể hiểu .NET nhanh hơn.
- Mục tiêu: Không chỉ biết .NET 8 là gì, mà phải hiểu khi chuyển từ Spring Boot sang .NET thì project, config, build, test, ORM, layer và deploy thay đổi như thế nào.

> Ghi chú: Hiện nay Microsoft dùng tên ".NET" thay cho ".NET Core". Vì vậy "NET Core 8" trong keyword nên hiểu là ".NET 8", nền tảng .NET hiện đại, cross-platform, kế thừa từ .NET Core.

## 2. Tóm tắt ngắn gọn

.NET 8 là nền tảng backend hiện đại của Microsoft, thường dùng với ngôn ngữ C# và framework ASP.NET Core để xây dựng Web API, microservice, background service và hệ thống doanh nghiệp.

Nếu đã biết Spring Boot, có thể hiểu nhanh:

| Spring Boot | .NET 8 / ASP.NET Core |
| --- | --- |
| Java | C# |
| Spring Boot | ASP.NET Core |
| `application.yaml` / `application.properties` | `appsettings.json`, `appsettings.Development.json`, environment variables, user secrets |
| `pom.xml` / `build.gradle` | `.csproj`, `.sln`, NuGet, MSBuild, .NET CLI |
| Maven / Gradle | `dotnet restore`, `dotnet build`, `dotnet test`, `dotnet publish` |
| Spring IoC / Dependency Injection | Built-in Dependency Injection của ASP.NET Core |
| Spring MVC Controller | ASP.NET Core Controller hoặc Minimal API |
| Hibernate / Spring Data JPA | Entity Framework Core |
| Repository / Service / Controller | Controller / Service / Repository hoặc Clean Architecture |
| JUnit / Mockito | xUnit, NUnit hoặc MSTest; Moq, NSubstitute |
| `.jar` build artifact | `.dll`, `.exe`, publish folder, single-file executable hoặc container image |

Điểm quan trọng: .NET không chỉ thay đổi ngôn ngữ từ Java sang C#. Cách tổ chức project, cấu hình, dependency, migration database, test và publish cũng khác Spring Boot.

## 3. Khái niệm và mục đích sử dụng

.NET là nền tảng lập trình miễn phí, mã nguồn mở và cross-platform của Microsoft. .NET 8 là bản Long Term Support (LTS), phát hành ngày 14/11/2023 và được hỗ trợ đến ngày 10/11/2026 theo chính sách của Microsoft.

Trong backend, .NET 8 thường đi cùng ASP.NET Core để xây dựng:

- RESTful API.
- Microservice.
- Realtime service với SignalR.
- Background worker service.
- Ứng dụng cloud-native/container.
- Hệ thống enterprise cần hiệu năng và độ ổn định cao.

Với người đã biết Spring Boot, mục tiêu nghiên cứu .NET 8 không nên dừng ở việc liệt kê runtime, SDK hay C# 12. Những phần cần hiểu sâu hơn là cách .NET giải quyết các vấn đề backend quen thuộc: cấu hình môi trường, dependency injection, build, test, ORM, migration, phân layer và deploy.

## 4. Cấu trúc project: có giống Spring Boot không?

Spring Boot thường tổ chức project theo package:

```text
src/main/java/com/example/demo
├── controller
├── service
├── repository
├── entity
└── dto

src/main/resources
└── application.yaml

src/test/java
```

ASP.NET Core cũng có thể tổ chức theo layer tương tự, nhưng không bắt buộc theo một format duy nhất. Một cấu trúc đơn giản thường gặp:

```text
MyApp.Api
├── Controllers
├── Services
├── Repositories
├── Models
├── DTOs
├── Data
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
└── MyApp.Api.csproj

MyApp.Tests
├── Services
├── Controllers
└── MyApp.Tests.csproj

MyApp.sln
```

Với project lớn hơn, .NET thường tách thành nhiều project con trong cùng một solution:

```text
MyApp.sln
├── MyApp.Api
├── MyApp.Application
├── MyApp.Domain
├── MyApp.Infrastructure
└── MyApp.Tests
```

Cách này gần với Clean Architecture:

- `Api`: controller, request/response, middleware, DI setup.
- `Application`: use case, business service, interface.
- `Domain`: entity, domain model, business rule thuần.
- `Infrastructure`: database, EF Core, repository implementation, external service.
- `Tests`: unit test và integration test.

Kết luận: .NET có thể chia layer giống Spring Boot, nhưng trong thực tế còn hay tách project theo boundary rõ hơn. Người mới nên bắt đầu với folder đơn giản, sau đó mới chuyển sang nhiều project khi hệ thống lớn.

## 5. File cấu hình: `application.yaml` bên Spring Boot tương đương gì?

Bên Spring Boot, cấu hình thường nằm trong:

```text
application.yaml
application-dev.yaml
application-prod.yaml
```

Bên ASP.NET Core, cấu hình thường nằm trong:

```text
appsettings.json
appsettings.Development.json
appsettings.Production.json
Properties/launchSettings.json
environment variables
user secrets
```

Ví dụ Spring Boot:

```yaml
server:
  port: 8080

spring:
  datasource:
    url: jdbc:postgresql://localhost:5432/shopdb
    username: postgres
    password: 123456
```

Ví dụ tương đương trong .NET:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=shopdb;Username=postgres;Password=123456"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

Đọc config trong `Program.cs`:

```csharp
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
```

Một số điểm cần chú ý:

- `appsettings.json` giống cấu hình mặc định.
- `appsettings.Development.json` giống profile dev.
- `ASPNETCORE_ENVIRONMENT` tương đương ý tưởng active profile.
- `Properties/launchSettings.json` chủ yếu dùng khi chạy local bằng Visual Studio hoặc `dotnet run`, ví dụ cấu hình URL/port dev.
- Secret như password, API key không nên hard-code trong `appsettings.json`. Khi dev có thể dùng `dotnet user-secrets`; khi production nên dùng environment variables hoặc secret manager.

## 6. Maven/Gradle bên Java tương đương gì trong .NET?

Bên Java:

- `pom.xml` dùng với Maven.
- `build.gradle` dùng với Gradle.
- Dependency khai báo trong `dependencies`.
- Build bằng `mvn package` hoặc `gradle build`.

Bên .NET:

- `.csproj` là file project, chứa target framework, package reference và build config.
- `.sln` là solution, gom nhiều project lại với nhau.
- NuGet là package manager.
- MSBuild là build engine phía sau.
- .NET CLI là công cụ thao tác chính.

Ví dụ file `.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.0" />
  </ItemGroup>
</Project>
```

Các lệnh thường dùng:

```bash
dotnet restore
dotnet build
dotnet run
dotnet test
dotnet publish
dotnet add package Microsoft.EntityFrameworkCore
dotnet add reference ../MyApp.Application/MyApp.Application.csproj
```

So sánh nhanh:

| Nhu cầu | Spring Boot | .NET 8 |
| --- | --- | --- |
| Khai báo dependency | `pom.xml`, `build.gradle` | `.csproj` |
| Package manager | Maven Central | NuGet |
| Build | `mvn package`, `gradle build` | `dotnet build` |
| Test | `mvn test`, `gradle test` | `dotnet test` |
| Tạo artifact deploy | `.jar` | `dotnet publish` tạo `.dll`, `.exe` hoặc publish folder |
| Multi-module | Maven multi-module, Gradle multi-project | `.sln` gồm nhiều `.csproj` |

## 7. Test trong .NET khác gì Java?

Bên Java/Spring Boot, test thường nằm trong:

```text
src/test/java
```

Bên .NET, test thường được tách thành project riêng:

```text
MyApp.Api
MyApp.Tests
```

Tạo test project:

```bash
dotnet new xunit -n MyApp.Tests
dotnet add MyApp.Tests/MyApp.Tests.csproj reference MyApp.Api/MyApp.Api.csproj
dotnet test
```

Các framework test phổ biến:

- xUnit: phổ biến trong cộng đồng .NET hiện đại.
- NUnit: giống phong cách test truyền thống, nhiều feature.
- MSTest: framework test chính thức từ Microsoft.

Các thư viện hỗ trợ thường gặp:

- Moq hoặc NSubstitute: mock dependency, tương tự Mockito.
- FluentAssertions: viết assert dễ đọc.
- Microsoft.AspNetCore.Mvc.Testing: integration test Web API với `WebApplicationFactory`.
- Testcontainers for .NET: chạy database thật bằng container khi test integration.

Ví dụ unit test với xUnit:

```csharp
public class ProductServiceTests
{
    [Fact]
    public void GetExpensiveProducts_ShouldReturnProductsWithPriceFrom50()
    {
        var products = new List<Product>
        {
            new("Laptop", 1500),
            new("Mouse", 20),
            new("Keyboard", 50)
        };

        var result = products
            .Where(product => product.Price >= 50)
            .ToList();

        Assert.Equal(2, result.Count);
    }
}

public record Product(string Name, decimal Price);
```

Điểm khác biệt lớn: trong .NET, test là một project riêng và reference tới project cần test. Cách này làm dependency rõ hơn, nhất là khi solution có nhiều layer.

## 8. Tương tác dữ liệu: Hibernate/JPA tương đương gì?

Bên Spring Boot, stack phổ biến là:

- Hibernate.
- Spring Data JPA.
- Repository interface.
- Entity annotation như `@Entity`, `@Table`, `@Id`.
- Migration bằng Flyway hoặc Liquibase.

Bên .NET, stack phổ biến là:

- Entity Framework Core.
- `DbContext`.
- `DbSet<TEntity>`.
- LINQ query.
- Migration bằng EF Core Migrations.
- Mapping bằng Data Annotations hoặc Fluent API.

Ví dụ entity trong .NET:

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

Ví dụ `DbContext`:

```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
}
```

Đăng ký database trong `Program.cs`:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
```

Query dữ liệu:

```csharp
var products = await dbContext.Products
    .Where(product => product.Price >= 50)
    .OrderByDescending(product => product.Price)
    .ToListAsync();
```

Migration:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

So sánh nhanh:

| Spring Boot / Hibernate | .NET / EF Core |
| --- | --- |
| `@Entity` class | Entity class |
| `JpaRepository<Product, Long>` | `DbSet<Product>` hoặc repository tự viết |
| JPQL / Criteria API | LINQ |
| `EntityManager` | `DbContext` |
| Hibernate migration thường kết hợp Flyway/Liquibase | EF Core Migrations |
| `application.yaml` datasource | `ConnectionStrings` trong `appsettings.json` |

Điểm cần nghiên cứu kỹ: EF Core không hoàn toàn giống Hibernate. Cần học tracking, no-tracking query, lazy/eager loading, migration, transaction, concurrency và cách tối ưu query để tránh N+1.

## 9. Có cần chia layer như Spring Boot không?

Có, nhưng mức độ tùy kích thước project.

Với demo nhỏ:

```text
Controllers
Services
Repositories
Models
DTOs
Data
```

Với project thực tế:

```text
Api
Application
Domain
Infrastructure
Tests
```

Mapping tư duy từ Spring Boot:

| Layer trong Spring Boot | Layer tương đương trong .NET |
| --- | --- |
| Controller | Controller hoặc Minimal API endpoint |
| Service | Application service / Use case |
| Repository | Repository hoặc trực tiếp `DbContext` tùy kiến trúc |
| Entity | Domain entity hoặc EF Core entity |
| DTO | Request/Response DTO |
| Mapper | AutoMapper hoặc mapping thủ công |
| Config class | Options pattern, extension method đăng ký service |

Lưu ý quan trọng: Không nên bê nguyên xi mọi pattern từ Spring Boot sang .NET. Ví dụ, trong EF Core, nhiều team không tạo repository generic vì `DbContext` đã có vai trò gần giống Unit of Work + Repository. Nếu project cần clean boundary hoặc test dễ hơn thì vẫn có thể tạo repository riêng.

## 10. Có cần build trước rồi chạy file build không?

Trong lúc dev:

```bash
dotnet run
```

Lệnh này tự build rồi chạy app, tương tự trải nghiệm chạy Spring Boot bằng IDE hoặc `mvn spring-boot:run`.

Khi cần kiểm tra build:

```bash
dotnet build
```

Khi cần tạo bản deploy:

```bash
dotnet publish -c Release -o ./publish
```

Sau publish, output thường gồm:

```text
publish/
├── MyApp.Api.dll
├── MyApp.Api.exe
├── appsettings.json
├── appsettings.Production.json
└── các dependency cần thiết
```

Chạy app đã publish:

```bash
dotnet MyApp.Api.dll
```

Tùy cách publish, artifact có thể là:

- Framework-dependent deployment: tạo `.dll`, máy/server cần cài .NET runtime phù hợp.
- Self-contained deployment: đóng kèm runtime, có thể chạy bằng file `.exe` trên Windows hoặc binary tương ứng trên Linux.
- Single-file publish: gom app thành một file thực thi.
- Container image: đóng gói app vào Docker image.

So với Spring Boot:

| Spring Boot | .NET 8 |
| --- | --- |
| Build ra `.jar` | Build/publish ra `.dll`, `.exe`, folder publish hoặc container |
| Chạy `java -jar app.jar` | Chạy `dotnet App.dll` hoặc chạy executable |
| JVM cần có trên server | .NET runtime cần có nếu framework-dependent |
| Fat jar chứa dependency | Publish folder hoặc self-contained chứa dependency/runtime |

## 11. Demo nhỏ: Minimal API + config + service

Ví dụ này giúp thấy các phần tương đương Spring Boot trong .NET:

```bash
dotnet new web -n DotNet8ApiDemo
cd DotNet8ApiDemo
dotnet run
```

`appsettings.json`:

```json
{
  "AppSettings": {
    "ApplicationName": "DotNet8ApiDemo"
  }
}
```

`Program.cs`:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ProductService>();

var app = builder.Build();

app.MapGet("/", (IConfiguration configuration) =>
{
    var appName = configuration["AppSettings:ApplicationName"];
    return $"{appName} is running";
});

app.MapGet("/products", (ProductService service) =>
{
    return service.GetProducts();
});

app.Run();

public class ProductService
{
    public IReadOnlyList<Product> GetProducts()
    {
        return new List<Product>
        {
            new Product(1, "Laptop", 1500),
            new Product(2, "Mouse", 20),
            new Product(3, "Keyboard", 50)
        };
    }
}

public record Product(int Id, string Name, decimal Price);
```

Trong ví dụ này:

- `Program.cs` giống nơi cấu hình app, routing và DI.
- `appsettings.json` giống `application.yaml`.
- `builder.Services.AddScoped<ProductService>()` giống đăng ký bean/service.
- `app.MapGet` giống khai báo endpoint.
- `ProductService` giống service layer.

## 12. Từ Spring Boot migrate sang C#/.NET cần nghiên cứu thêm gì?

Những phần cần học theo thứ tự ưu tiên:

1. C# căn bản: type system, class, interface, record, nullable reference types, async/await, LINQ.
2. .NET CLI và project structure: `.sln`, `.csproj`, `dotnet restore/build/run/test/publish`.
3. ASP.NET Core request pipeline: `Program.cs`, middleware, routing, controller, Minimal API.
4. Dependency Injection: lifetime `Singleton`, `Scoped`, `Transient`.
5. Configuration: `appsettings.json`, environment-specific config, environment variables, user secrets, options pattern.
6. Logging: built-in logging, log level, structured logging, Serilog nếu cần.
7. Validation và DTO: Data Annotations, FluentValidation, request/response model.
8. Entity Framework Core: `DbContext`, `DbSet`, LINQ, migration, tracking, relationship mapping, transaction.
9. Authentication/Authorization: JWT Bearer, policy-based authorization, claims.
10. Error handling: exception middleware, ProblemDetails, validation error response.
11. Testing: xUnit/NUnit/MSTest, mocking, integration test, test database.
12. Deployment: `dotnet publish`, runtime-dependent/self-contained, Docker, CI/CD.
13. Clean Architecture hoặc Vertical Slice Architecture: cách chia layer phù hợp với .NET thay vì copy nguyên cấu trúc Spring Boot.

## 13. Kiến thức rút ra

Sau khi nghiên cứu, có thể rút ra:

- .NET 8 tương đương vai trò nền tảng runtime + SDK, còn ASP.NET Core mới là framework web giống Spring Boot.
- `application.yaml` không phải file mặc định trong .NET; file tương đương thường là `appsettings.json`.
- Maven/Gradle không có bản tương đương 1-1; trong .NET cần hiểu `.csproj`, NuGet, MSBuild và .NET CLI.
- Test trong .NET thường là project riêng, chạy bằng `dotnet test`.
- ORM phổ biến là EF Core, nhưng cần học cách EF Core query, tracking và migration vì không giống Hibernate hoàn toàn.
- Chia layer là cần thiết với backend thực tế, nhưng cách chia nên theo convention .NET và quy mô dự án.
- Khi deploy, .NET có nhiều kiểu artifact: `.dll`, `.exe`, publish folder, single-file hoặc container image.

## 14. Khó khăn khi chuyển từ Spring Boot sang .NET

- Dễ nhầm `.NET 8` với `ASP.NET Core`: .NET là nền tảng, ASP.NET Core là web framework.
- Dễ tìm `application.yaml` nhưng .NET mặc định dùng JSON config.
- Dễ hiểu nhầm `.csproj` chỉ là metadata, trong khi nó tương đương nơi quản lý dependency/build config như `pom.xml`.
- Dễ lạm dụng Repository pattern giống Spring Data JPA, trong khi EF Core đã có `DbContext`.
- Cần làm quen với LINQ thay vì JPQL/Criteria API.
- Cần hiểu DI lifetime vì `Scoped` rất quan trọng trong Web API, đặc biệt khi dùng `DbContext`.
- Cách publish không giống tạo một file `.jar`; cần chọn runtime-dependent, self-contained, single-file hoặc container tùy môi trường.

## 15. Kết luận

Báo cáo research .NET 8 nên tập trung vào việc map các khái niệm backend quen thuộc từ Spring Boot sang .NET. Những khái niệm như runtime, SDK, C# 12 hay NuGet là cần biết, nhưng chưa đủ. Muốn nghiên cứu có giá trị thực tế, cần trả lời được các câu hỏi: cấu hình đặt ở đâu, build bằng gì, test tổ chức thế nào, ORM nào thay Hibernate, chia layer ra sao và deploy artifact dạng gì.

Với góc nhìn đó, .NET 8 là nền tảng cần học trước, ASP.NET Core là bước tiếp theo để xây dựng Web API, và EF Core là phần quan trọng nếu hệ thống có database. Người mới chuyển từ Spring Boot nên học theo hướng so sánh như trên để giảm nhầm lẫn và nhanh chóng áp dụng vào project thật.

## 16. Tài liệu tham khảo

- Microsoft Learn - Introduction to .NET: https://learn.microsoft.com/en-us/dotnet/core/introduction
- Microsoft Learn - .NET CLI overview: https://learn.microsoft.com/en-us/dotnet/core/tools/
- Microsoft Learn - Configuration in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/
- Microsoft Learn - Dependency injection in ASP.NET Core: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
- Microsoft Learn - Test ASP.NET Core apps: https://learn.microsoft.com/en-us/aspnet/core/test/
- Microsoft Learn - Entity Framework Core: https://learn.microsoft.com/en-us/ef/core/
- Microsoft Learn - .NET application publishing overview: https://learn.microsoft.com/en-us/dotnet/core/deploying/
- Microsoft .NET Support Policy: https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core
