using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagments.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employee;

        public MockEmployeeRepository()
        {
            _employee = new List<Employee>()
            {
                new Employee() {Id = 1, Name = "Ahmed", Department = "IT", Email = "agmghazi@hotmail.com"},
                new Employee() {Id = 2, Name = "ayman", Department = "IT", Email = "agmghazi2@hotmail.com"},
                new Employee() {Id = 3, Name = "eslam", Department = "HR", Email = "agmghazi3@hotmail.com"},
            };
        }
        public Employee GetEmployee(int id)
        {
            return this._employee.FirstOrDefault(x => x.Id == id);
        }
    }
}
