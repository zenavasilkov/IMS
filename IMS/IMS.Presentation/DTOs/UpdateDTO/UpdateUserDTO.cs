using Shared.Enums;

namespace IMS.Presentation.DTOs.UpdateDTO;

public record UpdateUserDto(
    string Email,
    string PhoneNumber,
    string FirstName,
    string LastName,
    string? Patronymic,
    Role Role
);
