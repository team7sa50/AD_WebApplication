﻿using Microsoft.VisualBasic;
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
        public Employee findEmployeeByEmail(string email)
        {
            return dbcontext.employees.Where(x => x.Email == email).FirstOrDefault();
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
            newAuthorize.EmployeeName = employee.Name;
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

            if (employeeAuthorize != null && employeeAuthorize.EmployeeId != userId)
            {
                return findEmployeeById(employeeAuthorize.EmployeeId);
            }
            return dbcontext.employees.Where(x => x.DepartmentsId == department.Id && x.Role == Role.DEPT_HEAD).FirstOrDefault();

        }

        public Employee findDeptRepresentative(string deptId) {
            return dbcontext.employees.Where(x => x.DepartmentsId == deptId && x.Role == Role.DEPT_REP).FirstOrDefault();
        }

        public List<CollectionPoint> findAllCollectionPts()
        {
            List<CollectionPoint> collectionPoint = (from cp in dbcontext.collectionPoints
                                                     select cp).ToList();
            return collectionPoint;
        }
        public bool IsAuthorizer(string userId) {
            Departments department = findDepartmentByEmployee(userId);
            //To extract authorization that is from the dept and have overlapping dates with Today
            EmployeeAuthorize employeeAuthorize = dbcontext.employeeAuthorizes
                                                .Where(x => DateTime.Now >= x.startDate
                                                        && DateTime.Now <= x.endDate
                                                        && x.DepartmentsId == department.Id).FirstOrDefault();

            if (employeeAuthorize != null && employeeAuthorize.EmployeeId == userId)
            {
                return true;
            }
            //If there is no Authorize Employee on the day and login user is dept head
            else if (employeeAuthorize == null && department.Employees.Where(x=>x.Role == Role.DEPT_HEAD).FirstOrDefault().Id == userId){
                return true;
            }
            else return false;
        }
        public List<AnalysisModel> startDepartmentAnalysis(string userId)
        {
            Departments department = findDepartmentByEmployee(userId);

            var past4Month = DateTime.Now.AddMonths(-1).Month;
            var Year = DateTime.Now.Year;
            var po = from req in dbcontext.requisitions
                     join req_d in dbcontext.requisitionDetails on req.Id equals req_d.RequisitionId
                     group req_d by new {  req.Department.DeptName,req_d.Inventory.ItemCategory.name, req.DateSubmitted.Month, req.DateSubmitted.Year } into h
                     where (h.Key.Month >= past4Month && h.Key.Year == Year && h.Key.DeptName == department.DeptName )
                     select new
                     {
                         Month = h.Key.Month,
                         Qty = h.Sum(x => x.RequestedQty),
                         Category=h.Key.name
                     };
            List<AnalysisModel> aQ = new List<AnalysisModel>();
            foreach (var c in po)
            {
                AnalysisModel a = new AnalysisModel();
                a.quantity = c.Qty;
                a.Category = c.Category;
                aQ.Add(a);
            }
            return aQ;
        }


    }
}
