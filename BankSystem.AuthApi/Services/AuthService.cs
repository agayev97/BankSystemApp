using BankSystem.AuthApi.DTOs;
using BankSystem.AuthApi.Models;
using BankSystem.AuthApi.Repositories.Interfaces;
using BankSystem.AuthApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankSystem.AuthApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            // 1. Bazadan istifadəçini tapırıq
            var user = await _userRepository.GetByUserNameAsync(dto.UserName);
            if (user == null)
                throw new Exception("İstifadəçi adı və ya şifrə yanlışdır.");
            // 2. Aktiv olub-olmamasını yoxlayırıq
            if (!user.IsActive)
                throw new Exception("İstifadəçi hesabı deaktiv edilib.");
            // 3. BCrypt ilə şifrəni yoxlayırıq
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isPasswordValid)
                throw new Exception("İstifadəçi adı və ya şifrə yanlışdır.");
            // 4. JWT Token hazırlayırıq
            var token = GenerateJwtToken(user, out DateTime expires);
            return new LoginResponseDto
            {
                Token = token,
                UserId = user.Id,
                UserName = user.UserName,
                Role = user.Role,
                CustomerId = user.CustomerId,
                Expiration = expires
            };
        }
        
        public async Task<int> RegisterAsync(RegisterDto dto)
        {
            // Əgər 0 və ya mənfi gələrsə, avtomatik null edirik
            int? customerId = (dto.CustomerId == null || dto.CustomerId <= 0) ? null : dto.CustomerId;
            // Şifrəni təhlükəsiz hash-ləyirik
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            // Bazaya yazırıq (usp_CreateUser avtomatik unikal username və s. yoxlayır)
            return await _userRepository.CreateUserAsync(dto.CustomerId, dto.UserName, passwordHash, dto.Role);
        }

        public async Task DeactivateAsync(int userId)
        {
            await _userRepository.DeactivateUserAsync(userId);
        }


        private string GenerateJwtToken(User user, out DateTime expires)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "AtlasBank_SecretKey_1234567890123456_VerySecureKey!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        };
            if (user.CustomerId.HasValue)
            {
                claims.Add(new Claim("CustomerId", user.CustomerId.Value.ToString()));
            }
            expires = DateTime.UtcNow.AddHours(4);
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"] ?? "BankSystemAuthApi",
                audience: jwtSettings["Audience"] ?? "BankSystemClients",
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}

