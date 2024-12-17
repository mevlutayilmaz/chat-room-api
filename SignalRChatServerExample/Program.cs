using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using SignalRChatServerExample.Contexts;
using SignalRChatServerExample.Entities;
using SignalRChatServerExample.Hubs;
using SignalRChatServerExample.Services.AuthService;
using SignalRChatServerExample.Services.ChatRoomService;
using SignalRChatServerExample.Services.MessageServices;
using SignalRChatServerExample.Services.TokenService;
using SignalRChatServerExample.Services.UserService;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.TryAddScoped<IUserService, UserService>();
builder.Services.TryAddScoped<IAuthService, AuthService>();
builder.Services.TryAddScoped<ITokenService, TokenService>();
builder.Services.TryAddScoped<IChatRoomService, ChatRoomService>();
builder.Services.TryAddScoped<IMessageService, MessageService>();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.SetIsOriginAllowed(x => true)
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials();
}));

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;

}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

            NameClaimType = ClaimTypes.Name
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var path = context.HttpContext.Request.Path;
                if (path.StartsWithSegments("/chathub"))
                {
                    // Access token'i sorgu parametresinden alma
                    var accessToken = context.Request.Query["access_token"];

                    // Sadece SignalR endpoint'inde access token'i işle
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }
                }
                

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSignalR();

builder.Services.AddDbContext<ApplicationDbContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL Server")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/chathub");

app.MapControllers();

app.Run();
