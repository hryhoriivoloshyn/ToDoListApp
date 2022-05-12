using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoListApp.Data;
using ToDoListApp.Dtos;
using ToDoListApp.Models;
using ToDoListApp.Service;

namespace ToDoListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _service;

        public TasksController(ITaskService service)
        {
            _service= service;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var tasks = _service.GetTasks();
            return Ok(tasks);
        }

        [HttpGet("{TaskId:int}",Name = "GetTaskById")]
        public ActionResult<TaskDto> GetTaskById(int TaskId)
        {
            if (TaskId <= 0)
            {
                return BadRequest(TaskId);
            }

            var taskFound = _service.GetById(TaskId);

            if (taskFound.Data == null)
            {
                return NotFound();
            }

            return Ok(taskFound);
        }

        [HttpPost]
        public ActionResult<TaskDto> Post([FromBody] CreateTaskDto createTaskDto)
        {
            if (createTaskDto == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newTask = _service.AddTask(createTaskDto);

            if (newTask.Success == false && newTask.Message == "Exist")
            {
                return Ok(newTask);
            }

            if (newTask.Success == false && newTask.Message == "RepositoryError")
            {
                ModelState.AddModelError("",
                    $"Something went wrong in data access layer when adding task {createTaskDto}");
                return StatusCode(500, ModelState);
            }

            if (newTask.Success == false && newTask.Message == "Error")
            {
                ModelState.AddModelError("",
                    $"Something went wrong in _service layer when adding task {createTaskDto}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTaskById", new {TaskId=newTask.Data.Id}, newTask);
        }

        [HttpPut("{TaskId:int}")]
        public IActionResult UpdateTask(int TaskId, [FromBody] UpdateTaskDto updateTaskDto)
        {
            if (updateTaskDto == null && updateTaskDto.Id != TaskId)
            {
                return BadRequest(ModelState);
            }

            var updateTask = _service.UpdateTask(updateTaskDto);

            if (updateTask.Success == false && updateTask.Message == "NotFound")
            {
                return NotFound(updateTask);
            }

            if (updateTask.Success == false && updateTask.Message == "RepositoryError")
            {
                ModelState.AddModelError("",
                    $"Some thing went wrong in the data access layer when updating task {updateTaskDto}");
                return StatusCode(500, ModelState);
            }

            if (updateTask.Success == false && updateTask.Message == "Error")
            {
                ModelState.AddModelError("",
                    $"Something went wrong in the _service layer when updating task {updateTaskDto}");
                return StatusCode(500, ModelState);
            }

            return Ok(updateTask);

            
        }

        [HttpDelete("{TaskId:int}")]
        public IActionResult DeleteTask(int TaskId)
        {
            var deleteTask = _service.DeleteTask(TaskId);

            if (deleteTask.Success == false && deleteTask.Message == "NotFound")
            {
                ModelState.AddModelError("", "Task Not Found");
                return NotFound(ModelState);
            }

            if (deleteTask.Success == false && deleteTask.Message == "RepositoryError")
            {
                ModelState.AddModelError("",
                    $"Something went wrong in data access layer when deleting task with id {TaskId}");
                return StatusCode(500, ModelState);
            }

            if (deleteTask.Success == false && deleteTask.Message == "Error")
            {
                ModelState.AddModelError("", 
                    $"Something went wrong in _service layer when deleting task with id {TaskId}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
