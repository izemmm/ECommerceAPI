namespace ECommerceAPI.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class CreateUserDto
    {
        public string FullName { get; set; } = "Yeni Kullanıcı";
        public string Email { get; set; } = "user@example.com";
        public string Password { get; set; } = "12345";
    }
}
public class UserLoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}