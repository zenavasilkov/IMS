namespace IMS.Presentation.DTOs;

public class UpdateFeedbackDTO
{
    public required Guid Id { get; set; }

    public required Guid TicketId { get; set; }

    public required Guid SentById { get; set; }

    public required Guid AddressedToId { get; set; }

    public required string Comment { get; set; }
}
