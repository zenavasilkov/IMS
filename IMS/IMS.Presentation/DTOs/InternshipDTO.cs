using Shared.Enums;

namespace IMS.Presentation.DTOs;

public class InternshipDTO
{
    public required Guid InternId { get; set; } 

    public required Guid MentorId { get; set; } 

    public required Guid HumanResourcesManagerId { get; set; } 

    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    public DateTime? EndDate { get; set; }

    public InternshipStatus Status { get; set; } = InternshipStatus.NotStarted;
}
