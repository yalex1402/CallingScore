using CallingScore.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Helpers
{
    public interface ICallHelper
    {
        Task<bool> AddCall(CallEntity call);

        Task<CallEntity> GetCall(int id);

        Task<List<CallEntity>> GetCalls(string id, DateTime startDate, DateTime endDate);
    }
}
