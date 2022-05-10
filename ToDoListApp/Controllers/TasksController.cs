using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListApp.Data;
using ToDoListApp.Models;

namespace ToDoListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context=context;
        }
        [HttpGet]
        public IEnumerable<Tasks> Get()
        {
            IEnumerable<Tasks> tasks= _context.Tasks.ToArray();
            return tasks;
        }

        [HttpPost]
        public void Post()
        {

        }
    }
}
