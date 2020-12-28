using Microsoft.EntityFrameworkCore;

namespace EmployeeManagments.Models
{
    public static class ModelBuilderExtentions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Mary",
                    Department = Dept.IT,
                    Email = "mary@gmail.com"
                },
                new Employee
                {
                    Id = 2,
                    Name = "ali",
                    Department = Dept.HR,
                    Email = "ali@gmail.com"
                }
                );
        }
    }
}
