using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration; // Şifre anahtarını okumak için

        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<List<UserDto>>> GetAllUsersAsync()
        {
            var response = new ServiceResponse<List<UserDto>>();
            var users = await _context.Users.Where(u => !u.IsDeleted).ToListAsync();
            response.Data = users.Select(u => new UserDto { Id = u.Id, FullName = u.FullName, Email = u.Email, Role = u.Role }).ToList();
            return response;
        }

        public async Task<ServiceResponse<UserDto>> GetUserByIdAsync(int id)
        {
            var response = new ServiceResponse<UserDto>();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            if (user == null) { response.Success = false; response.Message = "Kullanıcı bulunamadı."; return response; }
            response.Data = new UserDto { Id = user.Id, FullName = user.FullName, Email = user.Email, Role = user.Role };
            return response;
        }

        public async Task<ServiceResponse<UserDto>> CreateUserAsync(CreateUserDto userDto)
        {
            var response = new ServiceResponse<UserDto>();
            // Not: Gerçek projelerde şifreler asla böyle saklanmaz, hashlenir! (Örn: BCrypt)
            var user = new User { FullName = userDto.FullName, Email = userDto.Email, Password = userDto.Password, Role = "User" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            response.Data = new UserDto { Id = user.Id, FullName = user.FullName, Email = user.Email, Role = user.Role };
            response.Message = "Kullanıcı oluşturuldu.";
            return response;
        }

        // ==========================================
        // 🔥 YENİ EKLENEN LOGIN METODU
        // ==========================================
        public async Task<ServiceResponse<string>> LoginAsync(UserLoginDto request)
        {
            var response = new ServiceResponse<string>();
            
            // 1. Kullanıcıyı bul
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email && !u.IsDeleted);

            // 2. Kullanıcı yoksa veya şifre yanlışsa hata ver
            if (user == null || user.Password != request.Password)
            {
                response.Success = false;
                response.Message = "E-posta veya şifre hatalı!";
                return response;
            }

            // 3. Token Oluştur (JWT)
            string token = CreateToken(user);

            response.Data = token;
            response.Message = "Giriş başarılı. Token oluşturuldu.";
            return response;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1), // Token 1 gün geçerli
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}