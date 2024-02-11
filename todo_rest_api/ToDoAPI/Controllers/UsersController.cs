using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using AutoMapper;
using TodoAPI.Dtos;
using TodoAPI.Errors;
using TodoAPI.Services;

namespace TodoAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // GET: api/users
        // Fetches all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            var userDtos = _mapper.Map<IEnumerable<UserResponseDto>>(users).ToList();

            return Ok(userDtos);
        }

        // GET: api/users/5
        // Fetches a single user
        [HttpGet("{userId:int}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int userId)
        {
            var user = await _userService.GetUserAsync(userId);

            if (user == null)
            {
                return NotFound(new NotFoundError("The user was not found."));
            }

            var userDto = _mapper.Map<UserResponseDto>(user);

            return Ok(userDto);
        }

        // POST: api/users
        // Creates a new user
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> PostUser([FromBody]UserRequestDto userParams)
        {
            (var userCreateStatus, var createdUser) = await _userService.CreateUserAsync(userParams);

            if (userCreateStatus == UserActionStatus.Success)
            {
                var userDto = _mapper.Map<UserResponseDto>(createdUser);

                return CreatedAtAction(nameof(GetUser), new { userId = userDto.Id }, userDto);
            }

            return HandleUserActionRequest(userCreateStatus);
        }

        // PUT: api/users/5
        // Updates a single user
        [HttpPut("{userId:int}")]
        public async Task<ActionResult> PutUser(int userId, [FromBody]UserRequestDto userParams)
        {
            var userUpdateStatus = await _userService.UpdateUserAsync(userId, userParams);

            return HandleUserActionRequest(userUpdateStatus);
        }

        // DELETE: api/users/5
        // Deletes a single user
        [HttpDelete("{userId:int}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            var userDeleteResult = await _userService.DeleteUserAsync(userId);

            return HandleUserActionRequest(userDeleteResult);
        }

        private ActionResult HandleUserActionRequest(UserActionStatus userActionStatus)
        {
            switch (userActionStatus)
            {
                case UserActionStatus.UserIdsDoNotMatch:
                    return BadRequest(new BadRequestError("UserId in params and body do not match."));
                case UserActionStatus.UserNotFound:
                    return BadRequest(new BadRequestError("User was not found."));
                case UserActionStatus.EmailTaken:
                    return BadRequest(new BadRequestError("Email is already taken."));
            }

            return NoContent();
        }
    }
}