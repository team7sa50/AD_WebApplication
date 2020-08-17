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
    public class StationeryStoreController : Controller
    {
        protected StationeryContext dbcontext;
        protected RetrievalService rservice;

        public StationeryStoreController(StationeryContext dbcontext, RetrievalService rservice)
        {
            this.dbcontext = dbcontext;
            this.rservice = rservice;
        }

        public IActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("Reached Stationery Store Controller");
            return RedirectToAction("viewRetrieval", "StationeryStore");
        }
        
        [HttpPost]
        public JsonResult getDisbursementsByDepartment(string id)
        {
            List<Disbursement> disbursements = (from d in dbcontext.disbursements
                                               where d.Departments.Id == id
                                               select d).ToList();
            return Json(new { data = disbursements });

        }

        public IActionResult viewRetrieval()
        {
            //Replace with the selected Requisitions from RequisitionController
            List<Requisition> selectedReq = (from x in dbcontext.requisitions
                                            select x).ToList();

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
    }
}
