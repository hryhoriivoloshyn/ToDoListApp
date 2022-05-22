using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListApp.Models;

namespace ToDoListApp.Data
{
    public interface ITaskRepository : IDisposable
    {
        IEnumerable<ToDoTask> GetAllTasks();
        ToDoTask GetTaskById(int taskId);
        bool IsTaskExists(int taskId);
        bool InsertTask(ToDoTask task);
        bool UpdateTask(ToDoTask task);
        bool DeleteTask(int taskId);
        bool Save();
    }
}
