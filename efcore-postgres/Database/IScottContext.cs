using Microsoft.EntityFrameworkCore;

namespace efcore_postgres.Database
{
    public interface IScottContext
    {
        DbSet<Employee> Employees { get; }
        int SaveChanges();
    }
}
