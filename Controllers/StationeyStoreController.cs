using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;
using Team7_StationeryStore.Service;

namespace Team7_StationeryStore.Controllers
{
    public class StationeyStoreController : Controller
    {

        protected StationeryContext dbcontext;
        protected InventoryService invService;

        public StationeyStoreController(StationeryContext dbcontext, InventoryService invService)
        {
            
            this.dbcontext = dbcontext;
            this.invService = invService;
        }
        public IActionResult Index()
        {
            return View();
        }


        
    }
}
