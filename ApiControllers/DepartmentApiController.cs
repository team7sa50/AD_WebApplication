using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;
using Team7_StationeryStore.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Team7_StationeryStore.Controllers
{
    public class DepartmentApiController : Controller
    {
        protected StationeryContext dbcontext;
        protected RequisitionService reqService;
        public DepartmentApiController(StationeryContext dbcontext, RequisitionService reqService)
        {
            this.dbcontext = dbcontext;
            this.reqService = reqService;
        }
        // GET: api/<controller>
        [HttpGet]
        [Route("api/[controller]/viewCatalouge")]
        public ActionResult viewCatalouge()
        {
            List<Inventory> stationeryCatalogue = dbcontext.inventories.ToList();
            return Content(JsonConvert.SerializeObject(stationeryCatalogue));
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
        [Route("api/[controller]/raiseRequisitionForm")]

        public ActionResult raiseRequisitionForm(string userId,[FromBody] List<RequisitionDetail> value)
        {      
            reqService.CreateRequisition(userId, value);
            Object response = new
            {
                message = "Successfully created",
                code = HttpStatusCode.OK

            };
            return Content(JsonConvert.SerializeObject(response));

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
