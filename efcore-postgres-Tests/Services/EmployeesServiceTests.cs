using efcore_postgres;
using efcore_postgres.Database;
using efcore_postgres.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace efcore_postgres_Tests.Services
{
    public class TestsBase : IDisposable
    {
        protected EmployeesService _service;
        protected Mock<IScottContext> _mockDatabase;

        protected TestsBase()
        {
            // Called before every test

            // Want to test the service so create a mock database and a real service
            _mockDatabase = new Mock<IScottContext>();
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
            var response = await _service.GetByIdAsync(1);

            // Make sure get was called
            _mockDatabase.Verify(x => x.GetAsync(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task Add_CallsDBAddAndSave()
        {
            // Perform test
            var response = await _service.AddAsync(new Employee());

            // Make sure add and save were called
            _mockDatabase.Verify(x => x.AddRec(It.IsAny<Employee>()), Times.Once());
            _mockDatabase.Verify(x => x.SaveAsync(), Times.Once());
        }

        [Fact]
        public async Task Remove_CallsDBRemoveAndSave()
        {
            // Perform test
            var response = await _service.RemoveAsync(new Employee());

            // Make sure remove and save were called
            _mockDatabase.Verify(x => x.RemoveRec(It.IsAny<Employee>()), Times.Once());
            _mockDatabase.Verify(x => x.SaveAsync(), Times.Once());
        }
    }
}

