using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebAPI_Demo.Models;
using WebAPI_Demo.Rediscache.IServices;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Username or password is incorrect";
                return BadRequest(_responseDTO);
            }
            _responseDTO.Result = loginResponse;
            return Ok(_responseDTO);
        }


        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(string email, string role)
        {
            var assignRoleSuccessful = await _authService.AssignRole(email, role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _responseDTO.IsSuccess = false;
                _responseDTO.Message = "Error assign";
                return BadRequest(_responseDTO);
            }

            return Ok(_responseDTO);
        }
    }
}
