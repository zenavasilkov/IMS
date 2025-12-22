using Shared.Enums;

namespace IMS.Presentation.DTOs.GetDTO;

public class InternshipDto
{
    public Guid Id { get; init; }
    public Guid InternId { get; init; }  
    public Guid MentorId { get; init; }  
    public Guid HumanResourcesManagerId { get; init; }  
    public DateTime StartDate { get; init; }
    public DateTime? EndDate { get; init; } 
    public InternshipStatus Status { get; set; } = InternshipStatus.NotStarted;
}
