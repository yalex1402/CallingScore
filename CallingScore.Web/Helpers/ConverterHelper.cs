using CallingScore.Web.Data;
using CallingScore.Web.Data.Entities;
using CallingScore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _dataContext;
        private readonly ICodificationHelper _codificationHelper;
        private readonly ICallHelper _callHelper;
        private readonly IUserHelper _userHelper;

        public ConverterHelper(DataContext dataContext,
            ICodificationHelper codificationHelper,
            ICallHelper callHelper,
            IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _codificationHelper = codificationHelper;
            _callHelper = callHelper;
            _userHelper = userHelper;
        }

        public async Task<List<CallEntity>> ToCallEntity (List<DataUploadedViewModel> model)
        {
            List<CallEntity> calls = new List<CallEntity>();
            foreach (DataUploadedViewModel item in model)
            {
                CallEntity call = new CallEntity
                {
                    CustomerId = item.CustomerId,
                    CustomerProduct = item.CustomerProduct,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    Codification = await _codificationHelper.GetCodification(item.CodificationId),
                    User = _userHelper.GetUserByCodeAsync(item.UserCode)
                };
                calls.Add(call);
            }
            return calls;
        }
    }
}
