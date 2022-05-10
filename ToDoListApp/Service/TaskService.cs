using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ToDoListApp.Controllers;
using ToDoListApp.Data;
using ToDoListApp.Dtos;
using ToDoListApp.Models;

namespace ToDoListApp.Service
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        private readonly IMapper _mapper;

        public TaskService(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper= mapper;
        }


        public ServiceResponse<List<TaskDto>> GetTasks()
        {
            ServiceResponse<List<TaskDto>> response = new ServiceResponse<List<TaskDto>>();

            try
            {

                var tasksList = _taskRepository.GetAllTasks().ToList();

                var tasksListDto = new List<TaskDto>();

                tasksListDto= _mapper.Map<List<Tasks>, List<TaskDto>>(tasksList);

                response.Success = true;
                response.Message = "ok";
                response.Data = tasksListDto;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }

            return response;
        }

        public ServiceResponse<TaskDto> GetById(int id)
        {
            ServiceResponse<TaskDto> response = new ServiceResponse<TaskDto>();

            try
            {

                var task = _taskRepository.GetTaskById(id);

                if (task == null)
                {
                    response.Success = false;
                    response.Message = "Not Found";
                    return response;
                }

                var taskDto = _mapper.Map<TaskDto>(task);

                response.Success = true;
                response.Message = "ok";
                response.Data = taskDto;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }

            return response;
        }

        public ServiceResponse<TaskDto> GetByName(string name)
        {
            ServiceResponse<TaskDto> response = new ServiceResponse<TaskDto>();

            try
            {
                var task = _taskRepository.GetTaskByName(name);

                if (task == null)
                {
                    response.Success = false;
                    response.Message = "Not Found";
                    return response;
                }

                var taskDto = _mapper.Map<TaskDto>(task);

                response.Success = true;
                response.Message = "ok";
                response.Data = taskDto;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }

            return response;
        }

        public ServiceResponse<TaskDto> AddTask(CreateTaskDto createTaskDto)
        {
            ServiceResponse<TaskDto> response = new ServiceResponse<TaskDto>();

            try
            {
                Tasks newTask = new Tasks()
                {
                    Name = createTaskDto.Name,
                    Description = createTaskDto.Description,
                    DateTime = DateTime.Now,
                    IsCompleted = false
                };

                if (!_taskRepository.InsertTask(newTask))
                {
                    response.Success = false;
                    response.Message = "RepositoryError";
                    response.Data = null;
                    return response;
                }

                response.Success = true;
                response.Data = _mapper.Map<TaskDto>(newTask);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }

            return response;
        }

        public ServiceResponse<TaskDto> UpdateTask(UpdateTaskDto updateTaskDto)
        {
            ServiceResponse<TaskDto> response = new ServiceResponse<TaskDto>();

            try
            {
                var existingTask = _taskRepository.GetTaskById(updateTaskDto.Id);

                if (existingTask == null)
                {
                    response.Success = false;
                    response.Message = "NotFound";
                    response.Data = null;
                    return response;
                }

                existingTask.Name=updateTaskDto.Name;
                existingTask.Description=updateTaskDto.Description;
                existingTask.IsCompleted=updateTaskDto.IsCompleted;

                if (!_taskRepository.UpdateTask(existingTask))
                {
                    response.Success = false;
                    response.Message = "RepositoryError";
                    response.Data = null;
                    return response;
                }

                var taskDto = _mapper.Map<TaskDto>(existingTask);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }

            return response;
        }

        public ServiceResponse<TaskDto> DeleteTask(int id)
        {
            ServiceResponse<TaskDto> response = new ServiceResponse<TaskDto>();

            try
            {
                var existingTask = _taskRepository.IsTaskExists(id);

                if (existingTask == false)
                {
                    response.Success = false;
                    response.Message = "NotFound";
                    response.Data = null;
                    return response;
                }

                if (!_taskRepository.DeleteTask(id))
                {
                    response.Success = false;
                    response.Message = "RepositoryError";
                    response.Data = null;
                    return response;
                }

                response.Success = true;
                response.Message = "TaskDeleted";
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Error";
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }

            return response;

        }
    }
}
