using efcore_postgres;
using efcore_postgres.Controllers;
using efcore_postgres_Tests.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace efcore_postgres_Tests
{
    public class EmployeesControllerTests
    {
        EmployeesController _controller;
        EmployeesServiceMock _service;

        public EmployeesControllerTests()
        {
            _service = new EmployeesServiceMock();
            _controller = new EmployeesController(_service);
        }

        [Fact]
        public void GetAll_ReturnsAllEmployees()
        {
            var response = _controller.GetAll();

            Assert.NotNull(response.Value);
            var employees = Assert.IsAssignableFrom<IEnumerable<Employee>>(response.Value);

            Assert.Equal(3, employees.Count());
            Employee employee1 = employees.Where(a => a.Id == 1).FirstOrDefault();
            Assert.Equal("Person 1", employee1.Name);
        }

        [Fact]
        public void GetById_ReturnsEmployee()
        {
            var response = _controller.GetById(3);

            Assert.NotNull(response.Value);
            var employee = Assert.IsType<Employee>(response.Value);

            Assert.Equal(3, employee.Id);
            Assert.Equal("Person 3", employee.Name);
        }

        [Fact]
        public void GetById_Unknown_ReturnsNotFound()
        {
            var response = _controller.GetById(99);

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

            var response = _controller.Post(badEmployee);

            Assert.NotNull(response.Result);
            var result = Assert.IsType<BadRequestObjectResult>(response.Result);

            // Make sure failure is due to missing name
            var errors = result.Value as SerializableError;
            Assert.True(errors.ContainsKey("Name"));
        }


        [Fact]
        public void Add_ReturnsCreatedEmployee()
        {
            var employee = new Employee()
            {
                Name = "New Person",
                Salary = 12345M
            };

            var response = _controller.Post(employee);

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
            // Add a new employee
            var employee = new Employee() { Name = "Delete Me" };
            var response = _controller.Post(employee);
            Assert.NotNull(response.Value);
            var id = response.Value.Id;

            // Delete the employee
            var result = _controller.Delete(id);

            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);

            // Make sure employee has been deleted
            response = _controller.GetById(id);
            Assert.NotNull(response.Result);
            Assert.IsType<NotFoundObjectResult>(response.Result);
        }

        [Fact]
        public void Delete_Unknown_ReturnsNotFound()
        {
            var response = _controller.Delete(99);

            Assert.NotNull(response);
            var result = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal("Could not find employee with id: 99", (string)result.Value);
        }
    }
}
