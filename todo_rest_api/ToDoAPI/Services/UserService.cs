using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Data;
using TodoAPI.Dtos;
using TodoAPI.Helpers;
using TodoAPI.Models;

namespace TodoAPI.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserAsync(int userId);
        Task<(UserActionStatus, User)> CreateUserAsync(UserRequestDto userParams);
        Task<UserActionStatus> UpdateUserAsync(int userId, UserRequestDto userParams);
        Task<UserActionStatus> DeleteUserAsync(int userId);
    }

    public enum UserActionStatus
    {
        Success,
        UserIdsDoNotMatch,
        EmailTaken,
        UserNotFound
    }

    public class UserService : IUserService
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public UserService(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            return user;
        }

        public async Task<(UserActionStatus, User)> CreateUserAsync(UserRequestDto userParams)
        {
            if (UserExistsByEmail(userParams.Email))
            {
                return (UserActionStatus.EmailTaken, null);
            }

            SetUserPassword(userParams);

            var user = _mapper.Map<User>(userParams);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (UserActionStatus.Success, user);
        }

        public async Task<UserActionStatus> UpdateUserAsync(int userId, UserRequestDto userParams)
        {
            if (userParams.Id != userId)
            {
                return UserActionStatus.UserIdsDoNotMatch;
            }

            var existingUser = await GetUserAsync(userId);

            if (existingUser == null)
            {
                return UserActionStatus.UserNotFound;
            }

            _context.Entry(existingUser).State = EntityState.Detached;

            if (UserExistsByEmail(userParams.Email, userId))
            {
                return UserActionStatus.EmailTaken;
            }

            SetUserPassword(userParams);

            var user = _mapper.Map<User>(userParams);

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return UserActionStatus.Success;
        }

        public async Task<UserActionStatus> DeleteUserAsync(int userId)
        {
            var user = await GetUserAsync(userId);

            if (user == null)
            {
                return UserActionStatus.UserNotFound;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return UserActionStatus.Success;
        }

        private bool UserExistsByEmail(string email, int? userId = null)
        {
            if (userId.HasValue)
            {
                return _context.Users.Any(u => u.Email == email && u.Id != userId);
            }

            return _context.Users.Any(u => u.Email == email);
        }

        private void SetUserPassword(UserRequestDto userParams)
        {
            (var password, var salt) = PasswordHelper.CreateSecurePassword(userParams.Password);
            userParams.Salt = salt;
            userParams.Password = password;
        }
    }
}
