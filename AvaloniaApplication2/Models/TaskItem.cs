using System.ComponentModel.DataAnnotations;

namespace AvaloniaApplication2.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
