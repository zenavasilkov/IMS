using Shared.Enums;

namespace IMS.Presentation.DTOs.GetDTO;

public class UserDto
{
    public Guid Id { get; set; }
    public required string Email { get; set; } 
    public required string PhoneNumber { get; set; } 
    public required string Firstname { get; set; } 
    public required string Lastname { get; set; } 
    public string? Patronymic { get; set; } 
    public Role Role { get; set; } = Role.Unassigned;
}
