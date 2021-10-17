using System;
using System.Collections.Generic;
using System.Text;

namespace CoreApi.MeterData.BL
{
    public class BaseResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }

        public List<string> ValidationErrors { get; set; }
    }
}
