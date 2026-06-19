# Lab mini project: Product Management Web API with Docker

## 1. Mục tiêu

Lab này tạo một mini project ASP.NET Core Web API đơn giản và chạy bằng Docker Compose.

Nội dung project bao phủ các kiến thức đã research:

- ASP.NET Core Web API
- Controller, endpoint và routing
- HTTP method: GET, POST, PUT, DELETE
- Request body, response JSON và status code
- Middleware pipeline cơ bản
- Dependency Injection
- Swagger/OpenAPI
- LINQ
- Entity Framework Core theo hướng Database First
- SQL Server chạy bằng Docker

Lab không làm các phần nâng cao như login, JWT, role, frontend, AutoMapper, repository pattern, clean architecture hay unit test.

## 2. Mô tả mini project

Project tên `ProductApi`.

Chức năng: quản lý sản phẩm và danh mục sản phẩm.

Database có 2 bảng:

- `Categories`
- `Products`

Quan hệ:

- Một category có nhiều product.
- Một product thuộc về một category.

## 3. Cấu trúc folder

```text
lab-mini-project-product-api-docker/
  docker-compose.yml
  README.md
  database/
    init/
      01-create-database.sql
  docs/
    api-test-cases.md
    learning-map.md
  src/
    ProductApi/
      Dockerfile
      ProductApi.csproj
      Program.cs
      appsettings.json
      Controllers/
      Dtos/
      Models/
      Services/
```

## 4. Cách chạy project bằng Docker

Yêu cầu máy đã cài Docker Desktop.

Tại folder lab này, chạy:

```bash
docker compose up --build
```

Sau khi container chạy xong, mở Swagger:

```text
http://localhost:5000/swagger
```

SQL Server được expose ra máy host:

```text
Server: localhost,1433
Database: ProductManagementDb
User: sa
Password: YourStrong@Passw0rd
```

## 5. Các service trong Docker Compose

| Service | Vai trò |
| --- | --- |
| `sqlserver` | Chạy SQL Server |
| `db-init` | Tạo database, bảng và data mẫu |
| `api` | Chạy ASP.NET Core Web API |

## 6. API cần có

### Category

| Method | URL | Chức năng |
| --- | --- | --- |
| GET | `/api/categories` | Lấy danh sách danh mục |
| GET | `/api/categories/{id}` | Lấy chi tiết danh mục |
| POST | `/api/categories` | Tạo danh mục |
| PUT | `/api/categories/{id}` | Cập nhật danh mục |
| DELETE | `/api/categories/{id}` | Xóa danh mục |

### Product

| Method | URL | Chức năng |
| --- | --- | --- |
| GET | `/api/products` | Lấy danh sách sản phẩm, có filter và paging |
| GET | `/api/products/{id}` | Lấy chi tiết sản phẩm |
| POST | `/api/products` | Tạo sản phẩm |
| PUT | `/api/products/{id}` | Cập nhật sản phẩm |
| DELETE | `/api/products/{id}` | Xóa sản phẩm |

Endpoint danh sách product có query string:

```text
GET /api/products?keyword=logitech&categoryId=2&minPrice=300000&maxPrice=1000000&page=1&pageSize=10
```

## 7. Điểm cần quan sát khi học

### Program.cs

Ôn lại:

- `AddControllers`
- `AddSwaggerGen`
- `AddDbContext`
- `AddScoped`
- `UseSwagger`
- `UseHttpsRedirection`
- `MapControllers`

### Controller

Controller nhận request và trả response:

- `Ok`
- `CreatedAtAction`
- `NoContent`
- `NotFound`
- `BadRequest`

### Service

Service chứa logic cơ bản và dùng DbContext để truy vấn database.

### EF Core Database First

Trong lab này, database được tạo trước bằng SQL script:

```text
database/init/01-create-database.sql
```

Folder `Models` chứa entity và `ProductManagementDbContext` tương ứng với database. Khi học thực tế, có thể tạo các file này bằng lệnh scaffold:

```bash
dotnet ef dbcontext scaffold "Server=localhost,1433;Database=ProductManagementDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -c ProductManagementDbContext --force
```

### LINQ

Product list dùng các method:

- `Where`
- `Include`
- `OrderBy`
- `Skip`
- `Take`
- `Select`
- `FirstOrDefaultAsync`
- `AnyAsync`
- `ToListAsync`

## 8. Dừng container

Dừng container:

```bash
docker compose down
```

Dừng container và xóa data SQL Server:

```bash
docker compose down -v
```

## 9. Kết quả cần đạt

Sau khi hoàn thành lab, bạn cần nắm được luồng cơ bản:

```text
Swagger/Postman
  -> Controller
  -> Service
  -> DbContext
  -> SQL Server container
  -> DbContext
  -> Service
  -> Controller
  -> JSON response
```
