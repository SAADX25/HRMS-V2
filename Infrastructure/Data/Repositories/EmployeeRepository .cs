using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data.Repositories
{
   public  class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
       
     
        private readonly AppDbContext context;
        

        public EmployeeRepository(AppDbContext context) : base(context) 
        {
           
            this.context = context;
        }

        
         public async Task<IEnumerable<Employee>> GetByDepartmentAsync(int departmentId)
        {
            return await context.Employees.Where(e => e.DepartmentId == departmentId).ToListAsync();
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await context.Employees.AnyAsync(e => e.Email == email);
        }




    }
}
