﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WebSocketChatApp.Data.Entity;
using WebSocketChatApp.Data;
using WebSocketChatApp.DTOs.GenericDTOs;
using WebSocketChatApp.DTOs.UserDTOs;
using Microsoft.EntityFrameworkCore;

namespace WebSocketChatApp.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<PagedResult<UserResponseDTO>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _userManager.Users.AsQueryable();

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.UserName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => _mapper.Map<UserResponseDTO>(p))
                .ToListAsync();

            return new PagedResult<UserResponseDTO>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<List<UserResponseDTO>> GetAllAsync()
        {
            return await _userManager.Users.Select(p => _mapper.Map<UserResponseDTO>(p)).ToListAsync();
        }

        public async Task<UserResponseDTO?> GetByIdAsync(string id)
        {
            var entityVal = await _userManager.FindByIdAsync(id);
            if (entityVal == null) return null;

            var result = _mapper.Map<UserResponseDTO>(entityVal);
            return result;
        }

        public async Task<string> CreateAsync(UserCreateRequestDTO userCreateDTO)
        {
            var newUser = _mapper.Map<User>(userCreateDTO);
            newUser.Id = Guid.NewGuid();
            var result = await _userManager.CreateAsync(newUser, userCreateDTO.Password);
            if (!result.Succeeded)
            {
                /// Tìm lỗi liên quan đến mật khẩu trong danh sách lỗi
                var passwordErrors = result.Errors
                    .Where(e => e.Code.Contains("Password"))
                    .Select(e => e.Description);

                if (passwordErrors.Any())
                {
                    throw new Exception();
                }

                // Ném exception chung nếu lỗi không phải do mật khẩu
                var allErrors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception();
            }
            return newUser.Id.ToString();
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User id not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
            {
                // Ném exception chung nếu lỗi không phải do mật khẩu
                var allErrors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception();
            }
            return result.Succeeded;
        }

        public async Task<bool> UpdateAsync(string id, UserUpdateRequestDTO userUpdateDTO)
        {
            var existingUser = await _userManager.FindByIdAsync(id.ToString());
            if (existingUser == null) return false;

            _mapper.Map(userUpdateDTO, existingUser);

            await _userManager.UpdateAsync(existingUser);
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            await _userManager.DeleteAsync(user);
            return true;
        }
    }
}
