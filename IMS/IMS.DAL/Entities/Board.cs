using System.Collections;

namespace IMS.DAL.Entities
{
    public class Board : IEnumerable<Task>
    { 
        public Guid ID { get; private set; }
         
        public string Title { get; set; } = string.Empty;
          
        public string Description { get; set; } = string.Empty;
         
        public User CreatedBy { get; set; } = null!;
         
        public User CreatedTo { get; set; } = null!;
          
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
