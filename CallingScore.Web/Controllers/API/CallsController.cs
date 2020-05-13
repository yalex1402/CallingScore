using CallingScore.Common.Models;
using CallingScore.Web.Data;
using CallingScore.Web.Data.Entities;
using CallingScore.Web.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Controllers.API
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CallsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        public CallsController(DataContext dataContext,
            IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
        }

        [HttpPost]
        [Route("MyStatistics")]
        public async Task<IActionResult> GetMyStatistics([FromBody] StatisticsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserEntity user = _userHelper.GetUserByCodeAsync(request.UserCode);
            List<ContactStatistics> statistics = await _dataContext.ContactStatistics
                .FromSql($"SELECT DAY(c.StartDate) AS Day, CONVERT(FLOAT,ROUND(((COUNT(c.CustomerId)*1.0/tab.Total_Contacto)*100),0)) AS PercentContact FROM Calls AS c INNER JOIN Codifications AS cod  ON cod.Id = c.CodificationId INNER JOIN (SELECT DAY(c.StartDate) AS Dia, COUNT(c.CustomerId) AS Total_Contacto FROM Calls AS c WHERE c.UserId = {user.Id} AND MONTH(c.StartDate) = {request.Month} GROUP BY DAY(c.StartDate)) AS tab ON tab.Dia = DAY(c.StartDate) WHERE cod.ContactType = 0 AND c.UserId = {user.Id} AND MONTH(c.StartDate) = {request.Month} GROUP BY DAY(c.StartDate),tab.Total_Contacto ORDER BY DAY(c.StartDate) ASC").ToListAsync();
            List<EffectivityStatistics> statistics2 = await _dataContext.EffectivityStatistics
                .FromSql($"SELECT DAY(c.StartDate) AS Day, CONVERT(FLOAT,ROUND(((COUNT(c.CustomerId)*1.0/tab.Total_Contacto)*100),0)) AS PercentEffectivity FROM Calls AS c INNER JOIN Codifications AS cod  ON cod.Id = c.CodificationId INNER JOIN (SELECT DAY(c.StartDate) AS Dia, COUNT(c.CustomerId) AS Total_Contacto FROM Calls AS c WHERE c.UserId = {user.Id} AND MONTH(c.StartDate) = {request.Month} GROUP BY DAY(c.StartDate)) AS tab ON tab.Dia = DAY(c.StartDate) WHERE cod.EffectivityType = 0 AND c.UserId = {user.Id} AND MONTH(c.StartDate) = {request.Month} GROUP BY DAY(c.StartDate),tab.Total_Contacto ORDER BY DAY(c.StartDate) ASC").ToListAsync();
            ToShowChart response = new ToShowChart
            {
                ContactStatistics = statistics,
                EffectivityStatistics = statistics2
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("StatisticsByCampaign")]
        public async Task<IActionResult> GetStatisticsByCampaign([FromBody] StatisticsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<ContactStatistics> statistics = await _dataContext.ContactStatistics
                .FromSql(
                $"SELECT DAY(c.StartDate) AS Day, CONVERT(FLOAT,ROUND(((COUNT(c.CustomerId)*1.0/tab.Total_Contacto)*100),0)) AS PercentContact FROM Calls AS c INNER JOIN Codifications AS cod  ON cod.Id = c.CodificationId INNER JOIN AspNetUsers AS u ON u.Id = c.UserId INNER JOIN (SELECT DAY(c.StartDate) AS Dia,COUNT(c.CustomerId) AS Total_Contacto FROM Calls AS c INNER JOIN AspNetUsers AS u ON u.Id = c.UserId WHERE u.CampaignId = {request.CampaignId} AND MONTH(c.StartDate) = {request.Month} GROUP BY DAY(c.StartDate)) AS tab ON tab.Dia = DAY(c.StartDate) WHERE cod.ContactType = 0 AND u.CampaignId = {request.CampaignId} AND MONTH(c.StartDate) = {request.Month} GROUP BY DAY(c.StartDate),tab.Total_Contacto ORDER BY DAY(c.StartDate) ASC"
                ).ToListAsync();
            List<EffectivityStatistics> statistics2 = await _dataContext.EffectivityStatistics
                .FromSql(
                $"SELECT DAY(c.StartDate) AS Day, CONVERT(FLOAT,ROUND(((COUNT(c.CustomerId)*1.0/tab.Total_Contacto)*100),0)) AS PercentEffectivity FROM Calls AS c INNER JOIN Codifications AS cod  ON cod.Id = c.CodificationId INNER JOIN AspNetUsers AS u ON u.Id = c.UserId INNER JOIN (SELECT DAY(c.StartDate) AS Dia,COUNT(c.CustomerId) AS Total_Contacto FROM Calls AS c INNER JOIN AspNetUsers AS u ON u.Id = c.UserId WHERE u.CampaignId = {request.CampaignId} AND MONTH(c.StartDate) = {request.Month} GROUP BY DAY(c.StartDate)) AS tab ON tab.Dia = DAY(c.StartDate) WHERE cod.EffectivityType = 0 AND u.CampaignId = {request.CampaignId} AND MONTH(c.StartDate) = {request.Month} GROUP BY DAY(c.StartDate),tab.Total_Contacto ORDER BY DAY(c.StartDate) ASC"
                ).ToListAsync();
            ToShowChart response = new ToShowChart
            {
                ContactStatistics = statistics,
                EffectivityStatistics = statistics2
            };
            return Ok(response);
        }

    }
}
