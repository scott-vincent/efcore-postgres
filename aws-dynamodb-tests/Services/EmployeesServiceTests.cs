using aws_dynamodb.Database;
using aws_dynamodb.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace aws_dynamodb_tests.Services
{
    public class TestsBase : IDisposable
    {
        protected EmployeesService _service;
        protected Mock<IDbContext> _mockDatabase;

        protected TestsBase()
        {
            // Called before every test

            // Want to test the service so create a mock database and a real service
            _mockDatabase = new Mock<IDbContext>();
            _service = new EmployeesService(_mockDatabase.Object);
        }

        public void Dispose()
        {
            // Called after every test
        }
    }

    public class EmployeesServiceTests : TestsBase
    {
        /*
        private readonly ITestOutputHelper _output;

        public EmployeesServiceTests(ITestOutputHelper output)
        {
            _output = output;
            _output.WriteLine("Running Test");
        }
        */

        [Fact]
        public async Task GetAll_CallsDBGet()
        {
            // Setup service mocks
            var mockEmployees = new Mock<List<Employee>>();
            _mockDatabase.Setup(db => db.GetAllAsync())
                .ReturnsAsync(mockEmployees.Object);

            // Perform test
            var response = await _service.GetAllAsync();

            // Make sure get all was called
            _mockDatabase.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetById_CallsDBFind()
        {
            // Perform test
            var response = await _service.GetByIdAsync("1");

            // Make sure get was called
            _mockDatabase.Verify(x => x.GetByIdAsync(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Add_CallsDBAdd()
        {
            // Perform test
            var response = await _service.AddAsync(new Employee());

            // Make sure add was called
            _mockDatabase.Verify(x => x.AddAsync(It.IsAny<Employee>()), Times.Once());
        }

        [Fact]
        public async Task Remove_CallsDBRemove()
        {
            // Perform test
            var response = await _service.RemoveAsync(new Employee());

            // Make sure remove was called
            _mockDatabase.Verify(x => x.RemoveAsync(It.IsAny<Employee>()), Times.Once());
        }
    }
}

