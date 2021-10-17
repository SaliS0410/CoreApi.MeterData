using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CoreApi.MeterData.Dal;

using System.Threading.Tasks;

namespace CoreApi.MeterData.RepositoryEf.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(MeterDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Account> GetAccountByAccountId(int accountId)
        {
            return await _dbContext.Account.FirstOrDefaultAsync(t=> t.AccountId == accountId);
        }
    }
}
