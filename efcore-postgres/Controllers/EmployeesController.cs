using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
        [Produces("application/json", Type = typeof(List<Employee>))]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        ///
        /// GET api/employees/5
        /// 
        /// <summary>
        /// Gets a single employee.
        /// </summary>
        /// 
        [HttpGet("{id}")]
        [Produces("application/json", Type = typeof(Employee))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Employee employee = await _service.GetByIdAsync(id);
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
        [Produces("application/json", Type = typeof(Employee))]
        public async Task<IActionResult> PostAsync([FromBody] Employee employee)
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
                var newEmployee = await _service.AddAsync(employee);
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
        [Produces("application/json", Type = typeof(bool))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool success = false;
            Employee employee = await _service.GetByIdAsync(id);
            if (employee != null)
            {
                success = await _service.RemoveAsync(employee);
            }

            if (!success)
            {
                return NotFound($"Could not find employee with id: {id}");
            }

            return Ok(success);
        }
    }
}
