namespace IMS.Presentation.DTOs.GetDTO;

public class FeedbackDTO
{
    public Guid Id { get; set; }
    public Guid TicketId { get; set; }
    public Guid SentById { get; set; }
    public Guid AddressedToId { get; set; }
    public required string Comment { get; set; }
}
