using Shared.Enums;

namespace IMS.Presentation.DTOs.CreateDTO;

public record CreateUserDto(
    string Email, 
    string PhoneNumber,
    string FirstName,
    string LastName,
    string? Patronymic,
    Role Role  
);
