using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.ComponentModel.DataAnnotations;
using IMS.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace IMS.DAL.Entities
{
    public class User
    {
        [Key]
        public int ID { get; private set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        public string Lastname { get; set; } = string.Empty;

        public string? Patronymic { get; set; }

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public Role Role { get; set; } = Role.Unassigned;

        [Required]
        public DateOnly CreatedAt { get; private set; } = DateOnly.FromDateTime(DateTime.Now);

        [Required]
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
