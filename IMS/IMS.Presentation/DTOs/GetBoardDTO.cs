namespace IMS.Presentation.DTOs;

public class GetBoardDTO
{  
    public required string Title { get; set; }

    public required string Description { get; set; }

    public required Guid CreatedById { get; set; }

    public required Guid CreatedToId { get; set; }
}
