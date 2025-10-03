using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using IMS.DAL.Entities;
using System.Collections;

namespace IMS.DAL.Entities
{
    public class Board : IEnumerable<Task>
    {
        [Key]
        public int ID { get; private set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required] 
        public string Description { get; set; } = string.Empty;

        [Required]
        public User CreatedBy { get; set; } = null!;

        [Required]
        public User CreatedTo { get; set; } = null!;

        [Required] 
        public List<Task> Tasks { get; set; } = [];

        protected Board() { }

        public Board(string title, string description, User createdBy, User createdTo)
        {
            Title = title;
            Description = description;
            CreatedBy = createdBy;
            CreatedTo = createdTo;
        }

        public IEnumerator<Task> GetEnumerator()
        {
            return Tasks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
