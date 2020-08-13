﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;
using Team7_StationeryStore.Services;

namespace Team7_StationeryStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        protected StationeryContext dbcontext;
        protected UserService userService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Login() {
            return View();
        }

        public IActionResult Login(string email, string password) {

            if (email == null || password == null) {
                return View();
            }

            Employee user = userService.findEmployee(email);
            if (user == null || password !=user.Password) {
                ViewData["login_error"] = "User not found/Password Incorrect";
                return View();
            }
            if (user.Role == Role.DEPT_HEAD || user.Role == Role.DEPT_REP || user.Role == Role.EMPLOYEE)
            {
                return RedirectToAction("Index", "Department", new {userid = user.Id});
            }
            else {

                return RedirectToAction("Home", "StationeryStore", new { userid = user.Id});
            }
        }
        public ActionResult Logout()
        {
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
