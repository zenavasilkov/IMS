namespace IMS.Presentation.DTOs.CreateDTO;

public record CreateFeedbackDTO(
     Guid TicketId,
     Guid SentById,
     Guid AddressedToId,
     string Comment
);