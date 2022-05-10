using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListApp.Models;

namespace ToDoListApp.Data
{
    public interface ITaskRepository : IDisposable
    {
        IEnumerable<Tasks> GetAllTasks();
        Tasks GetTaskById(int taskId);
        Tasks GetTaskByName(string name);
        bool IsTaskExists(int taskId);
        bool InsertTask(Tasks task);
        bool UpdateTask(Tasks task);
        bool DeleteTask(int taskId);
        bool Save();
    }
}
