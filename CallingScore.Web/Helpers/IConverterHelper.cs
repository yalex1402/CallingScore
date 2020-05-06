using CallingScore.Common.Models;
using CallingScore.Web.Data.Entities;
using CallingScore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<List<CallEntity>> ToCallEntity(List<CallsUploadedViewModel> model);

        List<ArrivalsEntity> ToArrivalsEntity(List<ArrivalsUploadedViewModel> model);

        UserResponse ToUserResponse(UserEntity userEntity);
    }
}
