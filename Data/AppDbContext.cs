using Microsoft.EntityFrameworkCore;
using Modules.Category;
using Modules.News;
using Modules.User;
using Utils;

namespace Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<NewsModel> News => Set<NewsModel>();
        public DbSet<UserModel> Users => Set<UserModel>();
        public DbSet<CategoryModel> Categories => Set<CategoryModel>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType
                    .ClrType.GetProperties()
                    .Where(p => Attribute.IsDefined(p, typeof(UniqueAttribute)));

                foreach (var property in properties)
                {
                    modelBuilder.Entity(entityType.Name).HasIndex(property.Name).IsUnique();
                }
            }

            modelBuilder
                .Entity<NewsModel>()
                .HasOne(n => n.Author)
                .WithMany(u => u.News)
                .HasForeignKey(n => n.AuthorId)
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }
    }
}
