using efcore_postgres.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace efcore_postgres.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IScottContext _dbcontext;
        public EmployeesService(IScottContext context)
        {
            _dbcontext = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _dbcontext.GetAllAsync();
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _dbcontext.GetAsync(id);
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            _dbcontext.AddRec(employee);
            await _dbcontext.SaveAsync();
            return employee;
        }

        public async Task<bool> RemoveAsync(Employee employee)
        {
            try
            {
                _dbcontext.RemoveRec(employee);
                await _dbcontext.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

            return true;
        }
    }
}
