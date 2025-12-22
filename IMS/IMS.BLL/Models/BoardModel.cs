namespace IMS.BLL.Models;

public class BoardModel : ModelBase
{
    public required string Title { get; set; }

    public required string Description { get; set; }

    public required Guid CreatedById { get; init; }

    public required UserModel CreatedBy { get; init; }

    public required Guid CreatedToId { get; init; }

    public required UserModel CreatedTo { get; init; }

    public List<TicketModel> Tickets { get; set; } = [];
}
