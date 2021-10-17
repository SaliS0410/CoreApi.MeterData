using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreApi.MeterData.Dal
{
    public interface IAccountRepository : IAsyncRepository<Account>
    {
        Task<Account> GetAccountByAccountId(int accountId);
    }
}
