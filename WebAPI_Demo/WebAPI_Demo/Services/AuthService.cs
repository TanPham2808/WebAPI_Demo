using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI_Demo.Data;
using WebAPI_Demo.Models;
using WebAPI_Demo.Services.IServices;

namespace WebAPI_Demo.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly JWTOption _jWTOption;

        public AuthService(AppDbContext db, 
            UserManager<IdentityUser> userManager, 
            IConfiguration config,
            IOptions<JWTOption> jWTOption,
            RoleManager<IdentityRole> roleManager
            )
        {
            _db = db;
            _userManager = userManager;
            _config = config;
            _jWTOption = jWTOption.Value;
            _roleManager = roleManager;
        }

        public async Task<LoginResponseDTO> Login(LoginRequest loginRequestDTO)
        {
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDTO.UserName);
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDTO()
                {
                    User = null,
                    Token = ""
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            // Generate JWT Token
            var token = GenerateToken(user, roles);

            UserDTO userDTO = new()
            {
                Email = user.Email,
                ID = user.Id,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                User = userDTO,
                Token = token,
            };

            return loginResponseDTO;
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

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    // Tạo role nếu không tồn tại
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public string GenerateToken(IdentityUser identityUser, IEnumerable<string> roles)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var keySecrect = Encoding.ASCII.GetBytes(_jWTOption.Secret);

                // Các giá trị cơ bản của user add vào token mã hóa
                var claimList = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email,identityUser.Email),
                    new Claim(JwtRegisteredClaimNames.Sub,identityUser.Id),
                    new Claim(JwtRegisteredClaimNames.Name,identityUser.UserName)
                };

                // Add role của user vào trong token
                claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var tokenDescription = new SecurityTokenDescriptor
                {
                    Audience = _jWTOption.Audience,
                    Issuer = _jWTOption.Issuer,
                    Subject = new ClaimsIdentity(claimList),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keySecrect), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescription);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
