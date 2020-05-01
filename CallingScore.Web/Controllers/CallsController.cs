﻿using CallingScore.Web.Data;
using CallingScore.Web.Data.Entities;
using CallingScore.Web.Helpers;
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
        private readonly IConverterHelper _converterHelper;
        private readonly ICallHelper _callHelper;
        private readonly IArrivalsHelper _arrivalsHelper;

        public CallsController(DataContext dataContext,
            IConverterHelper converterHelper,
            ICallHelper callHelper,
            IArrivalsHelper arrivalsHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
            _callHelper = callHelper;
            _arrivalsHelper = arrivalsHelper;
        }

        public IActionResult UploadCalls()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadCalls(UploadDataViewModel model)
        {
            List<CallsUploadedViewModel> dataUploaded = new List<CallsUploadedViewModel>();
            try
            {
                using (StreamReader reader = new StreamReader(model.File.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        string[] entries = (reader.ReadLine()).Split(";");
                        CallsUploadedViewModel data = new CallsUploadedViewModel
                        {
                            CustomerId = entries[0],
                            CustomerProduct = entries[1],
                            StartDate = DateTime.Parse(entries[2]),
                            EndDate = DateTime.Parse(entries[3]),
                            CodificationId = Int32.Parse(entries[4]),
                            UserCode = entries[5]
                        };
                        dataUploaded.Add(data);
                    }
                    List<CallEntity> callsUploaded = await _converterHelper.ToCallEntity(dataUploaded);
                    bool IsSuccess = await _callHelper.AddCalls(callsUploaded);
                    if (!IsSuccess)
                    {
                        ViewBag.Message = "There has been an error when trying to process the file";
                        return View(model);
                    }
                }
                ViewBag.Message = $"The file {model.File.FileName} has been processed successfully!";
            }
            catch (Exception)
            {
                ViewBag.Message = "There has been an error when trying to process the file";
            }
            
            return View(model);
        }

        public IActionResult UploadArrivals()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadArrivals(UploadDataViewModel model)
        {
            List<ArrivalsUploadedViewModel> dataUploaded = new List<ArrivalsUploadedViewModel>();
            try
            {
                using (StreamReader reader = new StreamReader(model.File.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        string[] entries = (reader.ReadLine()).Split(";");
                        ArrivalsUploadedViewModel data = new ArrivalsUploadedViewModel
                        {
                            InDate = DateTime.Parse(entries[0]),
                            OutDate = DateTime.Parse(entries[1]),
                            UserCode = entries[2]
                        };
                        dataUploaded.Add(data);
                    }
                    List<ArrivalsEntity> arrivalsUploaded = _converterHelper.ToArrivalsEntity(dataUploaded);
                    bool IsSuccess = await _arrivalsHelper.AddArrivals(arrivalsUploaded);
                    if (!IsSuccess)
                    {
                        ViewBag.Message = "There has been an error when trying to process the file";
                        return View(model);
                    }
                }
                ViewBag.Message = $"The file {model.File.FileName} has been processed successfully!";
            }
            catch (Exception)
            {
                ViewBag.Message = "There has been an error when trying to process the file";
            }

            return View(model);
        }
    }
}
