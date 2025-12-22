using Shared.Enums;

namespace IMS.BLL.Models;

public class UserModel : ModelBase
{ 
    public required string Email { get; set; }

    public required string PhoneNumber { get; set; }

    public required string Firstname { get; set; }

    public required string Lastname { get; set; }

    public string? Patronymic { get; set; }

    public Role Role { get; set; }

    public List<InternshipModel> Internships { get; set; } = [];
}
