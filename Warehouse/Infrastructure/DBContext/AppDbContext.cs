using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DBContext
{
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
    : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseItem> WarehouseItems { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var passwordHash = BCrypt.Net.BCrypt.HashPassword("P@ssw0rd");

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Email = "admin@happywarehouse.com",
                FullName = "Admin User",
                PasswordHash = passwordHash,
                Role = "Admin",
                IsActive = true,
                CreatedBy = "System",
                UpdatedBy = "System",
            });

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Warehouse>()
                .HasIndex(w => w.Name)
                .IsUnique();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var baseEntity = (BaseEntity)entityEntry.Entity;
                var now = DateTime.UtcNow;
                var user = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "System";

                if (entityEntry.State == EntityState.Added)
                {
                    baseEntity.CreatedAt = now;
                    baseEntity.CreatedBy = user;
                }
                else
                {
                    baseEntity.UpdatedAt = now;
                    baseEntity.UpdatedBy = user;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
