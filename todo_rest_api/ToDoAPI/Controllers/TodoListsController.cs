using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoAPI.Dtos;
using TodoAPI.Errors;
using TodoAPI.Services;

namespace TodoAPI.Controllers
{ 
    [Authorize]
    [Route("api/todo-lists")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly ITodoListService _todoListService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public TodoListsController(ITodoListService todoListService, IUserService userService, IMapper mapper)
        {
            _todoListService = todoListService;
            _userService = userService;
            _mapper = mapper;
        }

        // GET: api/todo-lists
        // Fetches all todo-lists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoListResponseDto>>> GetTodoLists()
        {
            var todoLists = await _todoListService.GetTodoListsAsync();
            var todoListDtos = _mapper.Map<IEnumerable<TodoListResponseDto>>(todoLists).ToList();

            return Ok(todoListDtos);
        }

        // GET: api/todo-lists/5
        // Fetches a single todo-list
        [HttpGet("{todoListId:int}")]
        public async Task<ActionResult<TodoListResponseDto>> GetTodoList(int todoListId)
        {
            var todoList = await _todoListService.GetTodoListAsync(todoListId);

            if (todoList == null)
            {
                return NotFound(new NotFoundError("The todo-list was not found."));
            }

            var todoListDto = _mapper.Map<TodoListResponseDto>(todoList);

            return Ok(todoListDto);
        }

        // POST: api/todo-lists
        // Creates a new todo-list 
        [HttpPost]
        public async Task<ActionResult<TodoListResponseDto>> CreateTodoList([FromBody]TodoListRequestDto todoListParams)
        {
            var userIdName = User.FindFirstValue(ClaimTypes.Name);
            
            if (!Int32.TryParse(userIdName, out int userId))
            {
                return NotFound(new NotFoundError("The authentication identity was not found."));
            }

            var currentUser = await _userService.GetUserAsync(userId);

            if (currentUser == null)
            {
                return NotFound(new NotFoundError("The authenticated user was not found."));
            }

            var todoList = await _todoListService.CreateTodoListAsync(todoListParams, currentUser);
            var todoListDto = _mapper.Map<TodoListResponseDto>(todoList);

            return CreatedAtAction(nameof(GetTodoList), new { todoListId = todoListDto.Id }, todoListDto);
        }

        // GET: api/users/5/todo-lists
        // Fetches all todo-lists belonging to a single user
        [HttpGet("~/api/users/{userId:int}/todo-lists")]
        public async Task<ActionResult<IEnumerable<TodoListResponseDto>>> GetTodoListsByUser(int userId)
        {
            var user = await _userService.GetUserAsync(userId);

            if (user == null)
            {
                return NotFound(new NotFoundError("The user was not found."));
            }

            var todoLists = await _todoListService.GetTodoListsByUserAsync(userId);
            var todoListDtos = _mapper.Map<IEnumerable<TodoListResponseDto>>(todoLists).ToList();

            return Ok(todoListDtos);
        }

        // GET: api/users/5/todo-lists/5
        // Fetches a single todo-list belonging to a single user
        [HttpGet("~/api/users/{userId:int}/todo-lists/{todoListId:int}")]
        public async Task<ActionResult<TodoListResponseDto>> GetTodoListByUser(int userId, int todoListId)
        {
            var user = await _userService.GetUserAsync(userId);

            if (user == null)
            {
                return NotFound(new NotFoundError("The user was not found."));
            }

            var todoList = await _todoListService.GetTodoListByUserAsync(userId, todoListId);

            if (todoList == null)
            {
                return NotFound(new NotFoundError("The todo-list was not found."));
            }

            var todoListDto = _mapper.Map<TodoListResponseDto>(todoList);

            return Ok(todoListDto);
        }
    }
}
