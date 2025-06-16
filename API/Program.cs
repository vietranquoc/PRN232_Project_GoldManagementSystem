using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BusinessObjects.DBContext;
using Repositories.Infrastructure.Implementations;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;
using Services.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();

// Thêm DbContext cho SQL Server
builder.Services.AddDbContext<GoldManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseLazyLoadingProxies());

// Thêm Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGoldTypeRepository, GoldTypeRepository>();
builder.Services.AddScoped<IGoldPriceRepository, GoldPriceRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// Thêm Services
builder.Services.AddHttpClient<IGoldPriceService, GoldPriceService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

// Thêm xác thực JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "GoldManagementApi",
        ValidAudience = "GoldManagementApi",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };
});

builder.Services.AddControllers();  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
