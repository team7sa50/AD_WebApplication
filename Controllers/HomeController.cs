using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;
using System.Security.Cryptography;

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
            return View();
        }

        public IActionResult Login(string Email, string Password) {

            System.Diagnostics.Debug.WriteLine("Reached Login Controller");
            if (Email == null || Password == null) {
                return View();
            }
            System.Diagnostics.Debug.WriteLine("Processing Login Information Now");
            Employee user = dbcontext.employees.Where(x => x.Email == Email).FirstOrDefault();
            if (user == null && Password != null)
            {
                ViewData["login_error"] = "The username cannot be found.";

                return View();
            }
            System.Diagnostics.Debug.WriteLine("User found, comparing password now");
            ViewData["userId"] = user.Id;

            using (MD5 md5Hash = MD5.Create())
            {
                System.Diagnostics.Debug.WriteLine("Hash?");
                string hashPwd = MD5Hash.GetMd5Hash(md5Hash, Password);
                System.Diagnostics.Debug.WriteLine(hashPwd);
                System.Diagnostics.Debug.WriteLine(user.Password);
                if (user.Password != hashPwd)
                {
                    ViewData["login_error"] = "Password is wrong. Please try again.";
                    return View();
                }
                System.Diagnostics.Debug.WriteLine("Hashed");
            }


            if (user.Role == Role.DEPT_HEAD || user.Role == Role.DEPT_REP || user.Role == Role.EMPLOYEE)
            {
                return RedirectToAction("Index", "Department", new {userid = user.Id});
            }
            else {
                System.Diagnostics.Debug.WriteLine("Login successful, redirecting to clerk");
                return RedirectToAction("Index", "Home");
                /*return RedirectToAction("Index", "StationeryStore", new { userid = user.Id});*/
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
