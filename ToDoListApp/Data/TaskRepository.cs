using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoListApp.Models;

namespace ToDoListApp.Data
{
    public class TaskRepository : ITaskRepository, IDisposable
    {
        private ApplicationDbContext context;

        public TaskRepository(ApplicationDbContext _context)
        {
            context=_context;
        }


        public IEnumerable<Tasks> GetAllTasks()
        {
            return context.Tasks.ToList();
        }

        public Tasks GetTaskById(int taskId)
        {
            return context.Tasks.Find(taskId);
        }

        public void InsertTask(Tasks task)
        {
            context.Tasks.Add(task);
        }

        public void UpdateTask(Tasks task)
        {
            context.Tasks.Update(task);
        }

        public void DeleteTask(int taskId)
        {
            Tasks task = context.Tasks.Find(taskId);
            context.Tasks.Remove(task);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed=true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
