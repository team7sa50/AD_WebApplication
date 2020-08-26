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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Team7_StationeryStore.ApiControllers
{
    public class RequisitionApiController : Controller
    {
        protected StationeryContext dbcontext;
        protected RequisitionService reqService;
        protected InventoryService invService;
        protected DepartmentService deptService;


        public RequisitionApiController(RequisitionService reqService, InventoryService invService, DepartmentService deptService, StationeryContext dbcontext)
        {
            this.deptService = deptService;
            this.invService = invService;
            this.reqService = reqService;
            this.dbcontext = dbcontext;
        }
        // GET: api/<controller>/api/requisition/get-detail-information-by-req-id
        [HttpGet]
        [Route("api/[controller]/get-detail-information-by-req-id")]
        public ActionResult GetDetail(String id)
        {
            var details = reqService.retrieveRequisitionDetailList(id).Select(x => new
            {
                itemCode = x.Inventory.itemCode,
                description = x.Inventory.description,
                qty = x.RequestedQty
            });
            return Content(JsonConvert.SerializeObject(details, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }
        [HttpGet]
        [Route("api/[controller]/getAllRequisitions")]
        public ActionResult GetAllRequisitions()
        {
            List<Requisition> requisitions = reqService.retrieveAllRequisitions();
            return Content(JsonConvert.SerializeObject(requisitions, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

        }
        [HttpGet]
        [Route("api/[controller]/getAllRequisitionsByEmpId")]
        public ActionResult GetAllRequisitionsBYEmpId(string empId)
        {

            var items = (from c in dbcontext.requisitions
                         where c.EmployeeId == empId
                         orderby c.DateSubmitted descending
                         select new
                         {
                             Id = c.Id,
                             DateSubmitted = c.DateSubmitted,
                             status=c.status.ToString(),
                             remarks=c.Remarks
                         }
          );

            return Content(JsonConvert.SerializeObject(items, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

        }

        [HttpGet]
        [Route("api/[controller]/getAllPendingRequisitionsByAppEmpId")]
        public ActionResult GetAllPendingRequisitionsByAppEmpId(string empId)
        {

            var items = (from c in dbcontext.requisitions
                         where c.ApprovedEmployeeId == empId && c.status==ReqStatus.AWAITING_APPROVAL
                         orderby c.DateSubmitted descending
                         select new
                         {
                             Id = c.Id,
                             DateSubmitted = c.DateSubmitted,
                             status = c.status.ToString(),
                             remarks = c.Remarks
                         }
          );

            return Content(JsonConvert.SerializeObject(items, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Route("api/[controller]")]

        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        [Route("api/[controller]")]

        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Route("api/[controller]")]

        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Route("api/[controller]")]

        public void Delete(int id)
        {
        }
    }
}
