using CallingScore.Common.Models;
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

        public async Task<List<CallEntity>> ToCallEntity (List<CallsUploadedViewModel> model)
        {
            List<CallEntity> calls = new List<CallEntity>();
            foreach (CallsUploadedViewModel item in model)
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

        public List<ArrivalsEntity> ToArrivalsEntity(List<ArrivalsUploadedViewModel> model)
        {
            List<ArrivalsEntity> arrivals = new List<ArrivalsEntity>();
            foreach (ArrivalsUploadedViewModel item in model)
            {
                ArrivalsEntity arrival = new ArrivalsEntity
                {
                     InDate = item.InDate,
                     OutDate = item.OutDate,
                     User = _userHelper.GetUserByCodeAsync(item.UserCode)
                };
                arrivals.Add(arrival);
            }
            return arrivals;
        }

        public UserResponse ToUserResponse(UserEntity userEntity)
        {
            if (userEntity == null)
            {
                return null;
            }
            return new UserResponse
            {
                Id = userEntity.Id,
                UserCode = userEntity.UserCode,
                Document = userEntity.Document,
                Email = userEntity.Email,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                PhoneNumber = userEntity.PhoneNumber,
                PicturePath = userEntity.PicturePath,
                UserType = userEntity.UserType,
                Campaign = ToCampaign(userEntity.Campaign),
                LoginType = userEntity.LoginType
            };
        }

        public Campaign ToCampaign (CampaignEntity campaign)
        {
            if(campaign == null)
            {
                return null;
            }
            return new Campaign
            {
                Id = campaign.Id,
                Name = campaign.Name
            };
        }

    }
}
