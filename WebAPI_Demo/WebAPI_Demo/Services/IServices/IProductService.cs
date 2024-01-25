using WebAPI_Demo.Models;
using WebAPI_Demo.Models.DTO;

namespace WebAPI_Demo.Services.IServices
{
    public interface IProductService
    {
        Task<ResponseDTO> GetAllProductAsync();
        Task<ResponseDTO> GetProductByIdAsync(int id);
        Task<ResponseDTO> CreateProductAsync(ProductDTO productDTO);
        Task<ResponseDTO> UpdateProductAsync(int id, ProductDTO productDTO);
        Task<ResponseDTO> DeleteProductAsync(int id);
    }
}
