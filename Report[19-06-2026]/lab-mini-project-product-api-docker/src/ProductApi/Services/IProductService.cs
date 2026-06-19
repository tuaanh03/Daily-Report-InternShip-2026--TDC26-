using ProductApi.Dtos;

namespace ProductApi.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync(string? keyword, int? categoryId, decimal? minPrice, decimal? maxPrice, int page, int pageSize);

    Task<ProductDto?> GetByIdAsync(int id);

    Task<ProductDto?> CreateAsync(ProductCreateDto dto);

    Task<bool> UpdateAsync(int id, ProductUpdateDto dto);

    Task<bool> DeleteAsync(int id);
}

