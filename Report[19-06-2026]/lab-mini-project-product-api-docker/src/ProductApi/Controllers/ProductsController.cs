using Microsoft.AspNetCore.Mvc;
using ProductApi.Dtos;
using ProductApi.Services;

namespace ProductApi.Controllers;

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
    public async Task<ActionResult<List<ProductDto>>> GetAll(
        [FromQuery] string? keyword,
        [FromQuery] int? categoryId,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var products = await _productService.GetAllAsync(keyword, categoryId, minPrice, maxPrice, page, pageSize);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(ProductCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Product name is required.");
        }

        if (dto.Price < 0 || dto.StockQuantity < 0)
        {
            return BadRequest("Price and stock quantity must be greater than or equal to 0.");
        }

        var product = await _productService.CreateAsync(dto);

        if (product is null)
        {
            return BadRequest("Category does not exist.");
        }

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProductUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Product name is required.");
        }

        if (dto.Price < 0 || dto.StockQuantity < 0)
        {
            return BadRequest("Price and stock quantity must be greater than or equal to 0.");
        }

        var updated = await _productService.UpdateAsync(id, dto);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}

