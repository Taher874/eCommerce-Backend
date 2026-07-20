namespace eCommerce.DTOs.Register;

public class RegisterResponseDto
{
    public string Token { get; set; } = string.Empty;
    public RegisterUserDto User { get; set; } = new();
}

public class RegisterUserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}