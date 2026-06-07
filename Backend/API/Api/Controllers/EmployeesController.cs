using Microsoft.AspNetCore.Mvc;
using Api.DTOs;
using Api.Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        // GET: api/employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        // POST: api/employees
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> PostEmployee(CreateEmployeeDto employeeDto)
        {
            var createdEmployee = await _employeeService.CreateEmployeeAsync(employeeDto);
            return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.Id }, createdEmployee);
        }

        // PUT: api/employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, UpdateEmployeeDto employeeDto)
        {
            if (id != employeeDto.Id)
            {
                return BadRequest();
            }

            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employeeDto);
            if (updatedEmployee == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        // GET: api/employees/compute-pay
        [HttpGet("compute-pay")]
        public async Task<ActionResult<decimal>> ComputePay([FromQuery] int employeeId)
        {
            try
            {
                var pay = await _employeeService.ComputePayAsync(employeeId);
                return Ok(pay);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}