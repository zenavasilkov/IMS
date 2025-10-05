namespace IMS.DAL.Entities
{
    public class Board : EntityBase
    {   
        public required string Title { get; set; }
          
        public required string Description { get; set; }
         
        public Guid CreatedById { get; private set; }

        public User CreatedBy { get; private set; } = null!;
         
        public Guid CreatedToId { get; private set; }

        public User CreatedTo { get; set; } = null!;
          
        public List<Task> Tasks { get; set; } = [];
    }
}
