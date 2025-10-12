using Shared.Enums;

namespace IMS.Presentation.DTOs.UpdateDTO;

public record UpdateUserDTO(
    string Email,
    string PhoneNumber,
    string Firstname,
    string Lastname,
    string? Patronymic,
    Role Role
);
