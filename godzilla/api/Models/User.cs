namespace api.Models;

public class User : BaseModel 
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
}

public class UserDTO : BaseModel {
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class UserAuth {
    public bool Auth { get; set; }
    public UserDTO? User { get; set; }
    public string Token { get; set; } = string.Empty;
}

public record RecordUser(string Name, string Email, string Password);

public record SignUser(string Email, string Password);
