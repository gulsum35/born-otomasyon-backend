using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BornOtomasyonApi.Models;

namespace BornOtomasyonApi.Data
{
    public class BornOtomasyonContext : IdentityDbContext<ApplicationUser>
    {
        public BornOtomasyonContext(DbContextOptions<BornOtomasyonContext> options)
            : base(options)
        {
        }

        // FormData tablosu
        public DbSet<FormData> FormDatas { get; set; } = null!; // null-forgiving: EF runtime tarafından set edilecek

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // FormData entity konfigürasyonu
            modelBuilder.Entity<FormData>(entity =>
            {
                entity.ToTable("FormDatas"); // Tablo adı explicit belirtildi

                entity.HasKey(e => e.Id); 
                entity.Property(e => e.Text1)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.Num1)
                      .IsRequired();

                entity.Property(e => e.Date1)
                      .IsRequired();
            });

        }
    }
}
