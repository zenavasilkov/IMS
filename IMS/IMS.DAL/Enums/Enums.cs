using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAL.Enums
{
    public enum Role
    {
        Unassigned,
        Admin,
        HRManager,
        Mentor,
        Intern
    }

    public enum Status
    { 
        Unassigned,
        ToDo,   
        InProgress,
        PullRequest,
        Done
    }
}
