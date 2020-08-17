using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;

namespace Team7_StationeryStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        protected StationeryContext dbcontext;
        public HomeController(ILogger<HomeController> logger, StationeryContext dbcontext)
        {
            _logger = logger;
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        public IActionResult Login(string email, string password)
        {
            if (email == null || password == null)
            {
                ViewData["login_error"] = "Fill in User and Password";
                return View();
            }
            Employee user = dbcontext.employees.Where(x => x.Email == email).FirstOrDefault();
            using (MD5 md5Hash = MD5.Create())
            {
                string hashPwd = MD5Hash.GetMd5Hash(md5Hash, password);
                if (user == null || user.Password != hashPwd)
                {
                    ViewData["login_error"] = "User not found/Password Incorrect";
                    return View();
                }
            }
            ViewData["userId"] = user.Id;
            HttpContext.Session.SetString("userId", user.Id);
            HttpContext.Session.SetString("Department", user.DepartmentsId); 
            if (user.Role == Role.DEPT_HEAD || user.Role == Role.DEPT_REP || user.Role == Role.EMPLOYEE)
            {
                /*return RedirectToAction("viewCatalogue", "Department");*/
                return RedirectToAction("Home", "Department");
            }
            if(user.Role == Role.STORE_CLERK)
            {
                return RedirectToAction("ViewRequisitions", "Requisition");
            }
            else
            {
                return RedirectToAction("Home", "StationeryStore", new { userid = user.Id });
            }
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
