using AutoMapper;
using ToDoListApp.Models;

namespace ToDoListApp.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tasks, TaskDto>();
            CreateMap<TaskDto, Tasks>();
        }
    }
}
