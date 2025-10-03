using IMS.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DAL.Entities
{
    public class Task
    {
        [Key]
        public int ID { get; private set; }

        [Required]
        public Board Board { get; set; } = null!;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required] 
        public string Description { get; set; } = string.Empty;

        [Required]
        public Status Status { get; set; } = Status.Unassigned;

        [Required] 
        public DateOnly CreatedAd { get; private set; } = DateOnly.FromDateTime(DateTime.Now);

        [Required] 
        public DateOnly DeadLine { get; set; }

        protected Task() { }

        public Task(Board board, string title, string description, Status status, DateOnly deadLine)
        {
            Board = board;
            Title = title;
            Description = description;
            Status = status;
            DeadLine = deadLine;
        }

    }
}
