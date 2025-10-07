using Shared.Enums;

namespace IMS.BLL.Models;

public class TicketModel : ModelBase
{
    public required Guid BoardId { get; init; }

    public required BoardModel Board { get; init; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public TicketStatus Status { get; set; } = TicketStatus.Unassigned;

    public DateTime DeadLine { get; set; }

    public required List<FeedbackModel> Feedbacks { get; set; } = [];
}
