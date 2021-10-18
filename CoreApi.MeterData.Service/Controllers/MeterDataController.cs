using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using CoreApi.MeterData.BL;

namespace CoreApi.MeterData.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterDataController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MeterDataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut("meter-reading-uploads")]
        [SwaggerOperation(Summary = "Meter reads upload", Description = "Meter Reads will be saved and in response the number of failed/successful reads will be returned.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<MeterReadPayloadResponse>> ImportMeterDataPayLoad(MeterReadPayloadRequest payloadRequest)
        {
            var response = await _mediator.Send(payloadRequest);
            return Ok(response);
        }
    }
}
