using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;
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
        }
    }
}
