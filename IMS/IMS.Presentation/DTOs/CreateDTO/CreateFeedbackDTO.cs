namespace IMS.Presentation.DTOs.CreateDTO;

public record CreateFeedbackDto(
     Guid TicketId,
     Guid SentById,
     Guid AddressedToId,
     string Comment
);
