using WebSocketChatApp.Data.Entity;

namespace WebSocketChatApp.Infrastructures.EF
{
    public interface IUserRepository
    {
        Task<List<User>> GetPagedAsync(int pageNumber, int pageSize);
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(string id);
        Task<string> CreateAsync(User user, string password);
        Task<string> UpdateAsync(User user);
        Task<string> DeleteAsync(string id);
        Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
    }
}
