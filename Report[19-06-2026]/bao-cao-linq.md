# Báo cáo research: LINQ trong C# và ASP.NET Core

## 1. Thông tin chung

- Keyword: LINQ
- Ngày research: 19/06/2026
- Phạm vi: Tìm hiểu cơ bản cho người mới học ASP.NET Core, đặc biệt là người đã quen Spring Boot
- Mục tiêu: Hiểu LINQ là gì, dùng để làm gì, cú pháp cơ bản, cách dùng với collection và database thông qua Entity Framework Core.

## 2. Tóm tắt ngắn gọn

LINQ là viết tắt của **Language Integrated Query**, nghĩa là truy vấn dữ liệu được tích hợp trực tiếp vào ngôn ngữ C#.

Nếu trong Java/Spring Boot thường dùng:

- Java Stream API để lọc, map, sắp xếp dữ liệu trong list.
- Spring Data JPA hoặc JPQL để truy vấn database.
- Repository method như `findByNameContaining`, `findByStatus`, `findAll`.

Thì trong ASP.NET Core/C#, LINQ thường được dùng để:

- Lọc dữ liệu trong `List<T>`, array, collection.
- Chuyển đổi dữ liệu từ entity sang DTO.
- Sắp xếp, phân trang, nhóm dữ liệu.
- Viết query database thông qua Entity Framework Core.

Ví dụ ngắn:

```csharp
var activeUsers = users
    .Where(user => user.IsActive)
    .OrderBy(user => user.FullName)
    .ToList();
```

Đoạn code trên tương tự Java Stream:

```java
List<User> activeUsers = users.stream()
    .filter(user -> user.isActive())
    .sorted(Comparator.comparing(User::getFullName))
    .toList();
```

## 3. LINQ dùng để làm gì?

LINQ giúp viết các thao tác xử lý dữ liệu theo kiểu rõ ràng, ngắn gọn và dễ đọc hơn so với vòng lặp thủ công.

Các thao tác phổ biến:

| Nhu cầu | LINQ method |
| --- | --- |
| Lọc dữ liệu | `Where` |
| Lấy một phần tử | `First`, `FirstOrDefault`, `SingleOrDefault` |
| Chuyển đổi dữ liệu | `Select` |
| Sắp xếp | `OrderBy`, `OrderByDescending`, `ThenBy` |
| Đếm dữ liệu | `Count`, `LongCount` |
| Kiểm tra tồn tại | `Any`, `All` |
| Phân trang | `Skip`, `Take` |
| Nhóm dữ liệu | `GroupBy` |
| Nối dữ liệu | `Join` |
| Tính tổng, trung bình | `Sum`, `Average`, `Min`, `Max` |

## 4. Hai kiểu cú pháp LINQ

LINQ có hai kiểu viết chính.

### 4.1. Method syntax

Đây là kiểu thường gặp nhất trong ASP.NET Core.

```csharp
var result = products
    .Where(p => p.Price > 100)
    .OrderBy(p => p.Name)
    .Select(p => new
    {
        p.Id,
        p.Name,
        p.Price
    })
    .ToList();
```

### 4.2. Query syntax

Kiểu này nhìn gần giống SQL hơn.

```csharp
var result =
    from p in products
    where p.Price > 100
    orderby p.Name
    select new
    {
        p.Id,
        p.Name,
        p.Price
    };
```

Trong thực tế ASP.NET Core, method syntax được dùng nhiều hơn vì dễ nối chuỗi điều kiện và dễ kết hợp với Entity Framework Core.

## 5. Một số method LINQ quan trọng

### 5.1. Where

`Where` dùng để lọc dữ liệu theo điều kiện.

```csharp
var activeUsers = users
    .Where(u => u.IsActive)
    .ToList();
```

Tương tự Java Stream:

```java
users.stream()
    .filter(User::isActive)
    .toList();
```

### 5.2. Select

`Select` dùng để biến đổi dữ liệu. Trong ASP.NET Core, `Select` hay dùng để chuyển entity sang DTO.

```csharp
var userDtos = users
    .Select(u => new UserDto
    {
        Id = u.Id,
        FullName = u.FullName,
        Email = u.Email
    })
    .ToList();
```

Tư duy này giống việc map entity sang response DTO trong Spring Boot.

### 5.3. FirstOrDefault

`FirstOrDefault` lấy phần tử đầu tiên thỏa điều kiện. Nếu không có thì trả về `null` với reference type.

```csharp
var user = users.FirstOrDefault(u => u.Id == id);

if (user == null)
{
    return NotFound();
}
```

Trong Spring Boot, thao tác này gần giống:

```java
userRepository.findById(id).orElse(null);
```

### 5.4. Any

`Any` kiểm tra xem có phần tử nào thỏa điều kiện không.

```csharp
var exists = users.Any(u => u.Email == email);
```

Trong database, `Any` thường tốt hơn `Count() > 0` vì chỉ cần kiểm tra tồn tại, không cần đếm toàn bộ.

### 5.5. OrderBy và ThenBy

`OrderBy` dùng để sắp xếp chính, `ThenBy` dùng để sắp xếp phụ.

```csharp
var sortedUsers = users
    .OrderBy(u => u.LastName)
    .ThenBy(u => u.FirstName)
    .ToList();
```

### 5.6. Skip và Take

`Skip` và `Take` thường dùng để phân trang.

```csharp
var page = 1;
var pageSize = 10;

var usersPage = users
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToList();
```

Trong API thực tế:

```text
GET /api/users?page=1&pageSize=10
```

### 5.7. GroupBy

`GroupBy` dùng để gom nhóm dữ liệu.

```csharp
var usersByRole = users
    .GroupBy(u => u.Role)
    .Select(group => new
    {
        Role = group.Key,
        Total = group.Count()
    })
    .ToList();
```

Ví dụ kết quả:

```text
Admin: 2
User: 10
Manager: 3
```

## 6. IEnumerable và IQueryable

Đây là phần rất quan trọng khi chuyển từ Spring Boot sang ASP.NET Core.

### 6.1. IEnumerable

`IEnumerable<T>` thường dùng cho dữ liệu đã nằm trong memory, ví dụ `List<T>`, array.

```csharp
IEnumerable<User> users = GetUsersFromMemory();

var activeUsers = users
    .Where(u => u.IsActive)
    .ToList();
```

Khi dùng `IEnumerable`, việc lọc thường diễn ra trong ứng dụng C#.

### 6.2. IQueryable

`IQueryable<T>` thường dùng khi làm việc với database thông qua Entity Framework Core.

```csharp
IQueryable<User> query = _dbContext.Users;

var activeUsers = await query
    .Where(u => u.IsActive)
    .ToListAsync();
```

Khi dùng `IQueryable`, LINQ query có thể được Entity Framework Core dịch thành SQL và chạy ở database.

Ví dụ LINQ:

```csharp
var users = await _dbContext.Users
    .Where(u => u.IsActive)
    .OrderBy(u => u.FullName)
    .ToListAsync();
```

Có thể được dịch gần giống SQL:

```sql
SELECT *
FROM Users
WHERE IsActive = 1
ORDER BY FullName;
```

### 6.3. So sánh nhanh

| Tiêu chí | IEnumerable | IQueryable |
| --- | --- | --- |
| Dữ liệu thường ở đâu? | Trong memory | Database hoặc nguồn query |
| Query chạy ở đâu? | Trong ứng dụng | Có thể chạy ở database |
| Dùng phổ biến với | `List<T>`, array | EF Core `DbSet<T>` |
| Async database | Không dùng `ToListAsync` | Dùng được `ToListAsync` |

Khi làm Web API với EF Core, nên giữ query ở dạng `IQueryable` cho đến khi gọi `ToListAsync`, `FirstOrDefaultAsync`, `CountAsync`.

## 7. Deferred execution

LINQ có khái niệm **deferred execution**, nghĩa là query chưa chạy ngay khi được khai báo. Query chỉ chạy khi dữ liệu thật sự được lấy ra.

Ví dụ:

```csharp
var query = _dbContext.Users
    .Where(u => u.IsActive);

// Query database chưa chạy ở dòng trên.

var users = await query.ToListAsync();

// Query database chạy ở dòng ToListAsync.
```

Các method thường làm query chạy:

- `ToList`
- `ToArray`
- `First`
- `FirstOrDefault`
- `Single`
- `SingleOrDefault`
- `Count`
- `Any`

Với EF Core async:

- `ToListAsync`
- `FirstOrDefaultAsync`
- `SingleOrDefaultAsync`
- `CountAsync`
- `AnyAsync`

## 8. LINQ trong ASP.NET Core Web API với EF Core

Ví dụ entity:

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

Ví dụ DTO:

```csharp
public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

Ví dụ controller:

```csharp
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ProductsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponse>>> GetProducts(
        string? keyword,
        int page = 1,
        int pageSize = 10)
    {
        var query = _dbContext.Products
            .Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(p => p.Name.Contains(keyword));
        }

        var products = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            })
            .ToListAsync();

        return Ok(products);
    }
}
```

Điểm cần chú ý trong ví dụ trên:

- `_dbContext.Products` là `DbSet<Product>`, có thể query bằng LINQ.
- `Where` lọc sản phẩm đang active.
- Nếu có `keyword`, query được nối thêm điều kiện tìm kiếm.
- `OrderByDescending` sắp xếp sản phẩm mới trước.
- `Skip` và `Take` dùng để phân trang.
- `Select` chuyển entity sang DTO.
- `ToListAsync` là lúc query được thực thi xuống database.

## 9. Liên hệ với Spring Boot

| Spring Boot / Java | ASP.NET Core / C# |
| --- | --- |
| Java Stream API | LINQ trên `IEnumerable` |
| `filter` | `Where` |
| `map` | `Select` |
| `sorted` | `OrderBy`, `OrderByDescending` |
| `limit` | `Take` |
| `skip` | `Skip` |
| `findFirst` | `FirstOrDefault` |
| `anyMatch` | `Any` |
| Spring Data JPA Repository | EF Core `DbContext` / `DbSet` |
| JPQL / Criteria API | LINQ trên `IQueryable` |
| Entity to DTO mapping | `Select` sang DTO |

Điểm khác biệt quan trọng:

- Java Stream thường xử lý collection trong memory.
- LINQ có thể xử lý collection trong memory hoặc được EF Core dịch thành SQL.
- Trong Spring Boot, query database thường nằm trong repository method hoặc JPQL.
- Trong ASP.NET Core với EF Core, query database thường được viết trực tiếp bằng LINQ trên `DbSet`.

## 10. Một số lỗi người mới hay gặp

### 10.1. Gọi ToList quá sớm

Không nên:

```csharp
var products = await _dbContext.Products.ToListAsync();

var result = products
    .Where(p => p.IsActive)
    .Take(10)
    .ToList();
```

Vấn đề: Dữ liệu bị lấy hết từ database lên memory trước, sau đó mới lọc.

Nên viết:

```csharp
var result = await _dbContext.Products
    .Where(p => p.IsActive)
    .Take(10)
    .ToListAsync();
```

Lúc này database chỉ trả về dữ liệu cần thiết.

### 10.2. Dùng Count() > 0 thay vì Any()

Không nên:

```csharp
var exists = await _dbContext.Users
    .CountAsync(u => u.Email == email) > 0;
```

Nên viết:

```csharp
var exists = await _dbContext.Users
    .AnyAsync(u => u.Email == email);
```

`Any` thể hiện đúng mục đích là kiểm tra có tồn tại hay không.

### 10.3. Quên xử lý null với FirstOrDefault

```csharp
var user = await _dbContext.Users
    .FirstOrDefaultAsync(u => u.Id == id);

if (user == null)
{
    return NotFound();
}
```

`FirstOrDefaultAsync` có thể trả về `null`, vì vậy cần kiểm tra trước khi dùng.

### 10.4. Nhầm First và Single

| Method | Ý nghĩa |
| --- | --- |
| `First` | Lấy phần tử đầu tiên, lỗi nếu không có |
| `FirstOrDefault` | Lấy phần tử đầu tiên, không có thì trả về default/null |
| `Single` | Yêu cầu chỉ có đúng một phần tử, lỗi nếu không có hoặc có nhiều hơn một |
| `SingleOrDefault` | Không có thì trả về default/null, nhưng lỗi nếu có nhiều hơn một |

Với API tìm theo id, thường dùng `FirstOrDefaultAsync` hoặc `FindAsync`.

## 11. Gợi ý cách dùng LINQ trong project thực tế

Khi viết API danh sách:

```csharp
var query = _dbContext.Products.AsQueryable();

if (!string.IsNullOrWhiteSpace(keyword))
{
    query = query.Where(p => p.Name.Contains(keyword));
}

if (minPrice.HasValue)
{
    query = query.Where(p => p.Price >= minPrice.Value);
}

var total = await query.CountAsync();

var items = await query
    .OrderBy(p => p.Name)
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .Select(p => new ProductResponse
    {
        Id = p.Id,
        Name = p.Name,
        Price = p.Price
    })
    .ToListAsync();
```

Cách viết này giúp:

- Tách từng điều kiện lọc rõ ràng.
- Chỉ query database khi cần.
- Dễ thêm search, filter, sort, paging.
- Trả DTO thay vì trả trực tiếp entity.

## 12. Keyword cần nhớ

| Keyword | Ý nghĩa ngắn gọn |
| --- | --- |
| LINQ | Cách truy vấn dữ liệu trực tiếp trong C# |
| `Where` | Lọc dữ liệu |
| `Select` | Chuyển đổi dữ liệu |
| `OrderBy` | Sắp xếp tăng dần |
| `OrderByDescending` | Sắp xếp giảm dần |
| `Skip` | Bỏ qua một số dòng |
| `Take` | Lấy một số dòng |
| `Any` | Kiểm tra có tồn tại hay không |
| `Count` | Đếm số lượng |
| `GroupBy` | Gom nhóm dữ liệu |
| `IEnumerable` | Query dữ liệu trong memory |
| `IQueryable` | Query có thể được dịch sang nguồn dữ liệu như database |
| Deferred execution | Query chưa chạy cho đến khi lấy dữ liệu thật |
| `ToList` | Thực thi query và đưa kết quả về list |
| `ToListAsync` | Thực thi query database theo kiểu async trong EF Core |
| EF Core | ORM của .NET, tương tự Hibernate/JPA ở Java |
| `DbContext` | Lớp đại diện cho database session/context |
| `DbSet` | Đại diện cho một bảng/entity set trong database |

## 13. Kết luận

LINQ là kiến thức nền tảng khi học C# và ASP.NET Core. Với người đã học Spring Boot, có thể hiểu LINQ theo hai hướng:

- Khi làm việc với collection trong memory, LINQ gần giống Java Stream API.
- Khi làm việc với database thông qua EF Core, LINQ gần giống cách viết query trong repository hoặc JPQL, nhưng được viết trực tiếp bằng C#.

Khi xây dựng ASP.NET Core Web API, LINQ thường xuất hiện trong service hoặc controller để lọc dữ liệu, tìm kiếm, sắp xếp, phân trang và map entity sang DTO. Người mới nên nắm chắc các method như `Where`, `Select`, `FirstOrDefault`, `Any`, `OrderBy`, `Skip`, `Take`, đồng thời hiểu rõ khác biệt giữa `IEnumerable` và `IQueryable` để tránh query database không hiệu quả.
