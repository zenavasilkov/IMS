using Shared.Enums;

namespace IMS.BLL.Models;

public class InternshipModel : ModelBase
{
    public required Guid InternId { get; init; }

    public required UserModel Intern { get; set; }

    public required Guid MentorId { get; set; }

    public required UserModel Mentor { get; set; }

    public required Guid HumanResourcesManagerId { get; set; }

    public required UserModel HumanResourcesManager { get; set; }

    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    public DateTime? EndDate { get; set; }

    public InternshipStatus Status { get; set; } = InternshipStatus.NotStarted;
}
