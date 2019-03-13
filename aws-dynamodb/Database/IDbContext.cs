using System.Collections.Generic;
using System.Threading.Tasks;

namespace aws_dynamodb.Database
{
    public interface IDbContext
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(string id);
        Task<Employee> AddAsync(Employee employee);
        Task<bool> RemoveAsync(Employee employee);
    }
}