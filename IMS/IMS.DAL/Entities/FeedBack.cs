namespace IMS.DAL.Entities
{
    public class Feedback : EntityBase
    {   
        public Guid TaskId { get; private set; }

        public Task Task { get; private set; } = null!;

        public Guid MentorId { get; private set; }

        public User Mentor { get; set; } = null!;

        public Guid InternId { get; private set; }

        public User Intern { get; set; } = null!;

        public required string Comment { get; set; }
    }
}
