using Shared.Enums;

namespace IMS.Presentation.DTOs.GetDTO;

public class GetInternshipDTO
{
    public Guid Id { get; set; }
    public Guid InternId { get; set; }  
    public Guid MentorId { get; set; }  
    public Guid HumanResourcesManagerId { get; set; }  
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; } 
    public InternshipStatus Status { get; set; } = InternshipStatus.NotStarted;
}
