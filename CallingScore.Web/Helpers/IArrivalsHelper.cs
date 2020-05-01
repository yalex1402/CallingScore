using CallingScore.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Helpers
{
    public interface IArrivalsHelper
    {
        Task<bool> AddArrival(ArrivalsEntity arrival);

        Task<bool> AddArrivals(List<ArrivalsEntity> arrivals);

        Task<List<ArrivalsEntity>> GetArrivals(string id);

        ArrivalsEntity GetArrival(int? id);
    }
}
