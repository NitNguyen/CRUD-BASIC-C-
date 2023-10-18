using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAPI.Models
{
    [Table("tasks")]
    public class TaskItem
    {
        public int id { get; set; } 
        public string title { get; set; }
        public bool is_deleted { get; set; }
    }
}
