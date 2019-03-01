using System.Collections.Generic;
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
        public ActionResult<IEnumerable<Employee>> Get()
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
        public ActionResult<Employee> Get(int id)
        {
            Employee employee = _service.GetById(id);
            if (employee == null)
            {
                return NotFound();
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_service.Add(employee));
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
                return NotFound();
            }

            return Ok();
        }
    }
}
