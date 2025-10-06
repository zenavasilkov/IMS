namespace IMS.DAL.Entities
{
    public class Ticket : EntityBase
    {   
        public required Guid BoardId { get; init; }

        public required Board Board { get; set; }
         
        public required string Title { get; set; }
         
        public required string Description { get; set; }
         
        public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Unassigned;
         
        public DateTime DeadLine { get; set; }
    }
}
