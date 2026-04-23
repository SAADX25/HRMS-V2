using Application.DTOs.Employee;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService EmployeeService;

    public EmployeeController(IEmployeeService EmployeeService)
    {
        this.EmployeeService = EmployeeService;
    }

    // GET api/employee
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await EmployeeService.GetAllAsync();
        return Ok(employees);
    }

    // GET api/employee/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await EmployeeService.GetByIdAsync(id);
        if (employee is null)
        {
            return NotFound(); 
        }


        return Ok(employee);
    }

    // GET api/employee/department/3
    [HttpGet("department/{departmentId}")]
    public async Task<IActionResult> GetByDepartment(int departmentId)
    {
        var employees = await  EmployeeService.GetByDepartmentAsync(departmentId);

        return Ok(employees);
    }

    // POST api/employee
    [HttpPost]
    public async Task<IActionResult> Create(CreateEmployeeDto dto)
    {

        var employee = await EmployeeService.CreateAsync(dto);


        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    // PUT api/employee/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateEmployeeDto dto)
    {
        var employee = await EmployeeService.UpdateAsync(id, dto);
        if (employee is null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    // DELETE api/employee/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await EmployeeService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}



//repository pattern 
// Generic repository => CRUD operations for all entities  => Clean Artitecture 
// Auto Mapper => DTOs => Data Transfer Objects => Avoid exposing internal data structures => Security and flexibility


