using System.Collections.Generic;

namespace ToDoListApp.Service
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public StatusEnum? Message { get; set; } = null;
        public List<string> ErrorMessages { get; set; } = null;
    }
}
