using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using BornOtomasyonApi.Data;


 

namespace BornOtomasyonApi.Data
{
    public class BornOtomasyonContextFactory : IDesignTimeDbContextFactory<BornOtomasyonContext>
    {
        public BornOtomasyonContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BornOtomasyonContext>();
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=BornOtomasyonDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;");

            return new BornOtomasyonContext(optionsBuilder.Options);
        }
    }
}

