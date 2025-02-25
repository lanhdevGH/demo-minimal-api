using WebSocketChatApp.Controller.Handlers;

namespace WebSocketChatApp.Controller.Endpoins.v1
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/v1/user")
                          .WithTags("Users");

            // GET: Lấy danh sách user có phân trang
            group.MapGet("/page", UserHandler.GetPagedUsers)
                .WithName("GetPagedUsers")
                .WithOpenApi();

            // GET: Lấy tất cả users
            group.MapGet("/all", UserHandler.GetAllUsers)
                .WithName("GetAllUsers")
                .WithOpenApi();

            // GET: Lấy user theo ID
            group.MapGet("/{id}", UserHandler.GetUserById)
                .WithName("GetUserById")
                .WithOpenApi();

            // POST: Tạo user mới
            group.MapPost("/", UserHandler.CreateUser)
                .WithName("CreateUser")
                .WithOpenApi();

            // PUT: Cập nhật thông tin user
            group.MapPut("/{id}", UserHandler.UpdateUser)
                .WithName("UpdateUser")
                .WithOpenApi();

            // PUT: Đổi mật khẩu
            group.MapPut("/{userId}/change-password", UserHandler.ChangePassword)
                .WithName("ChangePassword")
                .WithOpenApi();

            // DELETE: Xóa user
            group.MapDelete("/{id}", UserHandler.DeleteUser)
                .WithName("DeleteUser")
                .WithOpenApi();
        }
    }
}
