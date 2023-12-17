namespace InnoGotchiWebAPI.Models;

public partial class DbUserModel
{
    public string Login { get; set; } = null!;

    public byte[]? Password { get; set; }

    public string? Nickname { get; set; }

    public string? Role { get; set; } = AppConstants.DefaultRole;
}
