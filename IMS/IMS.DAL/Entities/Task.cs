namespace IMS.DAL.Entities
{
    public class Task : EntityBase
    {   
        public Guid BoardId { get; set; }

        public Board Board { get; set; } = null!;
         
        public required string Title { get; set; }
         
        public required string Description { get; set; }
         
        public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Unassigned;
         
        public DateTime DeadLine { get; set; }
    }
}
