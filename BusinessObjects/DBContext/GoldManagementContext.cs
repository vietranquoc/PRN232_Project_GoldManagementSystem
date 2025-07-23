using BusinessObjects.EntityModel;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjects.DBContext
{
    public class GoldManagementContext : DbContext
    {
        public GoldManagementContext(DbContextOptions<GoldManagementContext> options) : base(options)
        {
        }

        #region DbSet
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GoldType> GoldTypes { get; set; }
        public DbSet<GoldPrice> GoldPrices { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình Role
            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Cấu hình User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            
            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);
            
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(256);

            // Cấu hình decimal cho GoldPrice
            modelBuilder.Entity<GoldPrice>()
                .Property(gp => gp.BuyPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<GoldPrice>()
                .Property(gp => gp.SellPrice)
                .HasPrecision(18, 2);

            // Cấu hình decimal cho Transaction
            modelBuilder.Entity<Transaction>()
                .Property(t => t.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.UnitPrice)
                .HasPrecision(18, 2);

            // Cấu hình decimal cho TransactionDetail
            modelBuilder.Entity<TransactionDetail>()
                .Property(td => td.UnitPrice)
                .HasPrecision(18, 2);
            modelBuilder.Entity<TransactionDetail>()
                .Property(td => td.TotalAmount)
                .HasPrecision(18, 2);

            // Cấu hình mối quan hệ
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
            
            modelBuilder.Entity<GoldPrice>()
                .HasOne(gp => gp.GoldType)
                .WithMany(gt => gt.GoldPrices)
                .HasForeignKey(gp => gp.GoldTypeId);
            
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.GoldType)
                .WithMany(gt => gt.Transactions)
                .HasForeignKey(t => t.GoldTypeId);
            
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Cấu hình ràng buộc
            modelBuilder.Entity<User>()
                .Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<GoldType>()
                .Property(gt => gt.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Thêm index
            modelBuilder.Entity<GoldPrice>()
                .HasIndex(gp => gp.RecordedAt);

            modelBuilder.Entity<Transaction>()
                .HasIndex(t => t.TransactionDate);

            modelBuilder.Entity<GoldType>()
                .Property(gt => gt.PriceType)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<GoldType>()
                .Property(gt => gt.Karat)
                .HasPrecision(2, 0); // tối đa 99K, đủ dùng

            modelBuilder.Entity<GoldType>()
                .HasIndex(gt => new { gt.Name, gt.Karat, gt.PriceType })
                .IsUnique(); // tránh bị trùng loại vàng

            // Cấu hình Category
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Cấu hình Product
            modelBuilder.Entity<Product>()
                .Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CartItem>()
                .Property(ci => ci.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.GoldType)
                .WithMany()
                .HasForeignKey(p => p.GoldTypeId);

            // Cấu hình ProductImage
            modelBuilder.Entity<ProductImage>()
                .Property(pi => pi.ImageUrl)
                .IsRequired();

            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductId);



            #region Seed Data
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Customer", Description = "Khách hàng mua/bán vàng" },
                new Role { Id = 2, Name = "Employee", Description = "Nhân viên thực hiện giao dịch" },
                new Role { Id = 3, Name = "Manager", Description = "Quản lý tiệm vàng" }
            );
            #endregion
        }
    }
}
