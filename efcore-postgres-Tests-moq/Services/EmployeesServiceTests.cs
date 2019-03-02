using efcore_postgres;
using efcore_postgres.Database;
using efcore_postgres.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using Xunit;
using Xunit.Abstractions;

namespace efcore_postgres_Tests_Moq.Services
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
        public void GetAll_CallsDBGet()
        {
            // Setup service mocks
            var mockEmployees = new Mock<DbSet<Employee>>();
            _mockDatabase.Setup(db => db.Employees).Returns(mockEmployees.Object);

            // Perform test
            var response = _service.GetAll();

            // Make sure get was called
            _mockDatabase.Verify(db => db.Employees, Times.Once);
        }

        [Fact]
        public void GetById_CallsDBFind()
        {
            // Setup service mocks
            var mockEmployees = new Mock<DbSet<Employee>>();
            _mockDatabase.Setup(db => db.Employees).Returns(mockEmployees.Object);

            // Perform test
            var response = _service.GetById(1);

            // Make sure find was called
            mockEmployees.Verify(emp => emp.Find(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public void Add_CallsDBAddAndSave()
        {
            // Setup service mocks
            var mockEmployees = new Mock<DbSet<Employee>>();
            _mockDatabase.Setup(db => db.Employees).Returns(mockEmployees.Object);

            // Perform test
            var response = _service.Add(new Employee());

            // Make sure add and save were called
            mockEmployees.Verify(emp => emp.Add(It.IsAny<Employee>()), Times.Once());
            _mockDatabase.Verify(db => db.SaveChanges(), Times.Once());
        }

        [Fact]
        public void Remove_CallsDBRemoveAndSave()
        {
            // Setup service mocks
            var mockEmployees = new Mock<DbSet<Employee>>();
            _mockDatabase.Setup(db => db.Employees).Returns(mockEmployees.Object);

            // Perform test
            var response = _service.Remove(new Employee());

            // Make sure remove and save were called
            mockEmployees.Verify(emp => emp.Remove(It.IsAny<Employee>()), Times.Once());
            _mockDatabase.Verify(db => db.SaveChanges(), Times.Once());
        }
    }
}

