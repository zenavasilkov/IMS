namespace IMS.DAL.Entities
{
    public class FeedBack : EntityBase
    {   
        public Task Task { get; set; } = null!;

        public User Mentor { get; set; } = null!;

        public User Intern { get; set; } = null!;

        public string Comment { get; set; } = string.Empty; 

        public FeedBack() { }

        public FeedBack(Task task, User mentor, User intern, string comment)
        {
            Task = task;
            Mentor = mentor;
            Intern = intern;
            Comment = comment;
        }
    }
}
