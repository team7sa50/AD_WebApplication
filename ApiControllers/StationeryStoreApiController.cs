using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;
using Team7_StationeryStore.Service;

namespace Team7_StationeryStore.ApiControllers
{
    [ApiController]
    public class StationeryStoreApiController : ControllerBase
    {

        protected StationeryContext dbcontext;
        protected RequisitionService reqService;
        protected InventoryService invService;
        protected DepartmentService deptService;
        protected DisbursementService disService;
        protected NotificationService notiService;

        public StationeryStoreApiController(RequisitionService reqService, InventoryService invService, DepartmentService deptService, DisbursementService disService, NotificationService notiService, StationeryContext dbcontext)
        {
            this.deptService = deptService;
            this.invService = invService;
            this.reqService = reqService;
            this.dbcontext = dbcontext;
            this.disService = disService;
            this.notiService = notiService;
        }
        [HttpGet]
        [Route("api/[controller]/viewInventories")]
        public ActionResult viewInventories()
        {
            List<Inventory> inventories = invService.getAllInventories();
            return Content(JsonConvert.SerializeObject(inventories));
        }
        [HttpGet]
        [Route("api/[controller]/viewInventoryDetail")]
        public ActionResult viewInventoryDetail(string invId)
        {
            Inventory inventory = invService.retrieveInventory(invId);
            return Content(JsonConvert.SerializeObject(inventory));
        }
    }
}