namespace IMS.Presentation.DTOs.CreateDTO;

public record CreateBoardDto(
    Guid CreatedById,
    Guid CreatedToId,
    string Title,
    string Description
); 
