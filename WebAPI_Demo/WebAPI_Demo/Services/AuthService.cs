using Microsoft.AspNetCore.Identity;
using WebAPI_Demo.Data;
using WebAPI_Demo.Models;
using WebAPI_Demo.Services.IServices;

namespace WebAPI_Demo.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public AuthService(AppDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public Task<LoginResponseDTO> Login(LoginRequest loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Register(RegisterationRequest registerUser)
        {
            IdentityUser user = new()
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                NormalizedEmail = registerUser.Email,
                PhoneNumber = registerUser.PhoneNumber,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerUser.Password);

                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registerUser.Email);

                    UserDTO userDTO = new()
                    {
                        ID = userToReturn.Id,
                        Email = userToReturn.Email,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }

            }
            catch (Exception ex)
            {

            }

            return "Error register";
        }
    }
}
