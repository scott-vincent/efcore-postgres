using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
