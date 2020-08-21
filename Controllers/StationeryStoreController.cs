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
using System.Text;

namespace Team7_StationeryStore.Controllers
{
    public class StationeryStoreController : Controller
    {
        protected StationeryContext dbcontext;
        protected RetrievalService rservice;
        protected RequisitionService requisitionService;
        protected InventoryService invService;
        protected DepartmentService deptService;

        public StationeryStoreController(StationeryContext dbcontext, RetrievalService rservice,RequisitionService requisitionService,InventoryService invService,DepartmentService deptService)
        {
            this.dbcontext = dbcontext;
            this.rservice = rservice;
            this.requisitionService = requisitionService;
            this.invService = invService;
            this.deptService = deptService;
        }

        public IActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("Reached Stationery Store Controller");
            return RedirectToAction("viewRetrieval", "StationeryStore");
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
        public JsonResult getDisbursementsByDepartment(string id)
        {
            List<Disbursement> disbursements = (from d in dbcontext.disbursements
                                               where d.Departments.Id == id
                                               select d).ToList();
            return Json(new { data = disbursements });
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
            List<Requisition> selectedReq=new List<Requisition>();
            foreach (string value in req)
            {
                Requisition r = requisitionService.findRequisition(value);
                selectedReq.Add(r);
            }
            //Replace with the selected Requisitions from RequisitionController
           // List<Requisition> selectedReq = (from x in dbcontext.requisitions
                                            //select x).ToList();

            //Retrieval List Generation Starts here
            List<RequisitionDetail> selectedReqDetail = rservice.getRequisitionDetail(selectedReq);          
            Dictionary<string, int> totalItemQty = rservice.getTotalQtyPerItem(selectedReqDetail);
            Dictionary<string, List<RequisitionDetail>> reqPerIt = rservice.getReqDetailPerItem(selectedReqDetail);
            ViewData["totalItemQty"] = totalItemQty;
            rservice.recommendQty(reqPerIt);
            Dictionary<string, List<RequisitionDetail>> reqPerDept = rservice.getReqPerDeptPerItem(reqPerIt);
            ViewData["reqPerDept"] = reqPerDept;
            return View();        
        }
        public IActionResult viewDisbursements()
        {
            List<Departments> departments = (from d in dbcontext.departments
                                             select d).ToList();
            ViewData["departments"] = departments;
            //On click of button, transfer retrieved requests here
            List < Requisition > selectedReq = (from x in dbcontext.requisitions
                                                select x).ToList();
            List<RequisitionDetail> selectedReqDetail = rservice.getRequisitionDetail(selectedReq);
            
            //Convert request to disbursement 
            Dictionary<Departments, List<RequisitionDetail>> requisitionsForDepartment = new Dictionary<Departments, List<RequisitionDetail>>();
            foreach (var r in selectedReqDetail)
            {
                if (!requisitionsForDepartment.ContainsKey(r.Requisition.Department))
                {
                    requisitionsForDepartment.Add(r.Requisition.Department, new List<RequisitionDetail>());
                    requisitionsForDepartment[r.Requisition.Department].Add(r);
                }
                else if (requisitionsForDepartment.ContainsKey(r.Requisition.Department))
                {
                    requisitionsForDepartment[r.Requisition.Department].Add(r);
                }
            }
            System.Diagnostics.Debug.WriteLine("Sorted by departments");
            System.Diagnostics.Debug.WriteLine("Total Count:" + requisitionsForDepartment.Count);


            foreach (var dept in requisitionsForDepartment)
            {
                System.Diagnostics.Debug.WriteLine("Creating disbursements");
                Disbursement d = new Disbursement();
                d.Id = Guid.NewGuid().ToString();
                d.GeneratedDate = DateTime.Now;
                d.CollectionDate = DateTime.Now.AddDays(1);
                d.Departments = dept.Key;
                d.DepartmentsId = dept.Key.Id;
                d.status = DisbusementStatus.PENDING;
                dbcontext.Add(d);
                dbcontext.SaveChanges();
                
                foreach (var req in dept.Value)
                {
                    DisbursementDetail ddetail = new DisbursementDetail();
                    ddetail.Id = Guid.NewGuid().ToString(); 
                    ddetail.DisbursementId = d.Id;
                    ddetail.Disbursement = d;
                    ddetail.RequisitionDetail = req;
                    ddetail.disbursedQty = req.DistributedQty;
                    dbcontext.Add(ddetail);
                    dbcontext.SaveChanges();
                }
            }
            //get all disbursements and display 

            List<Disbursement> dibs = (from d in dbcontext.disbursements
                                       where d.status == DisbusementStatus.PENDING
                                       select d).ToList();
            System.Diagnostics.Debug.WriteLine("Total disbursements Count:" + dibs.Count);
            ViewData["disbursements"] = dibs;

            List<Disbursement> dibCompleted = (from d in dbcontext.disbursements
                                               where d.status == DisbusementStatus.COMPLETED
                                               select d).ToList();
            ViewData["completedDisb"] = dibCompleted;
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
