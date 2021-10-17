using System;
using System.Collections.Generic;

using System.Text;

namespace CoreApi.MeterData.Dal
{
    //[Table("Account")]
    public class Account
    {
        public int Id { get; set; }
        public int AccountId {get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
