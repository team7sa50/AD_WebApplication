using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Newtonsoft.Json;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;
using Team7_StationeryStore.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Team7_StationeryStore.Controllers
{

    public class HomeApiController : Controller
    {
        protected StationeryContext dbcontext;
        protected DepartmentService deptService;
        public HomeApiController(StationeryContext dbcontext, DepartmentService deptService)
        {
            this.dbcontext = dbcontext;
            this.deptService = deptService;
        }

        // GET: api/<controller>
        [HttpPost]
        [Route("api/[controller]/login")]
        public ActionResult Login([FromBody]Employee value)
        {

            if (value.Email == null || value.Password == null)
            {
                Object response = new
                {
                    message = "Please input credentials",
                    code = HttpStatusCode.NonAuthoritativeInformation

                };
                return Content(JsonConvert.SerializeObject(response));
            }
            else
            {
                /*                Employee empInfo = dbcontext.employees
                                                .Where(x => x.Email == value.Email)
                                                .FirstOrDefault();*/
                Employee empInfo = deptService.findEmployeeByEmail(value.Email);
                using (MD5 md5Hash = MD5.Create())
                {
                    string Hash_Password = MD5Hash.GetMd5Hash(md5Hash, value.Password);

                    if (empInfo == null || empInfo.Password != Hash_Password)
                    {
                        Object response = new
                        {
                            message = " Unauthorized",
                            code = HttpStatusCode.Unauthorized

                        };
                        return Content(JsonConvert.SerializeObject(response));
                    }
                    else
                    {
                        Object response = new
                        {
                            id = empInfo.Id,
                            name = empInfo.Name,
                            email = empInfo.Email,
                            departmentName = empInfo.Departments.DeptName,
                            role = empInfo.Role.ToString(),
                            status = empInfo.Status,
                            message = "Successfully Login",
                            code = HttpStatusCode.OK

                        };
                        return Content(JsonConvert.SerializeObject(response));
                    }
                }
            }
        }

    }
}

