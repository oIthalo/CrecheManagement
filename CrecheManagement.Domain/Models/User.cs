namespace CrecheManagement.Domain.Models;

public class User : BaseModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool KeepAlive { get; set; }
    public string RefreshToken { get; set; }
    public DateTime LoginDate { get; set; }
}