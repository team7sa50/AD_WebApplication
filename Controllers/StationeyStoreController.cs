using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Team7_StationeryStore.Controllers
{
    public class StationeyStoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
