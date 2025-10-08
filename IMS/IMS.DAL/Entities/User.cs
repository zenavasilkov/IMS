using Shared.Enums;

namespace IMS.DAL.Entities;

public class User : EntityBase
{   
    public required string Email { get; set; }
     
    public required string PhoneNumber { get; set; }
     
    public required string Firstname { get; set; }

    public required string Lastname { get; set; }

    public string? Patronymic { get; set; }

    public Role Role { get; set; } = Role.Unassigned;

    public List<Internship> Internships { get; set; } = [];
}
