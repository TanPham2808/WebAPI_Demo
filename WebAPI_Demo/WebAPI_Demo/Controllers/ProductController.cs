using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using WebAPI_Demo.Data;
using WebAPI_Demo.Models;
using WebAPI_Demo.Models.DTO;
using WebAPI_Demo.Services.IServices;


namespace WebAPI_Demo.Controllers
{
    [Route("api/product")]
    [ApiController]
    [EnableCors("AllowAll")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var respone = await _productService.GetAllProductAsync();

                if (respone != null && respone.IsSuccess)
                {
                    return Ok(respone);
                }

                return BadRequest();
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var respone = await _productService.GetProductByIdAsync(id);

                if (respone != null && respone.IsSuccess)
                {
                    var product = respone.Result;
                    return Ok(respone.Result);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<ProductController>
        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] ProductDTO productDTO)
        {
            try
            {
                var response = await _productService.CreateProductAsync(productDTO);
                if (response != null && response.IsSuccess)
                {
                    var product = response.Result;
                    return Ok(response.Result);
                }

                return BadRequest();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ProductController>/5
        [HttpPut("Edit")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductDTO productDTO)
        {
            try
            {
                var response = await _productService.UpdateProductAsync(id, productDTO);
                if (response != null && response.IsSuccess)
                {
                    var product = response.Result;
                    return Ok(response.Result);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("remove")]
        public async Task<IActionResult> Delete(int productId)
        {
            try
            {
                var response = await _productService.DeleteProductAsync(productId);
                if (response != null && response.IsSuccess)
                {
                    var product = response.Result;
                    return Ok(response.Result);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
