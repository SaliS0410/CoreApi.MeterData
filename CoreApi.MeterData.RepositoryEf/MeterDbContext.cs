using CoreApi.MeterData.Dal;
using Microsoft.EntityFrameworkCore;
using System;

namespace CoreApi.MeterData.RepositoryEf
{
    public class MeterDbContext : DbContext
    {
        public MeterDbContext(DbContextOptions<MeterDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        
        //entities
        public DbSet<MeterRead> MeterRead { get; set; }
        public DbSet<Account> Account{ get; set; }
    }
}
