using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Team7_StationeryStore.Controllers
{
    public class StationeryStoreController : Controller
    {
        public IActionResult Index()
        {
            Console.WriteLine("Reach the stationery store controller");
            return View();
        }
    }
}
