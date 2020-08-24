using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;
using Team7_StationeryStore.Models.Requisitions;

namespace Team7_StationeryStore.Service
{
    public class RequisitionService
    {
        protected StationeryContext dbcontext;
        protected DepartmentService deptService;
        protected NotificationService notificationService;

        public RequisitionService(StationeryContext dbcontext, DepartmentService deptService, NotificationService notificationService)
        {
            this.dbcontext = dbcontext;
            this.deptService = deptService;
            this.notificationService = notificationService;
        }
        public List<Requisition> findAllRequisitionsFromStationery()
        {
            return dbcontext.requisitions.Where(x => !x.status.Equals(ReqStatus.AWAITING_APPROVAL) && !x.status.Equals(ReqStatus.REJECTED)).ToList();

        }

        public List<Requisition> findLatestRequisitions()
        {
            List<Requisition> rq = (from r in dbcontext.requisitions
                                    select r).ToList();
            List<Requisition> result = new List<Requisition>();
            for (int i = 0; i < 5; i++)
            {
                result.Add(rq[i]);
            }
            return result;
        }
        public List<Requisition> retrieveAllRequisitions()
        {
            return dbcontext.requisitions.ToList();
        }
        public List<Requisition> findAllRequisitionsFromFilter(string departmentId)
        {
            if(departmentId == "all")
            {
                return dbcontext.requisitions.ToList();
            }
            return dbcontext.requisitions.Where(x => x.DepartmentId == departmentId).ToList();
        }
        public List<Requisition> findOustandingRequisitions()
        {
            return dbcontext.requisitions.Where(x => x.status.Equals(ReqStatus.OUTSTAND)).ToList();

        }

        public List<Requisition> findRequisitionsByDept(string departmentId,ReqStatus? status) {
            if (status == null) {

                return dbcontext.requisitions.Where(x => x.DepartmentId == departmentId).ToList();
            }
            else
            {
                return dbcontext.requisitions.Where(x => x.DepartmentId == departmentId && x.status == status).ToList();
            }  
        }

        public List<Requisition> retrieveRequisitionList(Employee employee) { 
            
            return dbcontext.requisitions.Where(x => x.DepartmentId == employee.DepartmentsId).ToList();
        }
        public List<Requisition> retrieveRequisitionByEmployee(Employee e)
        {
            return dbcontext.requisitions.Where(x => x.EmployeeId == e.Id).ToList();

        }

        public List<RequisitionDetail> retrieveRequisitionDetailList(string requisitionId)
        {

            return dbcontext.requisitionDetails.Where(x => x.RequisitionId == requisitionId).ToList();
        }

        public Requisition findRequisition(string requisitionId) { 
            return dbcontext.requisitions.Where(x => x.Id == requisitionId).FirstOrDefault();
        }

        public void updateRequisition(string? userId,string requisitionId, ReqStatus? status, string? remarks) {
            Requisition requisition = findRequisition(requisitionId);
            // When authorization period expired, the default approver will be dept head.
            requisition.ApprovedEmployeeId = userId;
            requisition.Remarks = remarks;
            requisition.status = (ReqStatus)status;
            dbcontext.Update(requisition);
            dbcontext.SaveChanges();
        }
        public List<RequisitionDetailView> findRequisitionDetail(string requisitionId)
        {
            var items = (from c in dbcontext.requisitionDetails
                         where c.RequisitionId == requisitionId 
                         select new
                         {
                             ItemCode = c.Inventory.itemCode,
                             qty= c.RequestedQty
                         }
                      );

            List<RequisitionDetailView> data = new List<RequisitionDetailView>();
            foreach (var c in items)
            {
                RequisitionDetailView r = new RequisitionDetailView();
                r.itemCode = c.ItemCode;
                r.RequestedQty = c.qty;
                data.Add(r);

            }
            return data;
        }
        public List<EmployeeCart> retrieveEmployeeCart(string userId) { 
            return dbcontext.employeeCarts.Where(x => x.EmployeeId == userId).ToList();
        }
        public void CreateRequisition(string userId)
        {
            List<EmployeeCart> cartList = dbcontext.employeeCarts.Where(x => x.EmployeeId == userId).ToList();
            Employee emp = deptService.findEmployeeById(userId);
            Requisition newRequisition = new Requisition(emp.Departments.DeptCode);
            newRequisition.status = ReqStatus.AWAITING_APPROVAL;
            Employee approver = deptService.setApprover(userId);
            newRequisition.ApprovedEmployee = approver;
            newRequisition.ApprovedEmployeeId = approver.Id;
            newRequisition.Employee = emp;
            newRequisition.EmployeeId = emp.Id;
            newRequisition.DepartmentId = emp.Departments.Id;
            foreach (var i in cartList)
            {
                Inventory inv = dbcontext.inventories.Where(x => x.Id == i.Id).FirstOrDefault();
                RequisitionDetail requisitionDetail = new RequisitionDetail();
                requisitionDetail.Id= Guid.NewGuid().ToString();
                requisitionDetail.RequisitionId = newRequisition.Id;
                requisitionDetail.Inventory = i.Inventory;
                requisitionDetail.RequestedQty = i.Qty;
                dbcontext.Add(requisitionDetail);
            }
            dbcontext.Add(newRequisition);
            dbcontext.employeeCarts.RemoveRange(cartList);
            dbcontext.SaveChanges();

            notificationService.sendNotification(NotificationType.REQUISITION, newRequisition,null,null);


        }

        public List<Requisition> getRequisitionsByIds(List<string> req)
        {
            List<Requisition> selectedReq = new List<Requisition>();
            foreach (string value in req)
            {
                Requisition r = findRequisition(value);
                selectedReq.Add(r);
            }
            return selectedReq; 
        }

        // To check if the requistion is fulfilled with iterating through each items.
        public bool isPartialFufiled(Requisition req)
        {
            List<RequisitionDetail> details = req.RequisitionDetails.ToList();
            bool verdict = false;
            List<bool> result = new List<bool>();
            foreach (var d in details)
            {
                if (d.RequestedQty - d.DistributedQty == 0) result.Add(false);
                else result.Add(true);
            }
            if (result.Contains(true)) return verdict = true;
            return verdict;
        }

        public Req_Complier joinRequisitionDetails(int year,int month) {
            var re = from req in dbcontext.requisitions
                     join req_d in dbcontext.requisitionDetails on req.Id equals req_d.RequisitionId
                     group req_d by new { req.DateSubmitted.Year, req.DateSubmitted.Month, req_d.Inventory.ItemCategory.name } into g
                     where (g.Key.Year == year && g.Key.Month >= month)
                     select new Req_Complier
                     {
                         Year = g.Key.Year,
                         Month = g.Key.Month,
                         InventoryCategory = g.Key.name,
                         Qty = g.Sum(x => x.RequestedQty)
                     };

            return (Req_Complier) re;
        }

    }
}
