using Shared.Enums;
using System.Text.Json.Serialization;

namespace IMS.Presentation.DTOs.UpdateDTO;

public record UpdateInternshipDto(
    [property: JsonRequired] Guid MentorId,
    [property: JsonRequired] Guid HumanResourcesManagerId,
    [property: JsonRequired] DateTime StartDate,
    DateTime? EndDate,
    [property: JsonRequired] InternshipStatus Status
);
