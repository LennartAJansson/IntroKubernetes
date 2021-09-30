using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SmhiDb
{
    public class SmhiDbContextFactory : IDesignTimeDbContextFactory<SmhiDbContext>
    {
        public SmhiDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SmhiDbContext>();

            optionsBuilder.UseSqlite("Data Source=c:\\Data\\SQLite\\smhidev.db;");

            return new SmhiDbContext(optionsBuilder.Options);
        }
    }
}

