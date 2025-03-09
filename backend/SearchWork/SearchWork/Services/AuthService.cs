using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using SearchWork.Data;
using SearchWork.Models.DTO;
using SearchWork.Models.Entity;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SearchWork.Services.Interfaces;

namespace SearchWork.Services
{
    public class AuthService : IAuth
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration config;

        public AuthService(ApplicationDbContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var role = context.Roles.FirstOrDefault(r => r.RoleId == user.RoleId)?.RoleName;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, role) 
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(jwtSettings["ExpireMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<string?> Login(string email, string password)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return GenerateJwtToken(user);
        }

        public async Task<string> Register(RegisterDto model)
        {
            if (context.Users.Any(u => u.Email == model.Email))
            {
                return "Пользователь с такой почтой уже существует";
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password); 

            User user = new User()
            {
                RoleId = model.RoleId,
                Username = model.Username,
                Email = model.Email,
                PasswordHash = hashedPassword,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedAt = DateTime.UtcNow,
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return GenerateJwtToken(user);
        }
    }
}
