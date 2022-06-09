using AutoMapper;
using ToDoListApp.Controllers;
using ToDoListApp.Models;

namespace ToDoListApp.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ToDoTask, TaskDto>().ReverseMap();
            CreateMap<CreateTaskDto, ToDoTask>().ReverseMap();
            CreateMap<UpdateTaskDto, ToDoTask>().ReverseMap();
        }
    }
}
