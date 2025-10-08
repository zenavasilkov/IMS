namespace IMS.BLL.Models;

public abstract class UserModel : ModelBase
{ 
    public required string Email { get; set; }

    public required string PhoneNumber { get; set; }

    public required string Firstname { get; set; }

    public required string Lastname { get; set; }

    public string? Patronymic { get; set; }
}
