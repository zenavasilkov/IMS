using Shared.Enums;

namespace IMS.Presentation.DTOs.UpdateDTO;

public record UpdateTicketDto( 
    string Title,
    string Description,
    TicketStatus Status,
    [property: JsonRequired] DateTime DeadLine
);
