using System.Collections.Generic;
using ToDoListApp.Controllers;
using ToDoListApp.Dtos;
using ToDoListApp.Models;

namespace ToDoListApp.Service
{
    public interface ITaskService
    {
        ServiceResponse<List<TaskDto>> GetTasks();
        ServiceResponse<TaskDto> GetById(int id);
        ServiceResponse<TaskDto> AddTask(CreateTaskDto createTaskDto);
        ServiceResponse<TaskDto> UpdateTask(UpdateTaskDto updateTaskDto);
        ServiceResponse<TaskDto> DeleteTask(int id);
    }
}
