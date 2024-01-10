using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications.Employee_Specs;

namespace Talabat.API.Controllers
{
    //Draft(For trying open close design pattern)

    public class EmployeeController : BaseApiController
    {
        private readonly IGenericRepository<Employee> employeeRepo;

        public EmployeeController(IGenericRepository<Employee> EmployeeRepo)
        {
            employeeRepo = EmployeeRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Employee>>> GetEmployees()
            {
            
                var spec = new EmployeeWithDepartmentSpecifications();
            
                var employees = employeeRepo.GetAllWithSpecAsync(spec);
            
                return Ok(employees);
            }
        [HttpGet("{id}")]
        public async Task<ActionResult<IReadOnlyList<Employee>>> GetEmployee(int id)
        {

            var spec = new EmployeeWithDepartmentSpecifications(id);

            var employee = employeeRepo.GetEntityWithSpecAsync(spec);

            return Ok(employee);
        }
    }
}
