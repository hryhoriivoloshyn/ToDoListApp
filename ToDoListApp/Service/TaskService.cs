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

                var tasksListDto= _mapper.Map<List<ToDoTask>, List<TaskDto>>(tasksList);

                response.Success = true;
                response.Message = StatusEnum.OK;
                response.Data = tasksListDto;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Data = null;
                response.Message = StatusEnum.Error;
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
                    response.Message = StatusEnum.NotFound;
                    return response;
                }

                var taskDto = _mapper.Map<TaskDto>(task);

                response.Success = true;
                response.Message = StatusEnum.OK;
                response.Data = taskDto;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Data = null;
                response.Message = StatusEnum.Error;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }

            return response;
        }

        public ServiceResponse<TaskDto> AddTask(CreateTaskDto createTaskDto)
        {
            ServiceResponse<TaskDto> response = new ServiceResponse<TaskDto>();

            try
            {
                ToDoTask newTask = _mapper.Map<ToDoTask>(createTaskDto);

                var insertResult = _taskRepository.InsertTask(newTask);

                if (!insertResult)
                {
                    response.Success = false;
                    response.Message = StatusEnum.RepositoryError;
                    response.Data = null;
                    return response;
                }

                response.Success = true;
                response.Message = StatusEnum.OK;
                response.Data = _mapper.Map<TaskDto>(newTask);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Data = null;
                response.Message = StatusEnum.Error;
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
                    response.Message = StatusEnum.NotFound;
                    response.Data = null;
                    return response;
                }

                existingTask = _mapper.Map(updateTaskDto, existingTask);

                if (!_taskRepository.UpdateTask(existingTask))
                {
                    response.Success = false;
                    response.Message = StatusEnum.RepositoryError;
                    response.Data = null;
                    return response;
                }

                response.Data = _mapper.Map<TaskDto>(existingTask);
                response.Success = true;
                response.Message = StatusEnum.OK;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Data = null;
                response.Message = StatusEnum.Error;
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
                    response.Message = StatusEnum.NotFound;
                    response.Data = null;
                    return response;
                }

                if (!_taskRepository.DeleteTask(id))
                {
                    response.Success = false;
                    response.Message = StatusEnum.RepositoryError;
                    response.Data = null;
                    return response;
                }

                response.Success = true;
                response.Message = StatusEnum.TaskDeleted;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Data = null;
                response.Message = StatusEnum.Error;
                response.ErrorMessages = new List<string> { Convert.ToString(e.Message) };
            }

            return response;

        }
    }
}
