# Báo cáo research: Entity Framework Core theo hướng Database First

## 1. Thông tin chung

- Keyword: Entity Framework Core Database First
- Ngày research: 19/06/2026
- Phạm vi: Tìm hiểu cơ bản cho người mới học ASP.NET Core, đặc biệt là người đã quen Spring Boot
- Mục tiêu: Hiểu Entity Framework Core là gì, Database First là gì, cách tạo model từ database có sẵn, cách dùng `DbContext`, `DbSet`, LINQ và cách kết nối với ASP.NET Core Web API.

## 2. Tóm tắt ngắn gọn

Entity Framework Core, thường viết tắt là EF Core, là ORM của .NET. ORM là công cụ giúp lập trình viên làm việc với database thông qua object trong code thay vì phải viết SQL thủ công cho mọi thao tác.

Nếu đã quen Spring Boot, có thể hiểu EF Core gần giống với Hibernate/JPA:

| Spring Boot | ASP.NET Core / EF Core |
| --- | --- |
| Hibernate / JPA | Entity Framework Core |
| Entity class | Entity class |
| `EntityManager` | `DbContext` |
| `Repository` / `JpaRepository` | `DbSet<T>` kết hợp service/repository |
| JPQL / Criteria / method query | LINQ |
| `application.properties` / `application.yml` | `appsettings.json` |
| `@Entity`, `@Table`, `@Column` | Fluent API hoặc attribute trong C# |

Database First là cách làm khi database đã tồn tại trước. Lập trình viên dùng EF Core để sinh ra các class C# từ bảng trong database. Cách này phù hợp khi dự án đã có database sẵn, hoặc database do team khác thiết kế trước.

## 3. EF Core là gì?

EF Core là thư viện giúp ứng dụng .NET làm việc với database theo hướng object.

Thay vì viết SQL như:

```sql
SELECT * FROM Products WHERE Price > 100;
```

Trong C# có thể viết bằng LINQ:

```csharp
var products = await context.Products
    .Where(p => p.Price > 100)
    .ToListAsync();
```

EF Core sẽ chuyển câu LINQ này thành SQL phù hợp với database đang sử dụng.

EF Core hỗ trợ nhiều hệ quản trị cơ sở dữ liệu như:

- SQL Server
- PostgreSQL
- MySQL
- SQLite
- Oracle, tùy provider được cài đặt

Trong ASP.NET Core Web API, EF Core thường được dùng để lấy dữ liệu, thêm mới, cập nhật và xóa dữ liệu trong database.

## 4. Database First là gì?

Database First nghĩa là database được thiết kế trước, sau đó code C# được sinh ra dựa trên database đó.

Quy trình cơ bản:

1. Có database sẵn, ví dụ SQL Server database tên `ShopDb`.
2. Cài các package EF Core cần thiết.
3. Chạy lệnh scaffold để sinh entity class và `DbContext`.
4. Đăng ký `DbContext` vào Dependency Injection trong `Program.cs`.
5. Dùng `DbContext` trong service hoặc controller để truy vấn dữ liệu.

Database First khác với Code First:

| Tiêu chí | Database First | Code First |
| --- | --- | --- |
| Bắt đầu từ đâu | Database có sẵn | Code C# có trước |
| Tạo entity | Sinh từ bảng database | Tự viết class C# |
| Tạo database | Database đã tồn tại | EF Core Migration tạo database |
| Phù hợp khi | Dự án có database cũ, database được thiết kế trước | Dự án mới, muốn quản lý schema bằng code |

Với người đã quen Spring Boot, Database First trong EF Core có thể hiểu giống tình huống đã có database, sau đó tạo entity mapping để ứng dụng Java/Spring làm việc với database đó.

## 5. Các thành phần chính trong EF Core DB First

### 5.1. Entity class

Entity class là class đại diện cho một bảng trong database.

Ví dụ bảng `Products` có thể sinh ra class:

```csharp
public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }
}
```

Trong Spring Boot, class này tương tự class có `@Entity`.

### 5.2. DbContext

`DbContext` là class trung tâm của EF Core. Nó đại diện cho một session làm việc với database.

Ví dụ:

```csharp
public partial class ShopDbContext : DbContext
{
    public ShopDbContext(DbContextOptions<ShopDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }
}
```

Có thể hiểu `DbContext` gần giống `EntityManager` trong JPA, nhưng trong ASP.NET Core thường được inject trực tiếp vào service hoặc repository.

### 5.3. DbSet

`DbSet<T>` đại diện cho một bảng hoặc tập entity trong database.

Ví dụ:

```csharp
context.Products
```

`Products` là `DbSet<Product>`, cho phép truy vấn, thêm, sửa, xóa dữ liệu trong bảng `Products`.

Các thao tác thường gặp:

```csharp
var products = await context.Products.ToListAsync();

var product = await context.Products.FindAsync(id);

context.Products.Add(newProduct);
await context.SaveChangesAsync();

context.Products.Remove(product);
await context.SaveChangesAsync();
```

## 6. Các package thường dùng

Khi dùng EF Core với SQL Server, thường cần các package:

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Ý nghĩa:

| Package | Vai trò |
| --- | --- |
| `Microsoft.EntityFrameworkCore.SqlServer` | Provider để EF Core kết nối SQL Server |
| `Microsoft.EntityFrameworkCore.Tools` | Cung cấp công cụ scaffold, migration trong Visual Studio Package Manager Console |
| `Microsoft.EntityFrameworkCore.Design` | Hỗ trợ design-time tool như `dotnet ef` |

Nếu dùng database khác, provider sẽ thay đổi. Ví dụ PostgreSQL thường dùng package `Npgsql.EntityFrameworkCore.PostgreSQL`.

## 7. Scaffold database thành code C#

Scaffold là quá trình đọc schema database rồi sinh entity class và `DbContext`.

Ví dụ với SQL Server:

```bash
dotnet ef dbcontext scaffold "Server=localhost;Database=ShopDb;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c ShopDbContext --context-dir Data --no-onconfiguring
```

Ý nghĩa một số phần quan trọng:

| Thành phần | Ý nghĩa |
| --- | --- |
| `dbcontext scaffold` | Lệnh sinh code từ database |
| Connection string | Thông tin kết nối database |
| `Microsoft.EntityFrameworkCore.SqlServer` | Provider database |
| `-o Models` | Thư mục chứa entity class |
| `-c ShopDbContext` | Tên class `DbContext` |
| `--context-dir Data` | Thư mục chứa `DbContext` |
| `--no-onconfiguring` | Không ghi connection string trực tiếp trong `DbContext` |

Nên dùng `--no-onconfiguring` để tránh connection string bị ghi cứng vào code. Connection string nên đặt trong `appsettings.json`, user secrets hoặc biến môi trường.

Sau khi scaffold, project thường có cấu trúc:

```text
MyWebApi
|-- Controllers
|-- Data
|   |-- ShopDbContext.cs
|-- Models
|   |-- Product.cs
|   |-- Category.cs
|-- appsettings.json
|-- Program.cs
|-- MyWebApi.csproj
```

## 8. Cấu hình connection string

Trong `appsettings.json` có thể khai báo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ShopDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Trong `Program.cs`, đăng ký `DbContext`:

```csharp
using Microsoft.EntityFrameworkCore;
using MyWebApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapControllers();

app.Run();
```

Trong Spring Boot, phần này tương tự cấu hình `spring.datasource.url`, `spring.datasource.username`, `spring.datasource.password` trong `application.properties`.

## 9. Truy vấn dữ liệu bằng EF Core

EF Core thường truy vấn database bằng LINQ.

Ví dụ lấy danh sách sản phẩm:

```csharp
[HttpGet]
public async Task<ActionResult<List<Product>>> GetProducts()
{
    var products = await context.Products.ToListAsync();
    return Ok(products);
}
```

Ví dụ lọc dữ liệu:

```csharp
[HttpGet("search")]
public async Task<ActionResult<List<Product>>> SearchProducts(string keyword)
{
    var products = await context.Products
        .Where(p => p.Name.Contains(keyword))
        .OrderBy(p => p.Name)
        .ToListAsync();

    return Ok(products);
}
```

Ví dụ lấy chi tiết theo id:

```csharp
[HttpGet("{id}")]
public async Task<ActionResult<Product>> GetProduct(int id)
{
    var product = await context.Products.FindAsync(id);

    if (product == null)
    {
        return NotFound();
    }

    return Ok(product);
}
```

## 10. Thêm, sửa, xóa dữ liệu

### 10.1. Thêm dữ liệu

```csharp
[HttpPost]
public async Task<ActionResult<Product>> CreateProduct(Product product)
{
    context.Products.Add(product);
    await context.SaveChangesAsync();

    return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
}
```

### 10.2. Cập nhật dữ liệu

```csharp
[HttpPut("{id}")]
public async Task<IActionResult> UpdateProduct(int id, Product product)
{
    if (id != product.Id)
    {
        return BadRequest();
    }

    context.Entry(product).State = EntityState.Modified;
    await context.SaveChangesAsync();

    return NoContent();
}
```

### 10.3. Xóa dữ liệu

```csharp
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteProduct(int id)
{
    var product = await context.Products.FindAsync(id);

    if (product == null)
    {
        return NotFound();
    }

    context.Products.Remove(product);
    await context.SaveChangesAsync();

    return NoContent();
}
```

Điểm cần nhớ: EF Core chỉ gửi thay đổi xuống database khi gọi `SaveChanges()` hoặc `SaveChangesAsync()`.

## 11. Service layer và cách tổ chức code

Trong demo nhỏ, có thể inject `DbContext` trực tiếp vào controller. Tuy nhiên trong dự án thực tế, nên tách logic sang service.

Ví dụ service:

```csharp
public class ProductService
{
    private readonly ShopDbContext context;

    public ProductService(ShopDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await context.Products
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
}
```

Đăng ký service:

```csharp
builder.Services.AddScoped<ProductService>();
```

Trong Spring Boot, cách này tương tự việc controller gọi service, service gọi repository.

## 12. Một số lưu ý quan trọng cho người mới học

### 12.1. Không nên trả thẳng entity trong dự án lớn

Với bài học cơ bản, trả thẳng entity từ API là dễ hiểu. Nhưng trong dự án thực tế, nên dùng DTO để kiểm soát dữ liệu trả về.

Ví dụ:

```csharp
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
}
```

DTO giúp tránh lộ dữ liệu không cần thiết, tránh vòng lặp quan hệ giữa các entity và giúp API ổn định hơn.

### 12.2. DbContext có lifetime Scoped

Trong ASP.NET Core, `DbContext` thường được đăng ký bằng `AddDbContext`, mặc định có lifetime là Scoped. Nghĩa là mỗi HTTP request thường có một instance `DbContext` riêng.

Không nên đăng ký `DbContext` là Singleton.

### 12.3. Dùng async khi truy vấn database

Nên dùng các method async như:

- `ToListAsync`
- `FirstOrDefaultAsync`
- `FindAsync`
- `SaveChangesAsync`

Điều này giúp API xử lý request tốt hơn khi phải chờ database.

### 12.4. Cẩn thận khi scaffold lại

Khi database thay đổi, có thể cần scaffold lại. Tuy nhiên scaffold lại có thể ghi đè file entity hoặc `DbContext`.

Cách làm an toàn:

- Không sửa logic nghiệp vụ trực tiếp trong file entity được scaffold.
- Dùng `partial class` nếu cần mở rộng entity.
- Commit code trước khi scaffold lại để dễ so sánh thay đổi.
- Tách DTO, service, controller ra khỏi code scaffold.

### 12.5. Migration không phải trọng tâm của Database First

Trong Code First, migration dùng để thay đổi database từ code. Trong Database First, database thường được quản lý bên ngoài, nên migration không phải phần chính.

Người mới học cần phân biệt rõ:

- Database First: database là nguồn chính.
- Code First: code C# và migration là nguồn chính.

## 13. So sánh nhanh với Spring Boot

Nếu chuyển từ Spring Boot sang ASP.NET Core, có thể nhớ các cặp khái niệm sau:

| Spring Boot | ASP.NET Core |
| --- | --- |
| `@RestController` | `[ApiController]` + `ControllerBase` |
| `@GetMapping`, `@PostMapping` | `[HttpGet]`, `[HttpPost]` |
| `@Service` | Class service + đăng ký DI |
| `@Autowired` / constructor injection | Constructor injection |
| `JpaRepository<Product, Integer>` | `DbSet<Product>` hoặc repository tự viết |
| `application.properties` | `appsettings.json` |
| `@Transactional` | Transaction của EF Core hoặc `Database.BeginTransaction()` khi cần |
| `save()` | `Add`, `Update`, `SaveChangesAsync` |
| `findById()` | `FindAsync` hoặc `FirstOrDefaultAsync` |
| JPQL | LINQ |

Điểm khác biệt lớn là ASP.NET Core không bắt buộc phải tạo repository giống Spring Data JPA. EF Core `DbContext` và `DbSet` đã cung cấp nhiều chức năng giống repository. Tuy nhiên vẫn có thể tự tạo repository nếu dự án cần tách kiến trúc rõ hơn.

## 14. Quy trình học đề xuất

Với người mới học EF Core DB First, nên học theo thứ tự:

1. Hiểu ORM là gì và EF Core dùng để làm gì.
2. Hiểu entity, `DbContext`, `DbSet`.
3. Tạo một database đơn giản có 2-3 bảng.
4. Scaffold database thành model C#.
5. Cấu hình connection string trong `appsettings.json`.
6. Đăng ký `DbContext` trong `Program.cs`.
7. Viết API GET để đọc dữ liệu.
8. Viết API POST, PUT, DELETE để thay đổi dữ liệu.
9. Tách DTO và service khi code bắt đầu dài.
10. Tìm hiểu quan hệ một-nhiều, nhiều-nhiều và lazy/eager loading.

## 15. Keyword quan trọng

| Keyword | Ý nghĩa ngắn gọn |
| --- | --- |
| EF Core | ORM của .NET dùng để làm việc với database |
| ORM | Kỹ thuật ánh xạ bảng database thành object trong code |
| Database First | Cách tạo code C# từ database có sẵn |
| Code First | Cách tạo database từ code C# và migration |
| Entity | Class đại diện cho bảng trong database |
| `DbContext` | Class trung tâm quản lý kết nối, entity và truy vấn |
| `DbSet<T>` | Đại diện cho một bảng hoặc tập entity |
| Scaffold | Sinh code từ database |
| Provider | Thư viện giúp EF Core làm việc với từng loại database |
| Connection string | Chuỗi thông tin kết nối database |
| LINQ | Cách viết truy vấn trong C# |
| `SaveChangesAsync` | Lệnh lưu thay đổi xuống database |
| DTO | Object dùng để truyền dữ liệu qua API |
| Migration | Cơ chế thay đổi database từ code, thường dùng trong Code First |

## 16. Kết luận

Entity Framework Core theo hướng Database First là lựa chọn phù hợp khi dự án ASP.NET Core cần làm việc với database đã có sẵn. Người mới học cần nắm rõ ba thành phần quan trọng nhất là entity, `DbContext` và `DbSet`.

Nếu đã quen Spring Boot, có thể xem EF Core như phần tương đương với Hibernate/JPA. Điểm cần làm quen là cách cấu hình trong `Program.cs`, cách dùng LINQ để truy vấn và cách scaffold database thành code C#.

Sau khi hiểu EF Core DB First, người học có thể kết hợp với ASP.NET Core Web API để xây dựng các chức năng CRUD cơ bản: lấy danh sách, xem chi tiết, thêm mới, cập nhật và xóa dữ liệu.

## 17. Tài liệu tham khảo

- Microsoft Learn - Entity Framework Core: https://learn.microsoft.com/en-us/ef/core/
- Microsoft Learn - Reverse Engineering / Scaffold Database: https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/
- Microsoft Learn - DbContext configuration: https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/
- Microsoft Learn - Querying data: https://learn.microsoft.com/en-us/ef/core/querying/
