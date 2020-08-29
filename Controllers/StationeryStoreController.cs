using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Team7_StationeryStore.Models;
using Team7_StationeryStore.Database;
using Microsoft.AspNetCore.Http;
using Team7_StationeryStore.Service;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Team7_StationeryStore.Analytics;
using Team7_StationeryStore.Analytics.ML.Objects;
using Team7_StationeryStore.Analytics.ML;
using System.Collections;
using System.Security.Cryptography;

namespace Team7_StationeryStore.Controllers
{
    public class StationeryStoreController : Controller
    {
        protected StationeryContext dbcontext;
        protected RetrievalService rservice;
        protected RequisitionService requisitionService;
        protected InventoryService invService;
        protected DepartmentService deptService;
        protected DisbursementService disbService;
        protected NotificationService notifService;
        public StationeryStoreController(StationeryContext dbcontext, RetrievalService rservice,RequisitionService requisitionService,InventoryService invService,DepartmentService deptService, DisbursementService disbService,NotificationService notifService)
        {
            this.dbcontext = dbcontext;
            this.rservice = rservice;
            this.requisitionService = requisitionService;
            this.invService = invService;
            this.deptService = deptService;
            this.disbService = disbService;
            this.notifService = notifService;
        }

        public IActionResult Home()
        {
            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["requisitions"] = requisitionService.findLatestRequisitions();
            ViewData["pos"] = invService.findLatestPurchaseOrder();
            ViewData["disbursement"] = disbService.findLatestDisbursements();
            ViewData["username"] = emp.Name;
            ViewData["user"] = emp;
            //Get Latest Requisitions 
            //Get Latest POs
            //Get Latest Disbursements 
            return View();
        }
        public IActionResult HomeManagerSupervisor()
        {
            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);
            List<AdjustmentVoucher> adList = invService.findAdjustmentVoucherToApprove(userid);
            ViewData["adjustmentList"] = adList;
            ViewData["username"] = emp.Name;
            ViewData["user"] = emp;
            return View();
        }
        [HttpPost]
        public IActionResult Export(){
            List<Employee> employees = (from er in dbcontext.employees
                                 select er).ToList();

            List<object> emps = new List<object>();
            int k = 0;
            foreach(Employee e in employees){
                k++;
                emps.Add(new string[6] { e.Id, e.Name , e.Email, e.Password, e.Role.ToString(), e.DepartmentsId});
            }
            emps.Insert(0, new string[6] {"emp_id", "name", "email", "password", "role", "departmentId"});
            StringBuilder sb = new StringBuilder(); 

            for (int i = 0; i < emps.Count; i++)
			{
                string[] employe = (string[])emps[i];

                for(int j=0; j< employe.Length;j++){
                sb.Append(employe[j] +',');
                }
                sb.Append("\r\n");
			}
            return File(Encoding.UTF8.GetBytes(sb.ToString()),"text/csv", "Grid.csv");
        }

        [HttpPost]
        public IActionResult updateRetrievalQty(string rqId, string itemId, string newQty, List<string> requi)
        {
            RequisitionDetail rd = (from r in dbcontext.requisitionDetails
                                    where r.Inventory.description == itemId &&
                                    r.Requisition.Id == rqId
                                    select r).FirstOrDefault();
            rd.DistributedQty = int.Parse(newQty);
            dbcontext.SaveChanges();
            System.Diagnostics.Debug.WriteLine("updateRetrievalQty reqlist: " + requi.Count);
            return RedirectToAction("viewRetrieval", new { req = requi }) ;
        }

        [HttpPost]
        public JsonResult GetEmployeeTest(string id)
        {
            Disbursement d = disbService.findDisbursementById(id);
            Employee deptRep = deptService.findDeptRepresentative(d.DepartmentsId);
            string name = deptRep.Name;
            string disbursementJson = JsonConvert.SerializeObject(d, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            string repName = JsonConvert.SerializeObject(name,new JsonSerializerSettings {ReferenceLoopHandling=ReferenceLoopHandling.Ignore });
            var result = new { disbursementJson, repName };
            return Json(result);
        }
        public IActionResult startPurchaseOrderAnalysis()
        {
            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["user"] = emp;
            ViewData["categories"] = invService.retrieveCategories();
            return View();
        }
        public IActionResult ViewAnalysis(string category)
        {
            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["user"] = emp;
            ItemCategory cat = invService.retrieveCategory(category);
            List<PurchaseOrderQuantity> poQ = invService.startPurchaseOrderAnalysis(cat);
            ViewData["dict"] = poQ;
            ViewData["category"] =cat.name ;
            return View();
        }
        public IActionResult startRequisitionAnalysis()
        {
            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["user"] = emp;
            ViewData["categories"] = invService.retrieveCategories();
            ViewData["departments"] = invService.getAllDepartments();
            return View();
        }
        public IActionResult ViewRequisitionAnalysis(string category,string department)
        {
            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["user"] = emp;
            ItemCategory cat = invService.retrieveCategory(category);
            Departments dept = invService.getDepartmentDetail(department);
            List<PurchaseOrderQuantity> reQ = requisitionService.startRequisitionAnalysis(cat, dept);
            ViewData["dict"] = reQ;
            ViewData["category"] = cat.name;
            ViewData["department"] = dept.DeptName;
            return View();
        }
        [HttpPost]
        public JsonResult SaveStatusToCompletedInDisb(string disId)
        {
            Disbursement d = dbcontext.disbursements.Where(x => x.Id == disId).FirstOrDefault();
            d.status = DisbusementStatus.COMPLETED;
            dbcontext.Update(d);
            dbcontext.SaveChanges();
            return Json("1");

        }
        [HttpPost]
        public JsonResult SaveChangesToDisb(string newDate, string newColl, string rqId)
        {
            rqId = rqId.Remove(rqId.Length - 1, 1);
            Disbursement dbDetail = (from rqd in dbcontext.disbursements
                                    where rqd.Id == rqId
                                    select rqd).FirstOrDefault();

            CollectionPoint cp = (from cps in dbcontext.collectionPoints
                                  where newColl == cps.Location
                                  select cps).FirstOrDefault();
            dbDetail.Departments.CollectionPoint = cp;

            DateTime collDateTimeObj = DateTime.Parse(newDate);
            dbDetail.CollectionDate = collDateTimeObj;
            dbDetail.status = DisbusementStatus.DELIVERED;
            notifService.sendNotification(NotificationType.DISBURSEMENT, null, dbDetail, null);

            dbcontext.SaveChanges();


            return Json("1");
        }
        [HttpPost]
        public IActionResult getDisbursementsByDepartment(string id)
        {
            List<Disbursement> disbursements = (from d in dbcontext.disbursements
                                               where d.Departments.Id == id
                                               select d).ToList();
            return Json(disbursements);
        }
        public IActionResult ViewInventory()
        {
            string userid = HttpContext.Session.GetString("userId");
            List<Inventory> stationeryCatalogue = invService.retrieveCatalogue();
            List<ItemCategory> categories = invService.retrieveCategories();
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["stationeryCatalgoue"] = stationeryCatalogue;
            ViewData["categories"] = categories;
            ViewData["username"] = emp.Name;
            ViewData["user"] = emp;
            return View();
        }

        public IActionResult viewRetrieval(List<string> req)
        {
            List<Requisition> selectedReq = requisitionService.getRequisitionsByIds(req);
            selectedReq.ForEach(x => x.status = ReqStatus.COLLECTION);
            dbcontext.UpdateRange(selectedReq);
            dbcontext.SaveChanges();
            System.Diagnostics.Debug.WriteLine("Selected Requests: " + selectedReq.Count);
            ViewData["Requisitions"] = selectedReq;

            List<RequisitionDetail> selectedReqD = new List<RequisitionDetail>();
            foreach (var r in selectedReq) {
                System.Diagnostics.Debug.WriteLine("Starting to loop through requisitions: " + "1");
                List<RequisitionDetail> rds = (from rd in dbcontext.requisitionDetails
                                               where rd.Requisition == r
                                               select rd).ToList();
               foreach(var rd in rds){
                    System.Diagnostics.Debug.WriteLine("Checking for rd: " + rd.Id);
                    selectedReqD.Add(rd);
                }
             }
            //Retrieval List Generation Starts here
            ViewData["totalItemQty"] = rservice.getTotalQtyPerItem(selectedReqD);
            Dictionary<string, List<RequisitionDetail>> reqPerIt = rservice.getReqDetailPerItem(selectedReqD);
            rservice.recommendQty(reqPerIt);
            ViewData["reqPerDept"] = rservice.getReqPerDeptPerItem(reqPerIt);
            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["user"] = emp;
            return View();        
        }

        public IActionResult generateDisbursement(List<string> req)
        {
            //Transfer retrieved requests here
            List<Requisition> selectedReq = requisitionService.getRequisitionsByIds(req);
            requisitionService.updateRequisitionStatus(selectedReq);
            List<RequisitionDetail> selectedReqDetail = rservice.getRequisitionDetail(selectedReq);
            string userId = HttpContext.Session.GetString("userId");
            //Convert request to disbursement 
            Dictionary<Departments, List<RequisitionDetail>> requisitionsForDepartment = disbService.sortRequisitionByDept(selectedReqDetail);
            disbService.saveRequisitionsAsDisbursement(userId,requisitionsForDepartment);

            return RedirectToAction("viewDisbursements");
        }

        public IActionResult viewDisbursements()
        { 
            ViewData["disbursements"] = disbService.getAllPendingDisbursements();
            ViewData["completedDisb"] = disbService.getAllCompletedDisbursements();
            ViewData["collectPoints"] = deptService.findAllCollectionPts();
            ViewData["departments"] = deptService.findAllDepartments();
            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["user"] = emp;
            return View();
        }
        //should set collection date as next workign day 

        public IActionResult CreateAdjustment(string itemId, int quantity, string reason) {
            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["user"] = emp;
            invService.CreateAdjustmentVoucher(userid, itemId, quantity, reason);
            return RedirectToAction("ViewInventory");
        }

        public IActionResult updateAdjustmentVoucher(string adjVoucherId,string action,string remarks) {
            string userId = HttpContext.Session.GetString("userId");
            ViewData["response"] = invService.UpdateAdjustmentVoucher(adjVoucherId, action, remarks);
            return RedirectToAction("HomeManagerSupervisor");
        }
       

        public IActionResult viewAdjustmentVouchers() {
            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["user"] = emp;
            ViewData["adjList"] = invService.findAdjustmentVoucherList(null);
            ViewData["PendingAdjList"] = invService.findAdjustmentVoucherList(Status.PENDING);
            return View();
        }
        [HttpPost]
        public ActionResult getLatestNotifications()
        {
            string emp = HttpContext.Session.GetString("userId");
            List<Notification> notifications = notifService.retrieveLatestNotifications(emp);
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
        public IActionResult sendDisbursement(string disId) {
            Disbursement dis = disbService.findDisbursementById(disId);
            Employee deptRep = deptService.findDeptRepresentative(dis.Departments.Id);
            List<Requisition> requisitions = dis.Requisitions.ToList();
            foreach(var r in requisitions)
            {
                requisitionService.updateRequisition(null, r.Id, ReqStatus.COLLECTION, null);
            }
            notifService.sendNotification(NotificationType.DISBURSEMENT, null, dis, null);
            return Json(new { msg = "Sent to Dept Rep: {0}",deptRep.Name });
        }
    }
}
