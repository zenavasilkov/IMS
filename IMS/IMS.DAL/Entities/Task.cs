namespace IMS.DAL.Entities
{
    public class Task : EntityBase
    {   
        public Guid BoardId { get; set; }

        public Board Board { get; set; } = null!;
         
        public string Title { get; set; } = null!;
         
        public string Description { get; set; } = null!;
         
        public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Unassigned;
         
        public DateOnly DeadLine { get; set; }
    }
}
