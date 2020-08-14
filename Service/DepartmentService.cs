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

        public Employee findEmployeeById(string userId)
        {
            return dbcontext.employees.Where(x => x.Id == userId).FirstOrDefault();
        }

        public List<Employee> findDepartmentEmployeeList(string userId)
        {
            Employee emp = findEmployeeById(userId);
            return dbcontext.employees.Where(x => (x.Role == Role.EMPLOYEE || x.Role == Role.DEPT_REP)
                                        && x.DepartmentsId == emp.DepartmentsId).ToList();

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


    }
}
