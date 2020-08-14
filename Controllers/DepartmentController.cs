using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Team7_StationeryStore.Models;
using Team7_StationeryStore.Database;
using Microsoft.AspNetCore.Http;
using Team7_StationeryStore.Service;

namespace Team7_StationeryStore.Controllers
{
    public class DepartmentController : Controller
    {
        protected StationeryContext dbcontext;
        protected RequisitionService reqService;
        protected InventoryService invService;
        protected DepartmentService deptService;


        public DepartmentController(RequisitionService reqService, InventoryService invService, DepartmentService deptService ,StationeryContext dbcontext)
        {
            this.deptService = deptService;
            this.invService = invService;
            this.reqService = reqService;
            this.dbcontext = dbcontext;
        }

        public IActionResult Index(string userid)
        {
            Employee employee = dbcontext.employees.Where(x => x.Id == userid).FirstOrDefault();

            return View();
        }

        public IActionResult viewCatalogue() {
            List<Inventory> stationeryCatalogue = invService.retrieveCatalogue();
            ViewData["stationeryCatalgoue"] = stationeryCatalogue;
            return View();
        }

        public IActionResult RaiseRequisition() {
            ViewData["employee"] = dbcontext.employees.Where(x=> x.Id == HttpContext.Session.GetString("UserId")).FirstOrDefault();
            return View();
        }

        public IActionResult AddToCart(string itemId,int qty) {
            return RedirectToAction("");
        }

        public IActionResult viewRequisitionList() {
            string userId = HttpContext.Session.GetString("userId");
            Employee employee = deptService.findEmployeeById("userId");
            List<Requisition> Requisition = reqService.retrieveRequisitionList(employee);
            ViewData["Requisitions"] = Requisition;
            return View();
        }

        public IActionResult viewRequisition(string requisitionId)
        {
            string userId = HttpContext.Session.GetString("userId");
            Requisition requisition = reqService.findRequisition(requisitionId);
            ViewData["Requisition"] = requisition;
            TempData["UserId"] = userId;
            return View();
        }

        public IActionResult approveRequisition(string requisitionId) {
            string userId = HttpContext.Session.GetString("userId");
            Requisition requisition = dbcontext.requisitions.Where(x => x.Id == requisitionId).FirstOrDefault();
            if (requisition == null) {
                ViewData["ErrorMsg"] = "Failed to locate requisition";    
                return RedirectToAction("ViewPendingRequisition");
            }
            if (requisition.EmployeeId == userId) {
                ViewData["ErrorMsg"] = "Cannot self-approve requisition";
                return RedirectToAction("ViewPendingRequisition");
            }
            requisition.status = ReqStatus.APPROVED;
            dbcontext.Update(requisition);
            dbcontext.SaveChanges();
            return RedirectToAction();       
        }

        public IActionResult rejectRequisition(string requisitionId) {
            string userId = HttpContext.Session.GetString("userId");
            Requisition requisition = dbcontext.requisitions.Where(x => x.Id == requisitionId).FirstOrDefault();
            requisition.status = ReqStatus.REJECTED;
            dbcontext.Update(requisition);
            dbcontext.SaveChanges();
            return View();

        }

        public IActionResult delegateAuthority() {
            string userId = HttpContext.Session.GetString("userId");
            List<Employee> deptEmployees = deptService.findDepartmentEmployeeList(userId);
            TempData["UserId"] = userId;
            ViewData["Employees"] = deptEmployees;
            return View();
        }

        public IActionResult submitDelegation(IFormCollection form) {

            var userId = TempData["UserId"];
            string Name = form["employeeName"];
            string SD = form["startDate"];
            string ED = form["endDate"];
            if (deptService.IsinvalidDate(SD,ED)) {
                ViewData["DateError"] = "Wrong Input Date";
                return RedirectToAction("DelegateAuthority");
            }
            deptService.createEmployeeAuthorization(Name, SD, ED);
            return RedirectToAction("DelegateAuthority");
        }

        
    }
}
