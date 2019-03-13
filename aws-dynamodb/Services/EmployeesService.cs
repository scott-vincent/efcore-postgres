using Amazon.DynamoDBv2;
using aws_dynamodb.Database;
using aws_dynamodb.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace aws_dynamodb.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IDbContext _dbContext;
        public EmployeesService(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _dbContext.GetAllAsync();
        }

        public async Task<Employee> GetByIdAsync(string id)
        {
            return await _dbContext.GetByIdAsync(id);
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            return await _dbContext.AddAsync(employee);
        }

        public async Task<bool> RemoveAsync(Employee employee)
        {
            return await _dbContext.RemoveAsync(employee);
        }
    }
}
