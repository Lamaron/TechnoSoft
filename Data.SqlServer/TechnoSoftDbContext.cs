using Microsoft.EntityFrameworkCore;
using Domain;
using Data.Interfaces;

namespace Data.SqlServer
{
    public class TechnoSoftDbContext : DbContext
    {
        public TechnoSoftDbContext(DbContextOptions<TechnoSoftDbContext> options) : base(options)
        {
        }

        public DbSet<Request> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Request>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Number).IsRequired().HasMaxLength(50);
                entity.Property(e => e.EquipmentType).IsRequired().HasMaxLength(100);
                entity.Property(e => e.EquipmentModel).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ProblemDescription).IsRequired();
                entity.Property(e => e.ClientFullName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ClientPhone).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Engineer).HasMaxLength(100);
                entity.Property(e => e.Comments);

                entity.Property(e => e.Status)
                    .HasConversion<int>()
                    .IsRequired();
            });
        }
    }
}
