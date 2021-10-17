using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreApi.MeterData.Dal
{
    [Table("MeterRead")]
    public class MeterRead
    {
        [Key]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public int ReadValue { get; set; }
    }
}
