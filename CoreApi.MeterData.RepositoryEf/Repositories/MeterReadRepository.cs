using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreApi.MeterData.Dal;
using Microsoft.EntityFrameworkCore;

namespace CoreApi.MeterData.RepositoryEf.Repositories
{
    public class MeterReadRepository : BaseRepository<MeterRead>, IMeterReadRepository
    {
        public MeterReadRepository(MeterDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<MeterRead> GetMeterReadIfExist(int accountId, DateTime meterReadDateTime, int readValue)
        {
            return await _dbContext.MeterRead.FirstOrDefaultAsync(t => t.AccountId == accountId &&
                                t.MeterReadingDateTime == meterReadDateTime && t.ReadValue == readValue);
        }
    }
}
