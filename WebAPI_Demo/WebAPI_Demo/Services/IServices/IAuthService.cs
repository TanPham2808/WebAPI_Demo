using WebAPI_Demo.Models;

namespace WebAPI_Demo.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegisterationRequest userDTO);
        Task<LoginResponseDTO> Login(LoginRequest loginRequestDTO);
    }
}
