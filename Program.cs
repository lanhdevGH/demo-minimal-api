using WebSocketChatApp.Configurations;
using WebSocketChatApp.Controller.Endpoins.v1;
using WebSocketChatApp.Services;
using WebSocketChatApp.Services.Implements;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

// Services
builder.Services.ConfigDbContext(builder.Configuration);
builder.Services.ConfigIdentity();
builder.Services.ConfigAuthentication(builder.Configuration);
builder.Services.ConfigCors();
builder.Services.ConfigureSwagger();
//
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Minimal API V1");
        c.RoutePrefix = string.Empty; // Optional: Serve Swagger UI at the root URL
    });
}

//app.MapGet("/hello", () => "Hello World!");
app.UseHttpsRedirection();
app.MapUserEndpoints();
app.Run();
