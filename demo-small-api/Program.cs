var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProductStore>();

var app = builder.Build();

app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        name = "Demo Small API",
        status = "running",
        framework = ".NET 8 Minimal API"
    });
});

app.MapGet("/products", (ProductStore store) =>
{
    return Results.Ok(store.GetAll());
});

app.MapGet("/products/{id:int}", (int id, ProductStore store) =>
{
    var product = store.GetById(id);

    return product is null
        ? Results.NotFound(new { message = $"Product with id {id} was not found." })
        : Results.Ok(product);
});

app.MapPost("/products", (CreateProductRequest request, ProductStore store) =>
{
    if (string.IsNullOrWhiteSpace(request.Name))
    {
        return Results.BadRequest(new { message = "Product name is required." });
    }

    if (request.Price < 0)
    {
        return Results.BadRequest(new { message = "Product price must be greater than or equal to 0." });
    }

    var product = store.Create(request.Name, request.Price);

    return Results.Created($"/products/{product.Id}", product);
});

app.Run();

public record Product(int Id, string Name, decimal Price);

public record CreateProductRequest(string Name, decimal Price);

public class ProductStore
{
    private readonly object _lock = new();
    private readonly List<Product> _products =
    [
        new Product(1, "Laptop", 1500),
        new Product(2, "Mouse", 20),
        new Product(3, "Keyboard", 50)
    ];

    private int _nextId = 4;

    public IReadOnlyList<Product> GetAll()
    {
        lock (_lock)
        {
            return _products.ToList();
        }
    }

    public Product? GetById(int id)
    {
        lock (_lock)
        {
            return _products.FirstOrDefault(product => product.Id == id);
        }
    }

    public Product Create(string name, decimal price)
    {
        lock (_lock)
        {
            var product = new Product(_nextId++, name.Trim(), price);
            _products.Add(product);
            return product;
        }
    }
}
