namespace InnoGotchiWebAPI.Models;

public class ClientUserModel
{
    public string Login { get; set; } = null!;
    public string? Nickname { get; set; } = null;
    public string Token { get; set; } = null!;
}
