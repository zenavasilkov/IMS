namespace IMS.DAL.Entities
{
    public class FeedBack
    { 
        public Guid ID { get; private set; }
        
        public Task Task { get; set; } = null!;

        public User Mentor { get; set; } = null!;

        public User Intern { get; set; } = null!;

        public string Comment { get; set; } = string.Empty;

        public DateOnly CreatedAt { get; private set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
