using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly MongoDbService _mongoDbService;

        public EmployeeController(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Employee>>> Get()
        {
            var employees = await _mongoDbService.GetAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> Get(string id)
        {
            var employee = await _mongoDbService.GetAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Employee employee)
        {
            await _mongoDbService.CreateAsync(employee);
            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Employee employee)
        {
            var existingEmployee = await _mongoDbService.GetAsync(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }
            employee.Id = id;
            await _mongoDbService.UpdateAsync(id, employee);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var employee = await _mongoDbService.GetAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            await _mongoDbService.RemoveAsync(id);
            return NoContent();
        }
    }
}
