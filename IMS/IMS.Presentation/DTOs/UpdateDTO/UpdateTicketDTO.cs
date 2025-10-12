using Shared.Enums;

namespace IMS.Presentation.DTOs.UpdateDTO;

public record UpdateTicketDTO( 
    string Title,
    string Description,
    TicketStatus Status,
    DateTime DeadLine
);
