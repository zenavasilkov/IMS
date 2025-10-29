namespace IMS.Presentation.DTOs.CreateDTO;

public record CreateBoardDTO(
    Guid CreatedById,
    Guid CreatedToId,
    string Title,
    string Description
); 
