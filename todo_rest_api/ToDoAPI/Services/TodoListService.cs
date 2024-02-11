using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Data;
using TodoAPI.Dtos;
using TodoAPI.Models;

namespace TodoAPI.Services
{
    public interface ITodoListService
    {
        Task<IEnumerable<TodoList>> GetTodoListsAsync();
        Task<TodoList> GetTodoListAsync(int todoListId);
        Task<IEnumerable<TodoList>> GetTodoListsByUserAsync(int userId);
        Task<TodoList> GetTodoListByUserAsync(int userId, int todoListId);
        Task<TodoList> CreateTodoListAsync(TodoListRequestDto todoListParams, User user);
    }

    public class TodoListService : ITodoListService
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public TodoListService(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TodoList>> GetTodoListsAsync()
        {
            var todoLists = await _context.TodoLists
                                        .Include(t => t.User)
                                        .Include(t => t.TodoItems)
                                        .ToListAsync();

            return todoLists;
        }

        public async Task<TodoList> GetTodoListAsync(int todoListId)
        {
            var todoList = await _context.TodoLists
                                        .Include(t => t.User)
                                        .Include(t => t.TodoItems)
                                        .SingleOrDefaultAsync(t => t.Id == todoListId);

            return todoList;
        }

        public async Task<IEnumerable<TodoList>> GetTodoListsByUserAsync(int userId)
        {
            var todoLists = await _context.TodoLists
                                        .Include(t => t.User)
                                        .Include(t => t.TodoItems)
                                        .Where(t => t.User.Id == userId)
                                        .ToListAsync();
            return todoLists;
        }

        public async Task<TodoList> GetTodoListByUserAsync(int userId, int todoListId)
        {
            var todoList = await _context.TodoLists
                                        .Include(t => t.User)
                                        .Include(t => t.TodoItems)
                                        .Where(t => t.Id == todoListId && t.User.Id == userId)
                                        .SingleOrDefaultAsync();

            return todoList;
        }

        public async Task<TodoList> CreateTodoListAsync(TodoListRequestDto todoListParams, User user)
        {
            todoListParams.User = user;

            var todoList = _mapper.Map<TodoList>(todoListParams);

            _context.TodoLists.Add(todoList);
            await _context.SaveChangesAsync();

            return todoList;
        }
    }
}
