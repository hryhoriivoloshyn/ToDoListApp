using System.ComponentModel.DataAnnotations;

namespace ToDoListApp.Controllers
{
    public class UpdateTaskDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
    }
}
