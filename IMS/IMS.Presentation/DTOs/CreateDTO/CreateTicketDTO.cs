using Shared.Enums;

namespace IMS.Presentation.DTOs.CreateDTO;

public record CreateTicketDto(
    Guid BoardId,
    string Title,
    string Description,
    TicketStatus Status,
    DateTime DeadLine 
);
