namespace eCommerce.DTOs.Login;

public class LoginResponseDto
{
    public string Token { get; set; } = "";
    public LoginUserDto User { get; set; } = new();
}

public class LoginUserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";

    public string Role { get; set; } = "";

    public string Token { get; set; } = "";

}
