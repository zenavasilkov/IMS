using IMS.Presentation.DTOs.UpdateDTO;
using Shared.Enums;

namespace IMS.Presentation.DTOs.CreateDTO;

public record CreateTicketDTO(
    Guid BoardId,
    string Title,
    string Description,
    TicketStatus Status,
    DateTime DeadLine 
) : UpdateTicketDTO(Title, Description, Status, DeadLine);
