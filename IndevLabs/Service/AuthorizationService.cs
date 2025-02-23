namespace IndevLabs.Service;

public class AuthorizationService
{
    private readonly IConfiguration _configuration;
    private readonly JwtService _jwtService;

    public AuthorizationService(IConfiguration configuration, JwtService jwtService)
    {
        _configuration = configuration;
        _jwtService = jwtService;
    }

    public string Login(string username, string password)
    {
        var configUsername = Environment.GetEnvironmentVariable("Username") ??
                             throw new ApplicationException("Environment variable Username is not set!");
        var configPasswordHash = Environment.GetEnvironmentVariable("PasswordHash") ??
                                 throw new ApplicationException("Environment variable PasswordHash is not set!");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        if (username != configUsername || !BCrypt.Net.BCrypt.Verify(password, configPasswordHash))
        {
            throw new Exception("Invalid username or password.");
        }
        
        return _jwtService.GenerateToken(username, "Admin");
    }
}