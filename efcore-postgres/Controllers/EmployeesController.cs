using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using efcore_postgres.Services;
using Microsoft.AspNetCore.Mvc;

namespace efcore_postgres.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _service;

        public EmployeesController(IEmployeesService service)
        {
            _service = service;
        }

        /// 
        /// GET api/employees
        /// 
        /// <summary>
        /// Gets all employees.
        /// </summary>
        /// <remarks>
        /// This remark appears in the Swagger documentation.
        /// </remarks>
        /// 
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        ///
        /// GET api/employees/5
        /// 
        /// <summary>
        /// Gets a single employee.
        /// </summary>
        /// 
        [HttpGet("{id}")]
        public ActionResult<Employee> GetById(int id)
        {
            Employee employee = _service.GetById(id);
            if (employee == null)
            {
                return NotFound($"Could not find employee with id: {id}");
            }

            return Ok(employee);
        }

        /// 
        /// POST api/employees
        /// 
        /// <summary>
        /// Adds a new employee.
        /// </summary>
        /// 
        [HttpPost]
        public ActionResult<Employee> Post([FromBody] Employee employee)
        {
            // Add our custom validation here.
            // Note: null checks can be done with a Required annotation on the model.
            if (employee.Name == null)
            {
                ModelState.AddModelError("Name", "The Name field is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var newEmployee = _service.Add(employee);
                return Ok(newEmployee);
            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        /// 
        /// DELETE api/employees/5
        /// 
        /// <summary>
        /// Deletes an employee.
        /// </summary>
        /// 
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Employee employee = _service.GetById(id);
            if (employee == null || !_service.Remove(employee))
            {
                return NotFound($"Could not find employee with id: {id}");
            }

            return Ok();
        }
    }
}
