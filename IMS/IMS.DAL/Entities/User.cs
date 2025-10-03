using IMS.DAL.Enums;

namespace IMS.DAL.Entities
{
    public class User
    { 
        public Guid ID { get; private set; }
         
        public string Email { get; set; } = string.Empty;
         
        public string PhoneNumber { get; set; } = string.Empty;
         
        public string Firstname { get; set; } = string.Empty;
         
        public string Lastname { get; set; } = string.Empty;

        public string? Patronymic { get; set; }
         
        public string PasswordHash { get; set; } = string.Empty;
         
        public Role Role { get; set; } = Role.Unassigned;
         
        public DateOnly CreatedAt { get; private set; } = DateOnly.FromDateTime(DateTime.Now);
         
        public DateOnly ModifiedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        public User() { }

        public User( string email, string phoneNumber, string firstname, string lastname, string? patronymic, string passwordHash, Role role)
        {
            Email = email;
            PhoneNumber = phoneNumber;
            Firstname = firstname;
            Lastname = lastname;
            Patronymic = patronymic;
            PasswordHash = passwordHash;
            Role = role;
        }
    }
}
