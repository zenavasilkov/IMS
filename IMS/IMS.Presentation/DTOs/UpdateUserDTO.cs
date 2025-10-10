using Shared.Enums;

namespace IMS.Presentation.DTOs;

public class UpdateUserDTO
{
    public required Guid Id { get; set; }

    public required string Email { get; set; }

    public required string PhoneNumber { get; set; }

    public required string Firstname { get; set; }

    public required string Lastname { get; set; }

    public string? Patronymic { get; set; }

    public required Role Role { get; set; }
}
