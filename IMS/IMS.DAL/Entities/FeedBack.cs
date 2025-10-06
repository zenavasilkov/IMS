namespace IMS.DAL.Entities;

public class Feedback : EntityBase
{   
    public required Guid TaskId { get; init; }

    public required Ticket Task { get; init; }

    public required Guid SendedById { get; init; }

    public required User SendedBy { get; init; }

    public required Guid AddressedToId { get; init; }

    public required User AddressedTo { get; init; }

    public required string Comment { get; set; }
}
