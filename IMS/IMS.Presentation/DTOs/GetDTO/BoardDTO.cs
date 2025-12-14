namespace IMS.Presentation.DTOs.GetDTO;

public class BoardDto
{
    public Guid Id { get; init; }
    public required string Title { get; init; } 
    public required string Description { get; init; } 
    public Guid CreatedById { get; init; }  
    public Guid CreatedToId { get; init; }
}
