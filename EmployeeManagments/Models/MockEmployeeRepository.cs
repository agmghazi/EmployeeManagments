using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagments.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employee;

        public MockEmployeeRepository()
        {
            _employee = new List<Employee>()
            {
                new Employee() {Id = 1, Name = "Ahmed", Department =Dept.HR, Email = "agmghazi@hotmail.com"},
                new Employee() {Id = 2, Name = "ayman", Department = Dept.IT, Email = "agmghazi2@hotmail.com"},
                new Employee() {Id = 3, Name = "eslam", Department = Dept.HR, Email = "agmghazi3@hotmail.com"},
            };
        }
        public Employee GetEmployee(int id)
        {
            return this._employee.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employee;
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employee.Max(x => x.Id) + 1;
            _employee.Add(employee);
            return employee;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employee.FirstOrDefault(x => x.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }

            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employee.FirstOrDefault(x => x.Id == id);
            if (employee != null)
            {
                _employee.Remove(employee);
            }

            return employee;
        }
    }
}
