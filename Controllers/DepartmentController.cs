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
        protected DisbursementService disService;
        protected NotificationService notiService;

        public DepartmentController(RequisitionService reqService, InventoryService invService, DepartmentService deptService, DisbursementService disService,NotificationService notiService, StationeryContext dbcontext)
        {
            this.deptService = deptService;
            this.invService = invService;
            this.reqService = reqService;
            this.dbcontext = dbcontext;
            this.disService = disService;
            this.notiService = notiService;
        }

        public IActionResult Home()
        {
            Employee employee = dbcontext.employees.Where(x => x.Id == HttpContext.Session.GetString("userId")).FirstOrDefault();
            ViewData["Employee"] = employee;
            string userid = HttpContext.Session.GetString("userId");
            Departments dept = deptService.findDepartmentByEmployee(userid);
            ViewData["dept"] = dept;
            ViewData["Authorizer"] = deptService.IsAuthorizer(userid);
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
            ViewData["user"] = emp;
            ViewData["Authorizer"] = deptService.IsAuthorizer(userid);

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
            string userId = HttpContext.Session.GetString("userId");

            ViewData["Authorizer"] = deptService.IsAuthorizer(userId);

            return View("viewCatalogue");
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
            ViewData["user"] = emp;
            ViewData["Authorizer"] = deptService.IsAuthorizer(userid);

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

        public IActionResult RaiseRequisition()
        {
            reqService.CreateRequisition(HttpContext.Session.GetString("userId"));
            return RedirectToAction("viewRequisitionList", "Department");
        }

        //For Employee
        public IActionResult viewRequisitionList() {
            string userId = HttpContext.Session.GetString("userId");
            Employee employee = deptService.findEmployeeById(userId);
            List<Requisition> Requisition = reqService.retrieveRequisitionByEmployee(employee);
            ViewData["Requisitions"] = Requisition;
            ViewData["userid"] = userId;
            Employee emp = deptService.findEmployeeById(userId);
            ViewData["username"] = emp.Name;
            ViewData["user"] = emp;
            ViewData["Authorizer"] = deptService.IsAuthorizer(userId);
            return View();
        }

        public IActionResult ViewRequisitionDetail(string reqid)
        {
            string userid = HttpContext.Session.GetString("userId");
            List<RequisitionDetail> requisitionDetails = reqService.retrieveRequisitionDetailList(reqid);
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["user"] = emp;
            ViewData["requisitionDetail"] = requisitionDetails;
            ViewData["username"] = emp.Name;
            ViewData["userid"] = userid;
            Requisition requisition = reqService.findRequisition(reqid);
            ViewData["reqid"] = requisition.Id;
            ViewData["reqstatus"] = requisition.status;
            ViewData["Authorizer"] = deptService.IsAuthorizer(userid);

            return View();
        }

        //For Department Head / Authorize Employee
        public IActionResult viewDepartmentRequisition() {
            string deptId = HttpContext.Session.GetString("Department");
            string userId = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userId);

            List<Requisition> pendingRequisitions = reqService.findRequisitionsByDept(deptId, ReqStatus.AWAITING_APPROVAL);
            List<Requisition> allRequisitions = reqService.findRequisitionsByDept(deptId, null);
            bool IsAuthorized = deptService.IsAuthorizer(userId);
            ViewData["PendingRequisitions"] = pendingRequisitions;
            ViewData["AllRequisitions"] = allRequisitions;
            ViewData["Authorizer"] = IsAuthorized;
            ViewData["user"] = emp;
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
            Employee emp = deptService.findEmployeeById(userId);
            ViewData["username"] = emp.Name;
            ViewData["Authorizer"] = deptService.IsAuthorizer(userId);

            return View();
        }

        public IActionResult updateRequisition(string requisitionId, string remarks,string action)
        {
            string userId = HttpContext.Session.GetString("userId");
            if(action == "approve") { reqService.updateRequisition(userId, requisitionId, ReqStatus.APPROVED, remarks); }
            if(action == "reject") { reqService.updateRequisition(userId, requisitionId, ReqStatus.REJECTED, remarks); }
            return RedirectToAction("viewDepartmentRequisition");
        }

        public IActionResult viewDepartmentDisbursements() {
            string userId = HttpContext.Session.GetString("userId");
            List<Disbursement> disbursements = disService.retrieveDisbursementByDept(HttpContext.Session.GetString("Department"));
            ViewData["disbursements"] = disbursements;
            ViewData["Authorizer"] = deptService.IsAuthorizer(userId);
            return View();
        }

        [Route("Department/findDisbursementDetail/{disId}")]
        public ActionResult findDisbursementDetail(String disId)
        {
            List<DisbursementDetailView> disbursementDetails = disService.retrieveDisbursmentDetailsView(disId);
            return Content(JsonConvert.SerializeObject(disbursementDetails));
        }

        public IActionResult delegateAuthority() {
            string userId = HttpContext.Session.GetString("userId");
            List<Employee> deptEmployees = deptService.findDepartmentEmployeeList(userId);
            TempData["UserId"] = userId;
            ViewData["Employees"] = deptEmployees;
            ViewData["Authorizer"] = deptService.IsAuthorizer(userId);
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
            return RedirectToAction("Home");
        }
        [HttpPost]
        public ActionResult getLatestNotifications()
        {
            string emp = HttpContext.Session.GetString("userId");
            List<Notification> notifications = notiService.retrieveLatestNotifications(emp);
            List<NotifcationString> strings = new List<NotifcationString>();
            foreach (var not in notifications)
            {
                NotifcationString s = new NotifcationString();
                s.Name = not.Sender.Name;
                s.reqId = not.typeId;
                s.date = not.date.ToString("MM / dd / yyyy h:mm");
                strings.Add(s);
            }
            return Content(JsonConvert.SerializeObject(strings));
        }
        public IActionResult viewNewRequisition(String reqId)
        {
            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);

            Requisition req = reqService.findRequisition(reqId);
            List<RequisitionDetail> requisitionDetails = reqService.retrieveRequisitionDetailList(reqId);
            ViewData["requisition"] = req;
            ViewData["username"] = emp.Name;
            ViewData["requisitionDetail"] = requisitionDetails;
            ViewData["Authorizer"] = deptService.IsAuthorizer(userid);
            return View();
        }

        public IActionResult viewDepartment()
        {
            string userid = HttpContext.Session.GetString("userId");
            Departments dept = deptService.findDepartmentByEmployee(userid);
            ViewData["department"] = dept;
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["username"] = emp.Name;
            CollectionPoint c = dbcontext.collectionPoints.Where(x => x.Id == dept.CollectionPointId).FirstOrDefault();
            ViewData["collectionpoint"] = c;
            ViewData["Authorizer"] = deptService.IsAuthorizer(userid);

            return View();
        }

        
        public IActionResult editDepartmentDetail()
        {
            string userid = HttpContext.Session.GetString("userId");
            Departments dept = deptService.findDepartmentByEmployee(userid);
            ViewData["department"] = dept;
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["username"] = emp.Name;
            ViewData["Authorizer"] = deptService.IsAuthorizer(userid);
            return View();
        }

        
        public IActionResult submit(string Location)
        {
            string userid = HttpContext.Session.GetString("userId");
            
            
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["username"] = emp.Name;
            Departments dept = deptService.findDepartmentByEmployee(userid);
            ViewData["department"] = dept;
            if (dept != null)
            {
                CollectionPoint c = dbcontext.collectionPoints.Where(x => x.Id == dept.CollectionPointId).FirstOrDefault();
                c.Location = Location;
                dbcontext.SaveChanges();
            }
            return RedirectToAction("viewDepartment");
        }

        public IActionResult back()
        {
            return RedirectToAction("Home");
        }
        public IActionResult DepartmentAnalysis()
        {
            string userid = HttpContext.Session.GetString("userId");
            bool IsAuthorized = deptService.IsAuthorizer(userid);

            Employee emp = deptService.findEmployeeById(userid);
            ViewData["user"] = emp;
            List<AnalysisModel> aQ = deptService.startDepartmentAnalysis(emp.Id);
            ViewData["dict"] = aQ;
            ViewData["Authorizer"] = IsAuthorized;

            return View();
        }

    }
}
