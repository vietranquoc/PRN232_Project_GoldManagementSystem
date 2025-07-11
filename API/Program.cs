using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BusinessObjects.DBContext;
using BusinessObjects.EntityModel;
using Repositories.Infrastructure.Implementations;
using Repositories.Infrastructure.Interfaces;
using Services.Services.Interfaces;
using Services.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

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
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();

// Thêm Services
builder.Services.AddHttpClient<IGoldPriceService, GoldPriceService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();

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

app.UseCors("CORSPolicy");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed dữ liệu động khi khởi động project
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GoldManagementContext>();

    // Seed Manager account nếu chưa có
    if (!context.Users.Any(u => u.RoleId == 3))
    {
        var manager = new User
        {
            FullName = "Manager",
            Username = "manager",
            // Hash password nếu hệ thống có hash, ở đây dùng plain cho demo
            Password = BCrypt.Net.BCrypt.HashPassword("123456"),
            RoleId = 3,
            Email = "manager@example.com",
            Address = "",
            PhoneNumber = "",
            IsActive = true
        };
        context.Users.Add(manager);
        context.SaveChanges();
    }
}

app.Run();
