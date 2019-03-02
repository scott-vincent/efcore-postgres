using efcore_postgres;
using efcore_postgres.Controllers;
using efcore_postgres.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace efcore_postgres_Tests_Moq.Controllers
{
    public class TestsBase : IDisposable
    {
        protected EmployeesController _controller;
        protected Mock<IEmployeesService> _mockService;

        protected TestsBase()
        {
            // Called before every test

            // Want to test the controller so create a mock service and a real controller
            _mockService = new Mock<IEmployeesService>();
            _controller = new EmployeesController(_mockService.Object);
        }

        public void Dispose()
        {
            // Called after every test
        }
    }

    public class EmployeesControllerTests : TestsBase
    {
        /*
        private readonly ITestOutputHelper _output;

        public EmployeesControllerTests(ITestOutputHelper output)
        {
            _output = output;
            _output.WriteLine("Running Test");
        }
        */

        [Fact]
        public void GetAll_ReturnsAllEmployees()
        {
            // Setup service mocks
            _mockService.Setup(x => x.GetAll())
                  .Returns(new List<Employee>() {
                      new Employee() { Id = 1, Name = "Person 1", Birthdate = null, Salary = null },
                      new Employee() { Id = 2, Name = "Person 2", Birthdate = new DateTime(1977, 4, 2), Salary = 123456.78M },
                      new Employee() { Id = 3, Name = "Person 3", Birthdate = null, Salary = 35000M }
                  });

            // Perform test
            var response = _controller.GetAll();

            // Check results
            Assert.NotNull(response.Value);
            var employees = Assert.IsAssignableFrom<IEnumerable<Employee>>(response.Value);

            Assert.Equal(3, employees.Count());
            Employee employee1 = employees.Where(a => a.Id == 1).FirstOrDefault();
            Assert.Equal("Person 1", employee1.Name);
        }

        [Fact]
        public void GetById_ReturnsEmployee()
        {
            // Setup service mocks
            _mockService.Setup(x => x.GetById(It.IsAny<int>()))
                  .Returns(new Employee() { Id = 3, Name = "Person 3", Birthdate = null, Salary = 35000M });

            // Perform test
            var response = _controller.GetById(3);

            // Check results
            Assert.NotNull(response.Value);
            var employee = Assert.IsType<Employee>(response.Value);

            Assert.Equal(3, employee.Id);
            Assert.Equal("Person 3", employee.Name);
        }

        [Fact]
        public void GetById_Unknown_ReturnsNotFound()
        {
            // Perform test
            var response = _controller.GetById(99);

            // Check results
            Assert.NotNull(response.Result);
            var result = Assert.IsType<NotFoundObjectResult>(response.Result);
            Assert.Equal("Could not find employee with id: 99", (string)result.Value);
        }

        [Fact]
        public void Add_MissingName_ReturnsBadRequest()
        {
            var badEmployee = new Employee()
            {
                Salary = 12345M
            };

            // Perform test
            var response = _controller.Post(badEmployee);

            // Check results
            Assert.NotNull(response.Result);
            var result = Assert.IsType<BadRequestObjectResult>(response.Result);

            // Make sure failure is due to missing name
            var errors = result.Value as SerializableError;
            Assert.True(errors.ContainsKey("Name"));
        }


        [Fact]
        public void Add_ReturnsCreatedEmployee()
        {
            // Setup test data
            var employee = new Employee() { Name = "New Person", Salary = 12345M };

            // Setup service mocks
            _mockService.Setup(x => x.Add(employee))
                  .Returns(new Employee() { Id = 9, Name = employee.Name, Birthdate = employee.Birthdate, Salary = employee.Salary });

            // Perform test
            var response = _controller.Post(employee);

            // Check results
            Assert.NotNull(response.Value);
            var newEmployee = response.Value;
            Assert.Equal("New Person", newEmployee.Name);
            Assert.Equal(12345M, newEmployee.Salary);

            // Make sure an id was allocated
            Assert.True(newEmployee.Id > 0);
        }

        [Fact]
        public void Delete_RemovesEmployee()
        {
            // Setup test data
            var employee = new Employee() { Id = 1, Name = "Delete Me" };

            // Setup service mocks
            _mockService.Setup(x => x.GetById(1))
                  .Returns(employee);

            _mockService.Setup(x => x.Remove(employee))
                  .Returns(true);

            // Delete the employee
            var result = _controller.Delete(1);

            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void Delete_Unknown_ReturnsNotFound()
        {
            // Perform test
            var response = _controller.Delete(99);

            // Check results
            Assert.NotNull(response);
            var result = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal("Could not find employee with id: 99", (string)result.Value);
        }
    }
}
