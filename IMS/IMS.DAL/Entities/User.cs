using IMS.DAL.Enums;

namespace IMS.DAL.Entities
{
    public abstract class User : EntityBase
    {   
        public string Email { get; set; } = null!;
         
        public string PhoneNumber { get; set; } = null!;
         
        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string? Patronymic { get; set; }

        public Role Role { get; set; } = Role.Unassigned;
    }
}
