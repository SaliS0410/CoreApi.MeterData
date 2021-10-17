using System;
using System.Collections.Generic;
using System.Text;

namespace CoreApi.MeterData.BL
{
    public class MeterReadPayloadResponse : BaseResponse
    {
        public string FileName { get; set; }

        public int SucceedReads { get; set; }

        public int InvalidReads { get; set; }
    }
}
