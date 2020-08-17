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
            List<Departments> departments = deptService.findAllDepartments();
            List<Requisition> requisitions = reqService.findAllRequisitionsFromStationery();
            List<Requisition> oustandingReq = reqService.findOustandingRequisitions();
            ViewData["outsandingReq"] = oustandingReq;
            ViewData["departments"] = departments; 
            ViewData["requisitions"] = requisitions;
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

        [Route("Requisition/GetRequisitionDetail/{reqId}")]
        public ActionResult GetRequisitionDetail(String reqId)
        {
            List<RequisitionDetailView> requisitionDetails = reqService.findRequisitionDetail(reqId);
            return Content(JsonConvert.SerializeObject(requisitionDetails));
        }


    }
}