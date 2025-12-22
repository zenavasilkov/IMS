using Shared.Enums;

namespace IMS.Presentation.DTOs.GetDTO;

public class UserDto
{
    public Guid Id { get; init; }
    public required string Email { get; init; } 
    public required string PhoneNumber { get; init; } 
    public required string FirstName { get; init; } 
    public required string LastName { get; init; } 
    public string? Patronymic { get; init; } 
    public Role Role { get; init; } = Role.Unassigned;
}
