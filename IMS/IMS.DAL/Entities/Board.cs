namespace IMS.DAL.Entities
{
    public class Board : EntityBase
    {   
        public required string Title { get; set; }
          
        public required string Description { get; set; }
         
        public required Guid CreatedById { get; init; }

        public required User CreatedBy { get; init; }
         
        public required Guid CreatedToId { get; init; }

        public required User CreatedTo { get; set; }
          
        public List<Ticket> Tasks { get; set; } = [];
    }
}
