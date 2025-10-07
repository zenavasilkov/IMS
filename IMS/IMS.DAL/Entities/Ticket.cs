using Shared.Enums;

namespace IMS.DAL.Entities;

public class Ticket : EntityBase
{   
    public required Guid BoardId { get; init; }

    public required Board Board { get; init; }
     
    public required string Title { get; set; }
     
    public required string Description { get; set; }
     
    public TicketStatus Status { get; set; } = TicketStatus.Unassigned;
     
    public required DateTime DeadLine { get; set; }
}
