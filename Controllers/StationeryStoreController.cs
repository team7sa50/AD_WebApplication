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
        protected Trainer trainer;
        protected Predictor predictor;
        public StationeryStoreController(StationeryContext dbcontext, RetrievalService rservice,RequisitionService requisitionService,InventoryService invService,DepartmentService deptService, DisbursementService disbService, Trainer trainer, Predictor predictor)
        {
            this.dbcontext = dbcontext;
            this.rservice = rservice;
            this.requisitionService = requisitionService;
            this.invService = invService;
            this.deptService = deptService;
            this.disbService = disbService;
            this.trainer = trainer;
            this.predictor = predictor;
        }

        public IActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("List Employee Method reached");
            return View();
        }
        public IActionResult Home()
        {
            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["username"] = emp.Name;
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
       /* public IActionResult StartAnalytics()
        {
            int Year = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month; // Auguest
            int past1Month = currentMonth - 1; // July
            int past2Month = currentMonth - 2; // June

            var po = from p in dbcontext.purchaseOrders
                     join pod in dbcontext.purchaseOrderDetails on p.Id equals pod.PurchaseOrderId
                     group pod by new { pod.Inventory.ItemCategory.name, p.date.Month, p.date.Year } into h
                     orderby h.Key.Year,h.Key.Month
                     where (h.Key.Month >= past2Month && h.Key.Year == Year)
                     select new
                     {
                         ItemCat = h.Key.name,
                         Month = h.Key.Month.ToString("MMM"),
                         Qty = h.Sum(x => x.quantity)
                     };

            




            // Gathering of data
            IEnumerable <Req> requisitionTable = from req in dbcontext.requisitions
                                                   join req_d in dbcontext.requisitionDetails
                                                   on req.Id equals req_d.RequisitionId into g
                                                   from d in g.DefaultIfEmpty()
                                                   orderby req.DateSubmitted
                                                   select new Req_Complier
                                                   {
                                                       Date = req.DateSubmitted,
                                                       /* Department = req.DepartmentId,
                                                          Item = d.InventoryId,
                                                       Qty = (float)d.RequestedQty
                                                   };


            trainer.TimeSeriesForcasting(requisitionTable);
            string traindata = @"C:\Users\User'\source\repos\team7sa50\AD_WebApplication\Analytics\Data\sampledata.csv";
            string testdata = @"C:\Users\User'\source\repos\team7sa50\AD_WebApplication\Analytics\Data\testdata.csv";
            System.Diagnostics.Debug.WriteLine("Starting Training");
            trainer.Train(traindata, testdata);           
            System.Diagnostics.Debug.WriteLine("Finished Training");
            return View();
        }
        
        [HttpPost]
        public JsonResult AnalyzeResults(int requestedQty, int stockQty, string dateT)
        {
            string month = dateT.Substring(5, 2);
            string year = dateT.Substring(0, 4);
            System.Diagnostics.Debug.WriteLine("Month: " + month);
            System.Diagnostics.Debug.WriteLine("Year" + year);
            Req_Complier rrn = new Req_Complier()
            {
                RequestedQty = requestedQty.ToString(),
                InventoryQty = stockQty.ToString(),
                Month = month,
                Year = year
            };
            string inputJson = JsonConvert.SerializeObject(rrn, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            System.Diagnostics.Debug.WriteLine("Results:" + inputJson);
            CarInventoryPrediction cp = predictor.Predict(inputJson);
            string results = JsonConvert.SerializeObject(cp, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });           
            return Json(results);
        }*/

        [HttpPost]
        public JsonResult GetEmployeeTest(string id)
        {
            Disbursement d = disbService.findDisbursementById(id);
            Employee deptRep = deptService.findDeptRepresentative(d.DepartmentsId);
            string disbursementJson = JsonConvert.SerializeObject(d, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            string rep = JsonConvert.SerializeObject(deptRep,new JsonSerializerSettings {ReferenceLoopHandling=ReferenceLoopHandling.Ignore });
            var result = new { disbursementJson, rep };
            return Json(result);
        }
        public IActionResult startPurchaseOrderAnalysis()
        {
            ViewData["categories"] = invService.retrieveCategories();
            return View();
        }
        public IActionResult ViewAnalysis(string category)
        {
            ItemCategory cat = invService.retrieveCategory(category);
            var past4Month = DateTime.Now.AddMonths(-4).Month;
            var Year = DateTime.Now.Year;
            var po = from p in dbcontext.purchaseOrders
                     join pod in dbcontext.purchaseOrderDetails on p.Id equals pod.PurchaseOrderId
                     group pod by new { pod.Inventory.ItemCategory.name, p.date.Month, p.date.Year } into h
                     where (h.Key.Month >= past4Month && h.Key.Year == Year && h.Key.name == cat.name)
                     orderby(h.Key.Month)
                     select new
                     {
                         Month = h.Key.Month,
                         Qty = h.Sum(x => x.quantity)
                     };
            List<PurchaseOrderQuantity> poQ = new List<PurchaseOrderQuantity>();
            foreach(var c in po)
            {
                PurchaseOrderQuantity p= new PurchaseOrderQuantity();
                p.Month = c.Month;
                p.quantity = c.Qty;
                poQ.Add(p);
            }
            ViewData["dict"] = poQ;
            ViewData["category"] =cat.name ;
            return View();
        }
        public IActionResult startRequisitionAnalysis()
        {
            ViewData["categories"] = invService.retrieveCategories();
            ViewData["departments"] = invService.getAllDepartments();
            return View();
        }
        public IActionResult ViewRequisitionAnalysis(string category,string department)
        {
            ItemCategory cat = invService.retrieveCategory(category);
            Departments dept = invService.getDepartmentDetail(department); 
            var past4Month = DateTime.Now.AddMonths(-4).Month;
            var Year = DateTime.Now.Year;
            var po = from req in dbcontext.requisitions
                     join req_d in dbcontext.requisitionDetails on req.Id equals req_d.RequisitionId
                     group req_d by new { req_d.Inventory.ItemCategory.name, req.Department.DeptName, req.DateSubmitted.Month,req.DateSubmitted.Year} into h
                     where (h.Key.Month >= past4Month && h.Key.Year == Year && h.Key.DeptName== dept.DeptName && h.Key.name == cat.name)
                     orderby (h.Key.Month)
                     select new
                     {
                         Month = h.Key.Month,
                         Qty = h.Sum(x => x.RequestedQty)
                     };
            List<PurchaseOrderQuantity> reQ = new List<PurchaseOrderQuantity>();
            foreach (var c in po)
            {
                PurchaseOrderQuantity p = new PurchaseOrderQuantity();
                p.Month = c.Month;
                p.quantity = c.Qty;
                reQ.Add(p);
            }
            ViewData["dict"] = reQ;
            ViewData["category"] = cat.name;
            ViewData["department"] = dept.DeptName;
            return View();
        }
        [HttpGet]
        [Route("api/[controller]/GetDataToAnalyze")]
        public ActionResult GetDataToAnalyze()
        {
            var past2Month = DateTime.Now.AddMonths(-2).Month;
            var Year = DateTime.Now.Year;
            string itemCat = "Clip";
            var po = from p in dbcontext.purchaseOrders
                     join pod in dbcontext.purchaseOrderDetails on p.Id equals pod.PurchaseOrderId
                     group pod by new { pod.Inventory.ItemCategory.name, p.date.Month, p.date.Year} into h
                     where (h.Key.Month >= past2Month && h.Key.Year == Year && h.Key.name == itemCat)
                     select new
                     {
                         Month = h.Key.Month,
                         Category =h.Key.name,
                         Qty = h.Sum(x => x.quantity)
                     };
            /*List<String> currentMonthPOIds = invService.retrievePurchaseOrder(DateTime.Now);
            List<String> previousMonthPOIds = invService.retrievePurchaseOrder(DateTime.Now.AddMonths(-1));
           // List<String> secondPreviousMonthPOIds = invService.retrievePurchaseOrder(DateTime.Now.AddMonths(-2));
            List<PurchaseOrderDetails> currentMonthPODetails = invService.retrievePurchaseOrderDetails(currentMonthPOIds);
            List<PurchaseOrderDetails> previousMonthPODetails = invService.retrievePurchaseOrderDetails(previousMonthPOIds);
            //List<PurchaseOrderDetails> secondPreviousMonthPODetails = invService.retrievePurchaseOrderDetails(secondPreviousMonthPOIds);
            Dictionary<string,int> currentMonthPODetailsTop3 = invService.findPurchaseOrderTop(currentMonthPODetails);
            Dictionary<string, int> previousMonthPODetailsTop3 = invService.findPurchaseOrderTop(previousMonthPODetails);*/

            
            return Content(JsonConvert.SerializeObject(po));

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
            return View();
        }

        public IActionResult viewRetrieval(List<string> req)
        {
            List<Requisition> selectedReq = requisitionService.getRequisitionsByIds(req);
            System.Diagnostics.Debug.WriteLine("Selected Requests: " + selectedReq.Count);
            /*List<RequisitionDetail> selectedReqDetail = rservice.getRequisitionDetail(selectedReq);
            System.Diagnostics.Debug.WriteLine("Selected Requests: " + selectedReqDetail.Count);*/
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
            return View();        
        }

        public IActionResult generateDisbursement(List<string> req)
        {
            //Transfer retrieved requests here
            List<Requisition> selectedReq = requisitionService.getRequisitionsByIds(req);
            List<RequisitionDetail> selectedReqDetail = rservice.getRequisitionDetail(selectedReq);

            //Convert request to disbursement 
            Dictionary<Departments, List<RequisitionDetail>> requisitionsForDepartment = disbService.sortRequisitionByDept(selectedReqDetail);
            disbService.saveRequisitionsAsDisbursement(requisitionsForDepartment);

            return RedirectToAction("viewDisbursements");
        }

        public IActionResult viewDisbursements()
        { 
            ViewData["disbursements"] = disbService.getAllPendingDisbursements();
            ViewData["completedDisb"] = disbService.getAllCompletedDisbursements();
            ViewData["collectPoints"] = deptService.findAllCollectionPts();
            ViewData["departments"] = deptService.findAllDepartments();
            return View();
        }
        //should set collection date as next workign day 

        public IActionResult CreateAdjustment(string itemId, int quantity, string reason) {
            string userid = HttpContext.Session.GetString("userId");
            invService.CreateAdjustmentVoucher(userid, itemId, quantity, reason);
            return RedirectToAction("ViewInventory");
        }

        public IActionResult updateAdjustmentVoucher(string adjVoucherId,string action,string remarks) {
            string userId = HttpContext.Session.GetString("userId");
            ViewData["response"] = invService.UpdateAdjustmentVoucher(adjVoucherId, action, remarks);
            return RedirectToAction("viewAdjustmentVouchers");
        }
       

        public IActionResult viewAdjustmentVouchers() {
            ViewData["adjList"] = invService.findAdjustmentVoucherList(null);
            ViewData["PendingAdjList"] = invService.findAdjustmentVoucherList(Status.PENDING);
            return View();
        }
        public IActionResult HomeManagerSupervisor()
        {
            Employee employee = dbcontext.employees.Where(x => x.Id == HttpContext.Session.GetString("userId")).FirstOrDefault();
            ViewData["user"] = employee;
            return View();
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
