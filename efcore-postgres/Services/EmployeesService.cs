using efcore_postgres.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace efcore_postgres.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IScottContext _dbcontext;
        public EmployeesService(IScottContext context)
        {
            _dbcontext = context;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _dbcontext.Employees;
        }

        public Employee GetById(int id)
        {
            return _dbcontext.Employees.Find(id);
        }

        public Employee Add(Employee employee)
        {
            _dbcontext.Employees.Add(employee);
            _dbcontext.SaveChanges();
            return employee;
        }

        public bool Remove(Employee employee)
        {
            try
            {
                _dbcontext.Employees.Remove(employee);
                _dbcontext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
    }
}
