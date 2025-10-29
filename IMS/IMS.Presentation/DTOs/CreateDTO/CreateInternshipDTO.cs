using Shared.Enums;

namespace IMS.Presentation.DTOs.CreateDTO;

public record CreateInternshipDTO(
    Guid InternId,
    Guid MentorId,
    Guid HumanResourcesManagerId,
    DateTime StartDate,
    DateTime? EndDate,
    InternshipStatus Status 
);
