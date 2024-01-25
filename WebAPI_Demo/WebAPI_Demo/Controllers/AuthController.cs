using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebAPI_Demo.Models;
using WebAPI_Demo.Services.IServices;

namespace WebAPI_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IAuthService _authService;
        protected ResponseDTO _responseDTO;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            _responseDTO = new ResponseDTO();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequest model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = errorMessage;
                return BadRequest(errorMessage);
            }

            return Ok(_responseDTO);
        }
    }
}
