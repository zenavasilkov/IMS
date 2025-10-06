namespace IMS.DAL.Entities
{
    public class Feedback : EntityBase
    {   
        public required Guid TaskId { get; init; }

        public required Task Task { get; init; }

        public required Guid LeftById { get; init; }

        public required User LeftBy { get; init; }

        public required Guid AddressedToId { get; init; }

        public required User AddressedTo { get; init; }

        public required string Comment { get; set; }
    }
}
