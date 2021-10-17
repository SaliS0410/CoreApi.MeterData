using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreApi.MeterData.Dal
{
    public interface IMeterReadRepository : IAsyncRepository<MeterRead>
    {
        public Task<MeterRead> GetMeterReadIfExist(int accountId, DateTime meterReadDateTime, int readValue);
    }
}
