using System;
using System.Collections.Generic;
using System.Linq;
using efcore_postgres.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // GET api/employees
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> Get()
        {
            return Ok(_service.GetAll());
        }

        // GET api/employees/5
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

        // POST api/employees
        [HttpPost]
        public ActionResult<Employee> Post([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(_service.Add(employee));
        }

        // DELETE api/employees/5
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
