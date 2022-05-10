using System.ComponentModel.DataAnnotations;

namespace ToDoListApp.Dtos
{
    public class CreateTaskDto
    {
        [Required(ErrorMessage = "Task name is required")]
        [MaxLength(50, ErrorMessage = "Task name is too long")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
