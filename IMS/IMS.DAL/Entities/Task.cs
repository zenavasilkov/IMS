namespace IMS.DAL.Entities
{
    public class Task : EntityBase
    {   
        public Board Board { get; set; } = null!;
         
        public string Title { get; set; } = string.Empty;
         
        public string Description { get; set; } = string.Empty;
         
        public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Unassigned;
         
        public DateOnly DeadLine { get; set; }

        protected Task() { }

        public Task(Board board, string title, string description, Enums.TaskStatus status, DateOnly deadLine)
        {
            Board = board;
            Title = title;
            Description = description;
            Status = status;
            DeadLine = deadLine;
        } 
    }
}
