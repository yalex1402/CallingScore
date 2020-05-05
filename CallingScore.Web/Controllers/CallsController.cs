using CallingScore.Web.Data;
using CallingScore.Web.Data.Entities;
using CallingScore.Web.Helpers;
using CallingScore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;

        public CallsController(DataContext dataContext,
            IConverterHelper converterHelper,
            ICallHelper callHelper,
            IArrivalsHelper arrivalsHelper,
            IUserHelper userHelper,
            ICombosHelper combosHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
            _callHelper = callHelper;
            _arrivalsHelper = arrivalsHelper;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
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

        public IActionResult StatisticsMain()
        {
            return View();
        }

        public async Task<IActionResult> Statistics()
        {
            List<ContactStatisticsViewModel> statistics = await _dataContext.ContactStatistics
                .FromSql(@"
                    SELECT DAY(c.StartDate) AS Day
	                    , CONVERT(FLOAT,ROUND(((COUNT(c.CustomerId)*1.0/tab.Total_Contacto)*100),0)) AS PercentContact
                    FROM Calls AS c 
                    INNER JOIN Codifications AS cod 
                    ON cod.Id = c.CodificationId
                    INNER JOIN (SELECT DAY(c.StartDate) AS Dia
				                    , COUNT(c.CustomerId) AS Total_Contacto
			                    FROM Calls AS c
			                    GROUP BY DAY(c.StartDate)) AS tab ON tab.Dia = DAY(c.StartDate)
                    WHERE cod.ContactType = 0
                    GROUP BY DAY(c.StartDate),tab.Total_Contacto
                    ORDER BY DAY(c.StartDate) ASC
                ").ToListAsync();
            List<EffectivityStatisticsViewModel> statistics2 = await _dataContext.EffectivityStatistics
                .FromSql(@"
                    SELECT DAY(c.StartDate) AS Day
	                    , CONVERT(FLOAT,ROUND(((COUNT(c.CustomerId)*1.0/tab.Total_Contacto)*100),0)) AS PercentEffectivity
                    FROM Calls AS c 
                    INNER JOIN Codifications AS cod 
                    ON cod.Id = c.CodificationId
                    INNER JOIN (SELECT DAY(c.StartDate) AS Dia
				                    , COUNT(c.CustomerId) AS Total_Contacto
			                    FROM Calls AS c
			                    GROUP BY DAY(c.StartDate)) AS tab ON tab.Dia = DAY(c.StartDate)
                    WHERE cod.EffectivityType = 0
                    GROUP BY DAY(c.StartDate),tab.Total_Contacto
                    ORDER BY DAY(c.StartDate) ASC
                ").ToListAsync();
            ToShowChartViewModel model = new ToShowChartViewModel
            {
                ContactStatistics = statistics,
                EffectivityStatistics = statistics2
            };
            return View(model);
        }

        public IActionResult SelectUser()
        {
            ToShowCharByUserViewModel model = new ToShowCharByUserViewModel
            {
                Users = _combosHelper.GetComboUsers()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> StatisticsByUser(ToShowCharByUserViewModel model)
        {
            UserEntity userId =  _userHelper.GetUserByCodeAsync(model.UserCode);
            List<ContactStatisticsViewModel> statistics = await _dataContext.ContactStatistics
                .FromSql($"SELECT DAY(c.StartDate) AS Day, CONVERT(FLOAT,ROUND(((COUNT(c.CustomerId)*1.0/tab.Total_Contacto)*100),0)) AS PercentContact FROM Calls AS c INNER JOIN Codifications AS cod  ON cod.Id = c.CodificationId INNER JOIN (SELECT DAY(c.StartDate) AS Dia, COUNT(c.CustomerId) AS Total_Contacto FROM Calls AS c WHERE c.UserId = {userId.Id} GROUP BY DAY(c.StartDate)) AS tab ON tab.Dia = DAY(c.StartDate) WHERE cod.ContactType = 0 AND c.UserId = {userId.Id} GROUP BY DAY(c.StartDate),tab.Total_Contacto ORDER BY DAY(c.StartDate) ASC").ToListAsync();
            List<EffectivityStatisticsViewModel> statistics2 = await _dataContext.EffectivityStatistics
                .FromSql($"SELECT DAY(c.StartDate) AS Day, CONVERT(FLOAT,ROUND(((COUNT(c.CustomerId)*1.0/tab.Total_Contacto)*100),0)) AS PercentEffectivity FROM Calls AS c INNER JOIN Codifications AS cod  ON cod.Id = c.CodificationId INNER JOIN (SELECT DAY(c.StartDate) AS Dia, COUNT(c.CustomerId) AS Total_Contacto FROM Calls AS c WHERE c.UserId = {userId.Id} GROUP BY DAY(c.StartDate)) AS tab ON tab.Dia = DAY(c.StartDate) WHERE cod.EffectivityType = 0 AND c.UserId = {userId.Id} GROUP BY DAY(c.StartDate),tab.Total_Contacto ORDER BY DAY(c.StartDate) ASC").ToListAsync();
            model.ContactStatistics = statistics;
            model.EffectivityStatistics = statistics2;
            return View(model);
        }

        public IActionResult SelectCampaign()
        {
            ToShowCharByCampaignViewModel model = new ToShowCharByCampaignViewModel
            {
                Campaigns = _combosHelper.GetComboCampaigns()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> StatisticsByCampaign(ToShowCharByCampaignViewModel model)
        {
            List<ContactStatisticsViewModel> statistics = await _dataContext.ContactStatistics
                .FromSql($"SELECT DAY(c.StartDate) AS Day,CONVERT(FLOAT,ROUND(((COUNT(c.CustomerId)*1.0/tab.Total_Contacto)*100),0)) AS PercentContact FROM Calls AS c INNER JOIN Codifications AS cod  ON cod.Id = c.CodificationId INNER JOIN AspNetUsers AS u ON u.Id = c.UserId  INNER JOIN (SELECT DAY(c.StartDate) AS Dia, COUNT(c.CustomerId) AS Total_Contacto FROM Calls AS c INNER JOIN AspNetUsers AS u ON u.Id = c.UserId WHERE u.CampaignId ={model.CampaignId} GROUP BY DAY(c.StartDate)) AS tab ON tab.Dia = DAY(c.StartDate) WHERE cod.ContactType = 0 AND u.CampaignId ={model.CampaignId} GROUP BY DAY(c.StartDate),tab.Total_Contacto ORDER BY DAY(c.StartDate) ASC").ToListAsync();
            List<EffectivityStatisticsViewModel> statistics2 = await _dataContext.EffectivityStatistics
                .FromSql($"SELECT DAY(c.StartDate) AS Day,CONVERT(FLOAT,ROUND(((COUNT(c.CustomerId)*1.0/tab.Total_Contacto)*100),0)) AS PercentEffectivity FROM Calls AS c INNER JOIN Codifications AS cod  ON cod.Id = c.CodificationId INNER JOIN AspNetUsers AS u ON u.Id = c.UserId  INNER JOIN (SELECT DAY(c.StartDate) AS Dia, COUNT(c.CustomerId) AS Total_Contacto FROM Calls AS c INNER JOIN AspNetUsers AS u ON u.Id = c.UserId WHERE u.CampaignId ={model.CampaignId} GROUP BY DAY(c.StartDate)) AS tab ON tab.Dia = DAY(c.StartDate) WHERE cod.EffectivityType = 0 AND u.CampaignId ={model.CampaignId} GROUP BY DAY(c.StartDate),tab.Total_Contacto ORDER BY DAY(c.StartDate) ASC").ToListAsync();
            model.ContactStatistics = statistics;
            model.EffectivityStatistics = statistics2;
            return View(model);
        }

    }
}
