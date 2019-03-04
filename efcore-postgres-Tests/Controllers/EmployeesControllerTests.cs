using efcore_postgres;
using efcore_postgres.Controllers;
using efcore_postgres.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace efcore_postgres_Tests.Controllers
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
            // Setup test data
            List<Employee> employees = new List<Employee>() {
                new Employee() { Id = 1, Name = "Person 1", Birthdate = null, Salary = null },
                new Employee() { Id = 2, Name = "Person 2", Birthdate = new DateTime(1977, 4, 2), Salary = 123456.78M },
                new Employee() { Id = 3, Name = "Person 3", Birthdate = null, Salary = 35000M }
            };

            // Setup service mocks
            _mockService.Setup(x => x.GetAllAsync()).ReturnsAsync(employees);

            // Perform test
            var response = _controller.GetAllAsync();

            // Check for OK response
            Assert.NotNull(response.Result);
            var result = Assert.IsType<OkObjectResult>(response.Result);

            // Make sure get all was called
            _mockService.Verify(x => x.GetAllAsync(), Times.Once);

            // Check returned data
            var gotEmployees = Assert.IsAssignableFrom<List<Employee>>(result.Value);
            Assert.Equal(3, gotEmployees.Count());
            Employee employee1 = gotEmployees.Where(a => a.Id == 1).FirstOrDefault();
            Assert.Equal("Person 1", employee1.Name);
        }

        [Fact]
        public void GetById_ReturnsEmployee()
        {
            // Setup test data
            Employee employee = new Employee() { Id = 3, Name = "Person 3", Birthdate = null, Salary = 35000M };

            // Setup service mocks
            _mockService.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(employee);

            // Perform test
            var response = _controller.GetByIdAsync(3);

            // Check for OK response
            Assert.NotNull(response.Result);
            var result = Assert.IsType<OkObjectResult>(response.Result);

            // Make sure get was called
            _mockService.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);

            // Check returned data
            var gotEmployee = Assert.IsType<Employee>(result.Value);
            Assert.Equal(3, gotEmployee.Id);
            Assert.Equal("Person 3", gotEmployee.Name);
        }

        [Fact]
        public void GetById_Unknown_ReturnsNotFound()
        {
            // Setup service mocks
            _mockService.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                  .ReturnsAsync(null as Employee);

            // Perform test
            var response = _controller.GetByIdAsync(99);

            // Check for Not Found response
            Assert.NotNull(response.Result);
            var result = Assert.IsType<NotFoundObjectResult>(response.Result);
            Assert.Equal("Could not find employee with id: 99", (string)result.Value);

            // Make sure get was called
            _mockService.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Add_MissingName_ReturnsBadRequest()
        {
            // Setup test data
            var badEmployee = new Employee()
            {
                Salary = 12345M
            };

            // Setup service mocks
            _mockService.Setup(x => x.AddAsync(It.IsAny<Employee>()))
                  .ReturnsAsync(null as Employee);

            // Perform test
            var response = _controller.PostAsync(badEmployee);

            // Check for Bad Request response
            Assert.NotNull(response.Result);
            var result = Assert.IsType<BadRequestObjectResult>(response.Result);

            // Make sure failure is due to missing name
            var errors = result.Value as SerializableError;
            Assert.True(errors.ContainsKey("Name"));

            // Make sure add was not called
            _mockService.Verify(x => x.AddAsync(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public void Add_ReturnsCreatedEmployee()
        {
            // Setup test data
            var employee = new Employee() { Name = "New Person", Salary = 12345M };

            // Setup service mocks
            _mockService.Setup(x => x.AddAsync(employee))
                  .ReturnsAsync(new Employee() { Id = 9, Name = employee.Name, Birthdate = employee.Birthdate, Salary = employee.Salary });

            // Perform test
            var response = _controller.PostAsync(employee);

            // Check for OK response
            Assert.NotNull(response.Result);
            var result = Assert.IsType<OkObjectResult>(response.Result);

            // Make sure add was called
            _mockService.Verify(x => x.AddAsync(It.IsAny<Employee>()), Times.Once);

            // Check returned data
            var newEmployee = result.Value as Employee;
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
            _mockService.Setup(x => x.GetByIdAsync(1))
                  .ReturnsAsync(employee);
            _mockService.Setup(x => x.RemoveAsync(employee))
                  .ReturnsAsync(true);

            // Delete the employee
            var response = _controller.DeleteAsync(1);

            // Check for OK response
            Assert.NotNull(response.Result);
            Assert.IsType<OkObjectResult>(response.Result);

            // Make sure get and remove were both called
            _mockService.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _mockService.Verify(x => x.RemoveAsync(It.IsAny<Employee>()), Times.Once);
        }

        [Fact]
        public void Delete_Unknown_ReturnsNotFound()
        {
            _mockService.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                  .ReturnsAsync(null as Employee);
            _mockService.Setup(x => x.RemoveAsync(It.IsAny<Employee>()))
                  .ReturnsAsync(false);

            // Perform test
            var response = _controller.DeleteAsync(99);

            // Check for Not Found response
            Assert.NotNull(response.Result);
            var result = Assert.IsType<NotFoundObjectResult>(response.Result);
            Assert.Equal("Could not find employee with id: 99", (string)result.Value);

            // Make sure get was called but remove was not called
            _mockService.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once);
            _mockService.Verify(x => x.RemoveAsync(It.IsAny<Employee>()), Times.Never);
        }
    }
}
