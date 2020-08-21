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
        public IActionResult StartAnalytics()
        {
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
            RequestModel rrn = new RequestModel()
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
        }

        [HttpPost]
        public JsonResult GetEmployeeTest(string id)
        {
            Disbursement d = disbService.findDisbursementById(id);
            string disbursementJson = JsonConvert.SerializeObject(d, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Json(disbursementJson);
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

        public IActionResult Home()
        {
            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);
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
        public void SendEmail()
        {
            var fromAddress = new MailAddress("stationerystoreteam7@gmail.com", "From Name");
            var toAddress = new MailAddress("storeclerkteam7@gmail.com", "To Name");
            const string fromPassword = "stationerystore";
            const string subject = "Testing";
            const string body = "Body";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

        }


        public IActionResult viewAdjustmentVouchers() {
            ViewData["adjList"] = invService.findAdjustmentVoucherList(null);
            ViewData["PendingAdjList"] = invService.findAdjustmentVoucherList(Status.PENDING);
            return View();
        }
    }
}
