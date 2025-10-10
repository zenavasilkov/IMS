namespace IMS.Presentation.DTOs;

public class GetFeedbackDTO
{
    public required Guid TicketId { get; set; }

    public required Guid SentById { get; set; }

    public required Guid AddressedToId { get; set; }

    public required string Comment { get; set; }
}
