using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace efcore_postgres.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ScottContext _dbcontext;
        public EmployeesController(ScottContext context)
        {
            _dbcontext = context;
        }

        // GET api/employees
        [HttpGet]
        public ActionResult<IEnumerable<Employee>> Get()
        {
            return _dbcontext.Employees;
        }

        // GET api/employees/5
        [HttpGet("{id}")]
        public ActionResult<Employee> Get(int id)
        {
            Employee employee = _dbcontext.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // POST api/employees
        [HttpPost]
        public void Post([FromBody] Employee employee)
        {
            _dbcontext.Employees.Add(employee);
            _dbcontext.SaveChanges();
        }

        // DELETE api/employees/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                Employee employee = _dbcontext.Employees.Find(id);
                if (employee == null)
                {
                    return NotFound();
                }
                _dbcontext.Employees.Remove(employee);
                _dbcontext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
