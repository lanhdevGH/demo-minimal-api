using Microsoft.AspNetCore.Mvc;
using WebSocketChatApp.DTOs.GenericDTOs;
using WebSocketChatApp.DTOs.UserDTOs;
using WebSocketChatApp.Services;

namespace WebSocketChatApp.Controller.Handlers
{
    public static class UserHandler
    {
        public static async Task<IResult> GetPagedUsers(HttpContext context,
            [FromServices] IUserService userService,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await userService.GetPagedAsync(pageNumber, pageSize);
            return Results.Ok(result);
        }

        public static async Task<IResult> GetAllUsers(HttpContext context,
            [FromServices] IUserService userService)
        {
            var users = await userService.GetAllAsync();
            return Results.Ok(users);
        }

        public static async Task<IResult> GetUserById(HttpContext context,
            [FromServices] IUserService userService,
             [FromQuery] string id)
        {
            var user = await userService.GetByIdAsync(id);
            if (user == null)
                return Results.NotFound("User not found");
            return Results.Ok(user);
        }

        public static async Task<IResult> CreateUser(HttpContext context,
            [FromServices] IUserService userService,
            [FromBody] UserCreateRequestDTO createRequestDTO)
        {
            try
            {
                if (createRequestDTO == null)
                    return Results.BadRequest("Invalid user data");

                var userId = await userService.CreateAsync(createRequestDTO);
                return Results.Ok(userId);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        public static async Task<IResult> ChangePassword(HttpContext context,
            [FromServices] IUserService userService,
            [FromQuery] string userId,
            [FromBody] ChangePasswordDTO changePassword)
        {
            try
            {
                if (changePassword == null)
                    return Results.BadRequest("Invalid password data");

                var result = await userService.ChangePasswordAsync(userId,
                    changePassword.CurrentPassword,
                    changePassword.NewPassword);
                return Results.Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound("User not found");
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        public static async Task<IResult> UpdateUser(HttpContext context,
            [FromServices] IUserService userService,
            [FromQuery] string id,
            [FromBody] UserUpdateRequestDTO userUpdate)
        {
            if (userUpdate == null)
                return Results.BadRequest("Invalid user data");

            var result = await userService.UpdateAsync(id, userUpdate);
            if (!result)
                return Results.NotFound("User not found");
            return Results.Ok();
        }

        public static async Task<IResult> DeleteUser(HttpContext context,
            [FromServices] IUserService userService,
            [FromQuery] string id)
        {
            var result = await userService.DeleteAsync(id);
            if (!result)
                return Results.NotFound("User not found");
            return Results.Ok();
        }
    }
}
