using System.Collections.Generic;

namespace efcore_postgres.Services
{
    public interface IEmployeesService
    {
        IEnumerable<Employee> GetAll();
        Employee GetById(int id);
        Employee Add(Employee employee);
        bool Remove(Employee employee);
    }
}
