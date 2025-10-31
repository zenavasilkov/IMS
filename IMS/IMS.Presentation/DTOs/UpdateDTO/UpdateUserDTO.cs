using Shared.Enums;
using System.Text.Json.Serialization;

namespace IMS.Presentation.DTOs.UpdateDTO;

public record UpdateUserDto(
    string Email,
    string PhoneNumber,
    string Firstname,
    string Lastname,
    string? Patronymic,
    [property: JsonRequired] Role Role
);
