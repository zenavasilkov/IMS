using Shared.Enums;

namespace IMS.Presentation.DTOs.GetDTO;

public class TicketDTO
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; } 
    public required string Title { get; set; } 
    public required string Description { get; set; } 
    public TicketStatus Status { get; set; } = TicketStatus.Unassigned; 
    public DateTime DeadLine { get; set; }
}
