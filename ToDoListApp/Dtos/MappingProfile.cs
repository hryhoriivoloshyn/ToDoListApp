using AutoMapper;
using ToDoListApp.Controllers;
using ToDoListApp.Models;

namespace ToDoListApp.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ToDoTask, TaskDto>();
            CreateMap<TaskDto, ToDoTask>();
            CreateMap<CreateTaskDto, ToDoTask>();
            CreateMap<ToDoTask, CreateTaskDto>();
            CreateMap<UpdateTaskDto, ToDoTask>();
            CreateMap<ToDoTask, UpdateTaskDto>();
        }
    }
}
