using Shared.Enums;

namespace IMS.Presentation.DTOs.UpdateDTO;

public record UpdateInternshipDTO( 
    Guid MentorId,
    Guid HumanResourcesManagerId,
    DateTime StartDate,
    DateTime EndDate,
    InternshipStatus Status
);
