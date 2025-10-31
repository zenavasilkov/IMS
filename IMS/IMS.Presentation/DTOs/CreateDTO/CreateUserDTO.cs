using Shared.Enums;

namespace IMS.Presentation.DTOs.CreateDTO;

public record CreateUserDto(
    string Email, 
    string PhoneNumber,  
    string Firstname,
    string Lastname,
    string? Patronymic,
    Role Role  
);
