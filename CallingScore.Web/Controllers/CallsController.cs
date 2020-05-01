using CallingScore.Web.Data;
using CallingScore.Web.Data.Entities;
using CallingScore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Controllers
{
    public class CallsController : Controller
    {
        private readonly DataContext _dataContext;

        public CallsController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult UploadCalls()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadCalls(UploadCallViewModel model)
        {
            using (StreamReader reader = new StreamReader(model.File.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    string[] entries = (reader.ReadLine()).Split(",");
                    //Create a ViewModel and then convert in CallEntity
                }
            }
            return View(model);
        }
    }
}
