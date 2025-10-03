using IMS.DAL.Enums;

namespace IMS.DAL.Entities
{
    public class User : EntityBase
    {   
        public string Email { get; set; } = string.Empty;
         
        public string PhoneNumber { get; set; } = string.Empty;
         
        public string Firstname { get; set; } = string.Empty;
         
        public string Lastname { get; set; } = string.Empty;

        public string? Patronymic { get; set; } 
         
        public Role Role { get; set; } = Role.Unassigned; 

        public User() { }

        public User( string email, string phoneNumber, string firstname, string lastname, string? patronymic, Role role)
        {
            Email = email;
            PhoneNumber = phoneNumber;
            Firstname = firstname;
            Lastname = lastname;
            Patronymic = patronymic; 
            Role = role;
        }
    }
}
