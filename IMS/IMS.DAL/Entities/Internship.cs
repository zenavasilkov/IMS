using IMS.DAL.Enums;

namespace IMS.DAL.Entities
{
    public class Internship : EntityBase
    {   
        public Guid InternId { get; set; }

        public User Intern { get; set; } = null!;

        public Guid MentorId { get; set; }

        public User Mentor { get; set; } = null!;

        public Guid HRMId { get; set; }

        public User HRM { get; set; } = null!; 
        // HRM assigns mentors to interns and creates internship records, as well as adds interns and mentors into system
         
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public DateOnly? EndDate { get; set; }
         
        public InternshipStatus Status { get; set; } = InternshipStatus.NotStarted;
    }
}
