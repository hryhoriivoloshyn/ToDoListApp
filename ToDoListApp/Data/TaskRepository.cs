using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoListApp.Models;

namespace ToDoListApp.Data
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context=context;
        }

        public IEnumerable<ToDoTask> GetAllTasks()
        {
            return _context.Tasks.ToList();
        }

        public ToDoTask GetTaskById(int taskId)
        {
            return _context.Tasks.Find(taskId);
        }

        public bool IsTaskExists(int id)
        {
            return _context.Tasks.Any(t => t.Id == id);
        }

        public bool InsertTask(ToDoTask task)
        {
            _context.Tasks.Add(task);
            return Save();
        }

        public bool UpdateTask(ToDoTask task)
        {
            _context.Tasks.Update(task);
            return Save();
        }

        public bool DeleteTask(int taskId)
        {
            ToDoTask task = _context.Tasks.Find(taskId);
            _context.Tasks.Remove(task);
            return Save();
        }

        private bool Save()
        {
            return _context.SaveChanges() >=0 ? true : false;
        }
    }
}
