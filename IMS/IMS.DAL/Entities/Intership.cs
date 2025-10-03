using IMS.DAL.Enums;

namespace IMS.DAL.Entities
{
    public class Intership
    { 
        public Guid ID { get; private set; }
         
        public User Intern { get; set; } = null!;
         
        public User Mentor { get; set; } = null!;
         
        public User HRM { get; set; } = null!;
        // Human Resource Manager assigns mentors to interns and creates internship records, as well as adds interns and mentors into system
         
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public DateOnly? EndDate { get; set; }
         
        public InternshipStatus Status { get; set; } = InternshipStatus.NotStarted;

        protected Intership() { }

        public Intership(User intern, User mentor, User hrm, DateOnly startDate, DateOnly? endDate, InternshipStatus status)
        {
            Intern = intern;
            Mentor = mentor;
            HRM = hrm;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
        }
    }
}
