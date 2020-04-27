using CallingScore.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Helpers
{
    public interface ICodificationHelper
    {
        Task<bool> AddCode(CodificationEntity codification);

        Task<CodificationEntity> GetCodification(int? id);

        Task<List<CodificationEntity>> GetCodes();

    }
}
