using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreApi.MeterData.BL
{
    public class MeterReadPayloadValidator : AbstractValidator<MeterReadPayloadRequest>
    {
        public MeterReadPayloadValidator()
        {
            RuleFor(req => req.FileName).NotEmpty().WithMessage("File Name is required.").NotNull();

            RuleFor(req => req.MeterReadsPayload).NotEmpty().WithMessage("Meter Reads cannnot be blank.").NotNull();
        }
    }
}
