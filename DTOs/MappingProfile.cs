using AutoMapper;
using WebSocketChatApp.Data.Entity;
using WebSocketChatApp.DTOs.UserDTOs;

namespace WebSocketChatApp.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserResponseDTO>();
            //
            CreateMap<UserCreateRequestDTO, User>();
            CreateMap<UserUpdateRequestDTO, User>();
        }
    }
}
