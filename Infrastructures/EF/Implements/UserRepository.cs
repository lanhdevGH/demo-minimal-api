using WebSocketChatApp.Data.Entity;
using Microsoft.EntityFrameworkCore;
using WebSocketChatApp.Data;
using Microsoft.AspNetCore.Identity;

namespace WebSocketChatApp.Infrastructures.EF.Implements
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<string> CreateAsync(User user, string password)
        {
            // Kiểm tra username đã tồn tại
            var existingUserByName = await _userManager.FindByNameAsync(user.UserName);
            if (existingUserByName != null)
            {
                throw new InvalidOperationException($"Username '{user.UserName}' đã được sử dụng");
            }

            // Kiểm tra email đã tồn tại
            var existingUserByEmail = await _userManager.FindByEmailAsync(user.Email);  
            if (existingUserByEmail != null)
            {
                throw new InvalidOperationException($"Email '{user.Email}' đã được sử dụng");
            }

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            return user.Id.ToString();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<List<User>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<string> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user.Id.ToString();
        }

        public async Task<string> DeleteAsync(string id)
        {
            var user = await GetByIdAsync(id);
            if (user == null) 
                throw new KeyNotFoundException($"User with id {id} not found");
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with id {userId} not found");

            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            
            if (!result.Succeeded)
                throw new InvalidOperationException($"Failed to change password: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            return true;
        }
    }   
}
