using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;
using Team7_StationeryStore.Models.Requisitions;
using Team7_StationeryStore.Service;
using Microsoft.AspNetCore.Http;

namespace Team7_StationeryStore.Controllers
{
    public class RequisitionController : Controller
    {
        protected StationeryContext dbcontext;
        protected RequisitionService reqService;
        protected InventoryService invService;
        protected DepartmentService deptService;


        public RequisitionController(RequisitionService reqService, InventoryService invService, DepartmentService deptService, StationeryContext dbcontext)
        {
            this.deptService = deptService;
            this.invService = invService;
            this.reqService = reqService;
            this.dbcontext = dbcontext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult viewAllRequisitionsFromStationery()
        {
            List<Requisition> requisitions = reqService.findAllRequisitionsFromStationery();
            ViewData["requisitions"] = requisitions;
            return View();
        }
        public IActionResult ViewRequisitions()
        {
            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);
            List<Departments> departments = deptService.findAllDepartments();
            List<Requisition> requisitions = reqService.findPendingREquisitionsFromStationery();
            List<Requisition> oustandingReq = reqService.findOustandingRequisitions();
            List<string> statuses = new List<string>();
            foreach (var e in Enum.GetValues(typeof(ReqStatus)))
            {
                if (e.ToString() == "APPROVED" || e.ToString() == "OUTSTAND")
                {
                    Console.WriteLine("Enum: " + e.ToString());
                    statuses.Add(e.ToString());
                }
            }
            ViewData["status"] = statuses;
            ViewData["outsandingReq"] = oustandingReq;
            ViewData["departments"] = departments; 
            ViewData["requisitions"] = requisitions;
            ViewData["user"] = emp;
            return View();
        }
        public IActionResult ViewRequisitionsByFilter(string Department)
        {
            List<Departments> departments = deptService.findAllDepartments();
            List<Requisition> requisitions = reqService.findAllRequisitionsFromFilter(Department);
            List<Requisition> oustandingReq = reqService.findOustandingRequisitions();
            ViewData["outsandingReq"] = oustandingReq;
            ViewData["departments"] = departments;
            ViewData["requisitions"] = requisitions;
            return View("ViewRequisitions");
        }

        [HttpPost]
        public JsonResult GetRequisitionDetail(string reqId)
        {
            System.Diagnostics.Debug.WriteLine("Reached Requisition Controller...");
            Requisition requisit = (from rll in dbcontext.requisitions
                             where rll.Id == reqId
                             select rll).FirstOrDefault();
            string requisitionJson = JsonConvert.SerializeObject(requisit, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return Json(requisitionJson);
        }


    }
}