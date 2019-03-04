using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace efcore_postgres.Database
{
    public interface IScottContext
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee> GetAsync(int id);
        void AddRec(Employee employee);
        void RemoveRec(Employee employee);
        Task<int> SaveAsync();
    }
}
