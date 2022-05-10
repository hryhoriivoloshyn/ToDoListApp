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
        void InsertTask(Tasks task);
        void UpdateTask(Tasks task);
        void DeleteTask(int taskId);
        void Save();
    }
}
