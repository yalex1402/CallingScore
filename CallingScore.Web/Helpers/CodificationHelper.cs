using CallingScore.Web.Data;
using CallingScore.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallingScore.Web.Helpers
{
    public class CodificationHelper : ICodificationHelper
    {
        private readonly DataContext _dataContext;

        public CodificationHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> AddCode(CodificationEntity codification)
        {
            if(codification == null)
            {
                return false;
            }
            _dataContext.Add(codification);
            await _dataContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<CodificationEntity>> GetCodes()
        {
            List<CodificationEntity> codes = await _dataContext.Codifications.ToListAsync();
            return codes;
        }

        public async Task<CodificationEntity> GetCodification(int? id)
        {
            CodificationEntity code = await _dataContext.Codifications
                .FirstOrDefaultAsync(c => c.Id == id);
            return code;
        }
    }
}
