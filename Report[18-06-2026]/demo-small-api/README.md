# Demo Small API

Demo Minimal API don gian bang .NET 8, dung de minh hoa cach mot backend developer tu Spring Boot co the tiep can ASP.NET Core.

## Noi dung demo

- `Program.cs`: cau hinh app, routing va dependency injection.
- `ProductStore`: service in-memory thay cho database.
- `Dockerfile`: build va chay API bang container.

## Endpoints

| Method | Endpoint | Mo ta |
| --- | --- | --- |
| GET | `/` | Kiem tra API dang chay |
| GET | `/products` | Lay danh sach san pham |
| GET | `/products/{id}` | Lay san pham theo id |
| POST | `/products` | Tao san pham moi |

## Chay bang .NET CLI

```bash
dotnet run
```

Vi du goi API:

```bash
curl http://localhost:8080/products
```

Port thuc te co the khac tuy theo output cua `dotnet run`.

## Chay bang Docker

Build image:

```bash
docker build -t demo-small-api .
```

Run container:

```bash
docker run --rm -p 8080:8080 demo-small-api
```

Goi API:

```bash
curl http://localhost:8080/products
```

Tao san pham moi:

```bash
curl -X POST http://localhost:8080/products ^
  -H "Content-Type: application/json" ^
  -d "{\"name\":\"Monitor\",\"price\":300}"
```

Neu dung Git Bash/Linux/macOS, thay `^` bang `\`.
