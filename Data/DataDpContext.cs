using DirectoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DirectoryApi.Data
{
    public class DataDpContext : DbContext
    {
        public DataDpContext(DbContextOptions<DataDpContext> options): base(options) { }
    public DbSet<Directory> Directory => Set<Directory>();
        public DbSet<User> Users => Set<User>();

         
    }
}
