using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Team7_StationeryStore.Controllers
{
    public class DepartmentApiController : Controller
    {
        protected StationeryContext dbcontext;
        public DepartmentApiController(StationeryContext dbcontext)
        {
            this.dbcontext = dbcontext;
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

        public ActionResult raiseRequisitionForm([FromBody]Employee value)
        {
            // dynamic request = JsonConvert.DeserializeObject(value);
            return Content(JsonConvert.SerializeObject(value));

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
