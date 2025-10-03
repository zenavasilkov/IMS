namespace IMS.DAL.Entities
{
    public class Board : EntityBase
    {   
        public string Title { get; set; } = null!;
          
        public string Description { get; set; } = null!;
         
        public Guid CreatedById { get; private set; }

        public User CreatedBy { get; private set; } = null!;
         
        public Guid CreatedToId { get; private set; }

        public User CreatedTo { get; set; } = null!;
          
        public List<Task> Tasks { get; set; } = [];
    }
}
