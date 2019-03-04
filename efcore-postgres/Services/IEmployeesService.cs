using System.Collections.Generic;
using System.Threading.Tasks;

namespace efcore_postgres.Services
{
    public interface IEmployeesService
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int id);
        Task<Employee> AddAsync(Employee employee);
        Task<bool> RemoveAsync(Employee employee);
    }
}
