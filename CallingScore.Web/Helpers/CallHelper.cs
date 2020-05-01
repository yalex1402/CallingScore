using CallingScore.Web.Data;
using CallingScore.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Helpers
{
    public class CallHelper : ICallHelper
    {
        private readonly DataContext _dataContext;

        public CallHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> AddCall(CallEntity call)
        {
            if(call == null)
            {
                return false;
            }
            _dataContext.Add(call);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddCalls(List<CallEntity> calls)
        {
            if (calls == null)
            {
                return false;
            }
            foreach (CallEntity call in calls)
            {
                _dataContext.Add(call);
                await _dataContext.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> AddScoreToCall(MonitoringEntity monitoring)
        {
            if (monitoring == null)
            {
                return false;
            }
            _dataContext.Add(monitoring);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<CallEntity> GetCall(int id)
        {
            CallEntity call = await _dataContext.Calls
                .Include(c => c.Codification)
                .Include(c => c.User)
                .ThenInclude(c => c.Monitorings)
                .FirstOrDefaultAsync(c => c.Id == id);
            return call;
        }

        public async Task<List<CallEntity>> GetCalls(string id, DateTime startDate, DateTime endDate)
        {
            List<CallEntity> calls = await _dataContext.Calls
                .Include(c => c.Codification)
                .Include(c => c.User)
                .ThenInclude(c => c.Monitorings)
                .Where (c => c.User.Id == id && c.StartDate >= startDate && c.EndDate <= endDate)
                .ToListAsync();
            return calls;
        }
    }
}
