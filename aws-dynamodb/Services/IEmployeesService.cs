using aws_dynamodb.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace aws_dynamodb.Services
{
    public interface IEmployeesService
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(string id);
        Task<Employee> AddAsync(Employee employee);
        Task<bool> RemoveAsync(Employee employee);
    }
}
