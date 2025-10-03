namespace IMS.DAL.Entities
{
    public class FeedBack : EntityBase
    {   
        public Task Task { get; set; } = null!;

        public User Mentor { get; set; } = null!;

        public User Intern { get; set; } = null!;

        public string Comment { get; set; } = string.Empty;
    }
}
