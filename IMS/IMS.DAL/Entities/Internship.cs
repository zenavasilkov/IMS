using Shared.Enums;

namespace IMS.DAL.Entities;

public class Internship : EntityBase
{   
    public required Guid InternId { get; init; }

    public required User Intern { get; init; }

    public required Guid MentorId { get; set; }

    public required User Mentor { get; set; }

    public required Guid HumanResourcesManagerId { get; set; }

    public required User HumanResourcesManager { get; set; }
     
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    public DateTime? EndDate { get; set; }
     
    public InternshipStatus Status { get; set; } = InternshipStatus.NotStarted;
}
