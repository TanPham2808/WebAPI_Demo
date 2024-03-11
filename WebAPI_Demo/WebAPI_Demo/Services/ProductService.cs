using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using WebAPI_Demo.Data;
using WebAPI_Demo.Models;
using WebAPI_Demo.Models.DTO;
using WebAPI_Demo.Services.IServices;

namespace WebAPI_Demo.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;
        private readonly ResponseDTO _res;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _res = new ResponseDTO();
            _mapper = mapper;
        }
        public async Task<ResponseDTO> CreateProductAsync(ProductDTO productDTO)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDTO);
                await _db.Products.AddAsync(product);
                await _db.SaveChangesAsync();

                _res.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _res.IsSuccess = false;
                _res.Message = ex.Message;

                // Cách Custom Exception giống FAP
                //ResponseCustomDTO.BadRequest(ex.ToString());

                // Cách Custom Exception giống Youtube

            }

            return _res;
        }

        public async Task<ResponseDTO> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _db.Products.FirstOrDefaultAsync(x => x.ProductId == id);
                if (product == null)
                {
                    _res.Message = "Don't have Product";
                    return _res;
                }

                _db.Products.Remove(product);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _res.IsSuccess = false;
                _res.Message = ex.Message;
            }

            return _res;
        }

        public async Task<ResponseDTO> GetAllProductAsync()
        {
            try
            {
                var lstProducts = await _db.Products.ToListAsync();
                _res.Result = _mapper.Map<List<ProductDTO>>(lstProducts);
            }
            catch(Exception ex)
            {
                _res.IsSuccess = false;
                _res.Message = ex.Message;
            }

            return _res;
        }

        public async Task<ResponseDTO> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _db.Products.FirstAsync(x=>x.ProductId == id);
                _res.Result = product;
            }
            catch (Exception ex)
            {
                _res.IsSuccess = false;
                _res.Message = ex.Message;
            }

            return _res;
        }

        public async Task<ResponseDTO> UpdateProductAsync(int id, ProductDTO productDTO)
        {
            try
            {
                var product = await _db.Products.FirstOrDefaultAsync(x => x.ProductId == id);
                if (product == null)
                {
                    _res.Message = "Don't have product";
                    _res.Result = "";
                    return _res;
                }

                product.Name = productDTO.Name;
                product.Price = productDTO.Price;
                product.Description = productDTO.Description;

                _db.Products.Update(product);
                _db.SaveChanges();

                _res.Result = product;
            }
            catch (Exception ex)
            {
                _res.IsSuccess = false;
                _res.Message = ex.Message;
            }
            return _res;
        }
    }
}
