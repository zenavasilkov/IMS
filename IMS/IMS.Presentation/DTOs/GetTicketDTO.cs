using Shared.Enums;

namespace IMS.Presentation.DTOs;

public class GetTicketDTO
{
    public required Guid BoardId { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public TicketStatus Status { get; set; } = TicketStatus.Unassigned;

    public required DateTime DeadLine { get; set; }
}
