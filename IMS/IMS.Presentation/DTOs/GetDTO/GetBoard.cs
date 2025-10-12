namespace IMS.Presentation.DTOs.GetDTO;

public class GetBoard
{
    public Guid Id { get; set; }
    public required string Title { get; set; } 
    public required string Description { get; set; } 
    public Guid CreatedById { get; set; }  
    public Guid CreatedToId { get; set; }
}
