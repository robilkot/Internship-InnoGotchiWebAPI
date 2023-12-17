namespace InnoGotchiWebAPI.Models;

public partial class DbUsersPetModel
{
    public Guid Id { get; set; }
    public string UserLogin { get; set; } = null!;

    public Guid PetId { get; set; }

    public virtual DbPetModel Pet { get; set; } = null!;

    public virtual DbUserModel UserLoginNavigation { get; set; } = null!;
}
