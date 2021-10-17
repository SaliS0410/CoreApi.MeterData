using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreApi.MeterData.BL
{
    public class MeterReadPayloadRequest : IRequest<MeterReadPayloadResponse>
    {
        public string FileName { get; set; }
        public string MeterReadsPayload { get; set; }
    }
}
