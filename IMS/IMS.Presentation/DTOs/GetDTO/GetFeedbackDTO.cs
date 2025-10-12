namespace IMS.Presentation.DTOs.GetDTO;

public class GetFeedbackDTO
{
    public Guid Id { get; set; }
    public Guid SentById { get; set; }
    public Guid AddressedToId { get; set; }
    public required string Comment { get; set; }
}
