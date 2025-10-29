using IMS.Presentation.DTOs.UpdateDTO;
using Shared.Enums;

namespace IMS.Presentation.DTOs.CreateDTO;

public record CreateUserDTO(
    string Email, 
    string PhoneNumber,  
    string Firstname,
    string Lastname,
    string? Patronymic,
    Role Role
) : UpdateUserDTO(Email, PhoneNumber, Firstname, Lastname, Patronymic, Role);
