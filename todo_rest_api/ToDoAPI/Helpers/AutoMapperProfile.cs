using AutoMapper;
using TodoAPI.Dtos;
using TodoAPI.Models;

namespace TodoAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserResponseDto>();
            CreateMap<UserResponseDto, User>();
            CreateMap<User, UserRequestDto>();
            CreateMap<UserRequestDto, User>();

            CreateMap<TodoList, TodoListResponseDto>();
            CreateMap<TodoListResponseDto, TodoList>();
            CreateMap<TodoList, TodoListRequestDto>();
            CreateMap<TodoListRequestDto, TodoList>();
        }
    }
}