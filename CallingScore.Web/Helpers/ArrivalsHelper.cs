using CallingScore.Web.Data;
using CallingScore.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Helpers
{
    public class ArrivalsHelper : IArrivalsHelper
    {
        private readonly DataContext _dataContext;

        public ArrivalsHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> AddArrival(ArrivalsEntity arrival)
        {
            if (arrival == null)
            {
                return false;
            }
            _dataContext.Add(arrival);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddArrivals(List<ArrivalsEntity> arrivals)
        {
            if (arrivals == null)
            {
                return false;
            }
            foreach (ArrivalsEntity arrival in arrivals)
            {
                _dataContext.Add(arrival);
                await _dataContext.SaveChangesAsync();
            }
            return true;
        }

        public ArrivalsEntity GetArrival(int? id)
        {
            ArrivalsEntity arrival = _dataContext.Arrivals
                .Include(a => a.User)
                .FirstOrDefault(a => a.Id == id);
            return arrival;
        }

        public async Task<List<ArrivalsEntity>> GetArrivals(string id)
        {
            List<ArrivalsEntity> arrivals = await _dataContext.Arrivals
                .Include(a => a.User)
                .Where(a => a.User.Id == id)
                .ToListAsync();
            return arrivals;
        }
    }
}
