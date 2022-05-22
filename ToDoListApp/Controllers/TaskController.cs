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
            return Ok(tasks.Data);
        }

        [HttpGet("{TaskId:int}",Name = "GetTaskById")]
        public IActionResult GetTaskById(int TaskId)
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

            return Ok(taskFound.Data);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateTaskDto createTaskDto)
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


            if (newTask.Success == false && newTask.Message == StatusEnum.RepositoryError)
            {
                ModelState.AddModelError("",
                    $"Something went wrong in data access layer when adding task {createTaskDto}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            if (newTask.Success == false && newTask.Message == StatusEnum.Error)
            {
                ModelState.AddModelError("",
                    $"Something went wrong in _service layer when adding task {createTaskDto}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok(newTask.Data);
        }

        [HttpPut("{TaskId:int}")]
        public IActionResult UpdateTask(int TaskId, [FromBody] UpdateTaskDto updateTaskDto)
        {
            if (updateTaskDto == null || updateTaskDto.Id != TaskId)
            {
                return BadRequest(ModelState);
            }

            var updateTask = _service.UpdateTask(updateTaskDto);

            if (updateTask.Success == false && updateTask.Message == StatusEnum.NotFound)
            {
                return NotFound(updateTask.Data);
            }

            if (updateTask.Success == false && updateTask.Message == StatusEnum.RepositoryError)
            {
                ModelState.AddModelError("",
                    $"Some thing went wrong in the data access layer when updating task {updateTaskDto}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            if (updateTask.Success == false && updateTask.Message == StatusEnum.Error)
            {
                ModelState.AddModelError("",
                    $"Something went wrong in the _service layer when updating task {updateTaskDto}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok(updateTask.Data);

            
        }

        [HttpDelete("{TaskId:int}")]
        public IActionResult DeleteTask(int TaskId)
        {
            var deleteTask = _service.DeleteTask(TaskId);

            if (deleteTask.Success == false && deleteTask.Message == StatusEnum.NotFound)
            {
                ModelState.AddModelError("", "Task Not Found");
                return NotFound(ModelState);
            }

            if (deleteTask.Success == false && deleteTask.Message == StatusEnum.RepositoryError)
            {
                ModelState.AddModelError("",
                    $"Something went wrong in data access layer when deleting task with id {TaskId}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            if (deleteTask.Success == false && deleteTask.Message == StatusEnum.Error)
            {
                ModelState.AddModelError("", 
                    $"Something went wrong in _service layer when deleting task with id {TaskId}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok();
        }

    }
}
