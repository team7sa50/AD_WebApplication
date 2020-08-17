using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;

namespace Team7_StationeryStore.Service
{
    public class DepartmentService
    {
        protected StationeryContext dbcontext;

        public DepartmentService(StationeryContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public List<Departments> findAllDepartments()
        {
            return dbcontext.departments.ToList();
        }

        public Employee findEmployeeById(string userId)
        {
            return dbcontext.employees.Where(x => x.Id == userId).FirstOrDefault();
        }

        public List<Employee> findDepartmentEmployeeList(string userId)
        {
            Employee emp = findEmployeeById(userId);
            return dbcontext.employees.Where(x => (x.Role != Role.DEPT_HEAD)
                                        && x.DepartmentsId == emp.DepartmentsId).ToList();

        }
        public Departments findDepartmentByEmployee(string userId)
        {
            Employee emp = dbcontext.employees.Where(x => x.Id == userId).FirstOrDefault();
            return emp.Departments;
        }
        public Employee findEmployeeByName(string name)
        {
            return dbcontext.employees.Where(x => x.Name == name).FirstOrDefault();
        }

        public bool IsinvalidDate(string SD, string ED)
        {
            DateTime startDate = DateTime.Parse(SD);
            DateTime endDate = DateTime.Parse(ED);
            if (startDate < DateTime.Now || startDate > endDate)
            {
                return false;
            }
            return true;
        }

        public void createEmployeeAuthorization(string employeeName, string SD, string ED)
        {
            DateTime startDate = DateTime.Parse(SD);
            DateTime endDate = DateTime.Parse(ED);
            Employee employee = dbcontext.employees.Where(x => x.Name == employeeName).FirstOrDefault();
            //To extract authorization that is from the dept and have overlapping dates
            List<EmployeeAuthorize> existEmpAuthorize = dbcontext.employeeAuthorizes.Where(x => x.DepartmentsId == employee.DepartmentsId && x.startDate <= endDate && x.endDate >= startDate).ToList();
            if (existEmpAuthorize.Count > 0) dbcontext.employeeAuthorizes.RemoveRange(existEmpAuthorize); //Remove any authorization that have overlaps in dates
            EmployeeAuthorize newAuthorize = new EmployeeAuthorize();
            newAuthorize.Id = Guid.NewGuid().ToString();
            newAuthorize.EmployeeId = employee.Id;
            newAuthorize.startDate = startDate;
            newAuthorize.endDate = endDate;
            newAuthorize.DepartmentsId = employee.DepartmentsId;
            dbcontext.employeeAuthorizes.Add(newAuthorize);
            dbcontext.SaveChanges();
        }

        public Employee setApprover(string userId)
        {
            Departments department = findDepartmentByEmployee(userId);
            //To extract authorization that is from the dept and have overlapping dates with Today
            EmployeeAuthorize employeeAuthorize = dbcontext.employeeAuthorizes
                                                .Where(x => DateTime.Now >= x.startDate
                                                        && DateTime.Now <= x.endDate
                                                        && x.DepartmentsId == department.Id).FirstOrDefault();
            if (employeeAuthorize != null)
            {
                return findEmployeeById(employeeAuthorize.EmployeeId);
            }
            return dbcontext.employees.Where(x => x.DepartmentsId == department.Id && x.Role == Role.DEPT_HEAD).FirstOrDefault();

        }
    }
}
