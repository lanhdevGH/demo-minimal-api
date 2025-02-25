using AutoMapper;
using WebSocketChatApp.Data.Entity;
using WebSocketChatApp.DTOs.GenericDTOs;
using WebSocketChatApp.DTOs.UserDTOs;
using WebSocketChatApp.Infrastructures.EF;

namespace WebSocketChatApp.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<PagedResult<UserResponseDTO>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var items = await _userRepository.GetAllAsync();

            var totalItems = items.Count;
            var result = items
                .OrderByDescending(p => p.UserName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => _mapper.Map<UserResponseDTO>(p))
                .ToList();
            return new PagedResult<UserResponseDTO>
            {
                Items = result,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<List<UserResponseDTO>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(p => _mapper.Map<UserResponseDTO>(p)).ToList();
        }

        public async Task<UserResponseDTO?> GetByIdAsync(string id)
        {
            var entityVal = await _userRepository.GetByIdAsync(id);
            if (entityVal == null) return null;

            var result = _mapper.Map<UserResponseDTO>(entityVal);
            return result;
        }

        public async Task<string> CreateAsync(UserCreateRequestDTO userCreateDTO)
        {
            var newUser = _mapper.Map<User>(userCreateDTO);
            newUser.Id = Guid.NewGuid();
            var result = await _userRepository.CreateAsync(newUser, userCreateDTO.Password);
            if (result == null)
            {
                throw new Exception("User creation failed.");
            }
            return newUser.Id.ToString();
        }

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User id not found");
            }

            return await _userRepository.ChangePasswordAsync(userId,currentPassword, newPassword);
        }

        public async Task<bool> UpdateAsync(string id, UserUpdateRequestDTO userUpdateDTO)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null) return false;

            _mapper.Map(userUpdateDTO, existingUser);

            await _userRepository.UpdateAsync(existingUser);
            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try 
            {
                await _userRepository.DeleteAsync(id);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}
