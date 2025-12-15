using Shared.Enums;

namespace IMS.Presentation.DTOs.GetDTO;

public class TicketDto
{
    public Guid Id { get; init; }
    public Guid BoardId { get; init; } 
    public required string Title { get; init; } 
    public required string Description { get; init; } 
    public TicketStatus Status { get; set; } = TicketStatus.Unassigned; 
    public DateTime DeadLine { get; init; }
}
