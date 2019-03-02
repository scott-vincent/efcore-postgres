using efcore_postgres;
using efcore_postgres.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace efcore_postgres_Tests.Services
{
    public class EmployeesServiceMock : IEmployeesService
    {
        private readonly List<Employee> _employees;
        private int lastId;

        public EmployeesServiceMock()
        {
            _employees = new List<Employee>()
            {
                new Employee() { Id = 1, Name = "Person 1", Birthdate = null, Salary = null },
                new Employee() { Id = 2, Name = "Person 2", Birthdate = new DateTime(1977, 4, 2), Salary = 123456.78M },
                new Employee() { Id = 3, Name = "Person 3", Birthdate = null, Salary = 35000M }
            };

            lastId = _employees[_employees.Count - 1].Id;
        }

        public Employee Add(Employee employee)
        {
            employee.Id = ++lastId;
            _employees.Add(employee);
            return employee;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _employees;
        }

        public Employee GetById(int id)
        {
            return _employees.Where(a => a.Id == id)
                        .FirstOrDefault();
        }

        public bool Remove(Employee employee)
        {
            return _employees.Remove(employee);
        }
    }
}
