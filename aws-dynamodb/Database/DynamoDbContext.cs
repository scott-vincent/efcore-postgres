using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aws_dynamodb.Database
{
    public class DynamoDbContext : IDbContext
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly DynamoDBContext _dynamoContext;

        public DynamoDbContext(IAmazonDynamoDB dynamoClient)
        {
            // Provides direct access to the database
            _dynamoDb = dynamoClient;

            // Provides access to the database via the Object Persistence Model
            _dynamoContext = new DynamoDBContext(_dynamoDb, new DynamoDBContextConfig { ConsistentRead = true, SkipVersionCheck = true });
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _dynamoContext.ScanAsync<Employee>(conditions).GetRemainingAsync();
        }

        public async Task<Employee> GetByIdAsync(string id)
        {
            return await _dynamoContext.LoadAsync<Employee>(id);
        }

        public async Task<Employee> AddAsync(Employee employee)
        {
            // Some databases will generate an id for you so leave this in the DB layer
            if (String.IsNullOrEmpty(employee.Id))
            {
                // Generate a new id
                employee.Id = Guid.NewGuid().ToString();
            }
            await _dynamoContext.SaveAsync<Employee>(employee);
            return employee;
        }

        public async Task<bool> RemoveAsync(Employee employee)
        {
            try
            {
                await _dynamoContext.DeleteAsync<Employee>(employee);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task CreateTableAsync()
        {
            var tableDef = new CreateTableRequest
            {
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "S"
                    }
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH" // Partition Key
                    }
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5
                },
                TableName = "Employees"
            };

            try
            {
                await _dynamoDb.CreateTableAsync(tableDef);
            }
            catch (ResourceInUseException)
            {
                // Not an error: table already exists
                return;
            }
        }
    }
}
