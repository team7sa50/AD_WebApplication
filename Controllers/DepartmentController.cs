using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Team7_StationeryStore.Models;
using Team7_StationeryStore.Database;
using Microsoft.AspNetCore.Http;
using Team7_StationeryStore.Service;
using Newtonsoft.Json;
using Team7_StationeryStore.Models.Requisitions;

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

        public IActionResult Home()
        {
            Employee employee = dbcontext.employees.Where(x => x.Id == HttpContext.Session.GetString("userId")).FirstOrDefault();
            ViewData["Employee"] = employee;
            return View();
        }

        public IActionResult viewCatalogue()
        {
            string userid = HttpContext.Session.GetString("userId");
            List<Inventory> stationeryCatalogue = invService.retrieveCatalogue();
            List<ItemCategory> categories = invService.retrieveCategories();
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["stationeryCatalgoue"] = stationeryCatalogue;
            ViewData["categories"] = categories;
            ViewData["username"] = emp.Name;
            return View();
        }

        [HttpPost]
        public IActionResult Search(string SearchString, string? userid)
        {
            List<ItemCategory> categories = invService.retrieveCategories();
            List<Inventory> items = new List<Inventory>();

            if (!String.IsNullOrEmpty(SearchString))
            {
                items = dbcontext.inventories.Where(s => s.itemCode.Contains(SearchString) || s.description.Contains(SearchString)).ToList();
            }
            ViewData["categories"] = categories;
            ViewData["stationeryCatalgoue"] = items;
            ViewData["userid"] = userid;
            return View("viewCatalogue");
        }

        public IActionResult RaiseRequisition() {
            reqService.CreateRequisition(HttpContext.Session.GetString("userId"));
            return RedirectToAction("viewRequisitionList", "Department");
        }

        public IActionResult AddToCart(string itemId, int quantity)
        {
            string userid = HttpContext.Session.GetString("userId");
            var User = dbcontext.employees.Where(x => x.Id == userid).FirstOrDefault();
            AddItem(userid, itemId, quantity);
            return RedirectToAction("viewCatalogue");
        }
        public IActionResult ViewCart()
        {
            string userid = HttpContext.Session.GetString("userId");
            List<EmployeeCart> employeeCarts = reqService.retrieveEmployeeCart(userid);
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["employeeCarts"] = employeeCarts;
            ViewData["username"] = emp.Name;
            ViewData["userid"] = userid;
            return View();
        }
        public void AddItem(string userid, string itemid, int qty)
        {
            var oldcartItem = dbcontext.employeeCarts
                .Where(x => x.EmployeeId == userid && x.InventoryId == itemid)
                .FirstOrDefault();
            if (oldcartItem == null)
            {
                var cartItem = new EmployeeCart()
                {
                    Id = Guid.NewGuid().ToString(),
                    EmployeeId = userid,
                    InventoryId = itemid,
                    Qty = qty,
                    Inventory = dbcontext.inventories.SingleOrDefault(p => p.Id == itemid)
                };
                dbcontext.employeeCarts.Add(cartItem);
                dbcontext.SaveChanges();

            }
            else
            {
                oldcartItem.Qty = qty;
                dbcontext.Update(oldcartItem);
                dbcontext.SaveChanges();
            }
        }

        /*public IActionResult UpdateQty(string itemId, int newQty)
            int newqty = newQty;
            string itemid = itemId;
            string user = HttpContext.Session.GetString("userid");

            var cartItem = dbcontext.employeeCarts
                .Where(x => x.EmployeeId == user && x.InventoryId == itemid)
                .FirstOrDefault();

            if (cartItem != null)
            {
                if (newqty > 0)
                {
                    cartItem.Qty = newqty;
                }
                else
                {
                    RemoveItem(user, itemid);
                }
            }
            dbcontext.SaveChanges();
            return RedirectToAction("viewRequisition");
        }*/

        public IActionResult RemoveItem(string userid, string itemId)
        {
            var cartItem = dbcontext.employeeCarts
                .Where(x => x.EmployeeId == userid && x.InventoryId == itemId)
                .FirstOrDefault();
            if (cartItem != null)
            {
                dbcontext.employeeCarts.Remove(cartItem);
            }
           
            dbcontext.SaveChanges();
            return RedirectToAction("ViewCart");
        }

        public IActionResult RemoveAllItems()
        {
            string userid = HttpContext.Session.GetString("userId");
            var cartItem = dbcontext.employeeCarts
                .Where(x => x.EmployeeId == userid)
                .ToList();
            if (cartItem != null)
            {
                foreach (var i in cartItem)
                    dbcontext.employeeCarts.Remove(i);
            }

            dbcontext.SaveChanges();
            return RedirectToAction("viewCatalogue");
        }
        
        //For Employee
        public IActionResult viewRequisitionList() {
            string userId = HttpContext.Session.GetString("userId");
            Employee employee = deptService.findEmployeeById(userId);
            List<Requisition> Requisition = reqService.retrieveRequisitionByEmployee(employee);
            ViewData["Requisitions"] = Requisition;
            return View();
        }
        //For Department Head
        public IActionResult viewPendingRequisition() {
            string deptId = HttpContext.Session.GetString("Department");
            List<Requisition> pendingRequisitions = reqService.findRequisitionsByDept(deptId, ReqStatus.AWAITING_APPROVAL);
            List<Requisition> allRequisitions = reqService.findRequisitionsByDept(deptId, null);
            ViewData["PendingRequisitions"] = pendingRequisitions;
            ViewData["AllRequisitions"] = allRequisitions;
            return View();
        }

        [Route("Department/findRequisitionDetail/{reqId}")]
        public ActionResult findRequisitionDetail(String reqId)
        {
            List<RequisitionDetailView> requisitionDetails = reqService.findRequisitionDetail(reqId);
            return Content(JsonConvert.SerializeObject(requisitionDetails));
        }

        public IActionResult viewRequisition(string requisitionId)
        {
            string userId = HttpContext.Session.GetString("userId");
            Requisition requisition = reqService.findRequisition(requisitionId);
            ViewData["Requisition"] = requisition;
            TempData["UserId"] = userId;
            return View();
        }

        public IActionResult updateRequisition(string requisitionId, string remarks,string action)
        {
            string userId = HttpContext.Session.GetString("userId");
            Requisition requisition = dbcontext.requisitions.Where(x => x.Id == requisitionId).FirstOrDefault();
            if (requisition == null)
            {
                ViewData["ErrorMsg"] = "Failed to locate requisition";
                return RedirectToAction("ViewPendingRequisition");
            }
            if (requisition.EmployeeId == userId)
            {
                ViewData["ErrorMsg"] = "Cannot self-approve requisition";
                return RedirectToAction("ViewPendingRequisition");
            }
            if(action == "approve") { requisition.status = ReqStatus.APPROVED; }
            if(action == "reject") { requisition.status = ReqStatus.REJECTED; }
            requisition.Remarks = remarks;
            dbcontext.Update(requisition);
            dbcontext.SaveChanges();
            return RedirectToAction("viewPendingRequisition");
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
