using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Team7_StationeryStore.Models;
using Team7_StationeryStore.Services;
using Team7_StationeryStore.Database;
namespace Team7_StationeryStore.Controllers
{
    public class DepartmentController : Controller
    {
        protected StationeryContext dbcontext;
        public DepartmentController(StationeryContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        private Employee employee;

        public IActionResult Index(string userid)
        {
            return View();
        }

        public IActionResult viewCatalogue() {
            List<Inventory> stationeryCatalogue = dbcontext.inventories.ToList();
            ViewData["stationeryCatalgoue"] = stationeryCatalogue;
            return View();
        }

        public IActionResult RaiseRequisition() {
            ViewData["employee"] = employee;
            return View();
        }

        public IActionResult AddToCart(string itemId,int qty) {
            return RedirectToAction("");
        }


    }
}
