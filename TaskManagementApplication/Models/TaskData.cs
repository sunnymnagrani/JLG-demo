using System.ComponentModel.DataAnnotations;

namespace TaskManagementApplication.Models
{
    public class TaskData
    {
        [Key]
        public  int  taskId { get; set; }

        public required string taskTitle { get; set; }

        public  string taskDesc { get; set; }

        public  bool taskStatus { get; set; }
    }
}
