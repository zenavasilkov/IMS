using Shared.Enums;

namespace IMS.Presentation.DTOs.UpdateDTO;

public record UpdateTicketDto( 
    string Title,
    string Description,
    TicketStatus Status,
    DateTime DeadLine
);
