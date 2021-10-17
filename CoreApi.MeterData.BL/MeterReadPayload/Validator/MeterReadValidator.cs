using CoreApi.MeterData.Dal;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreApi.MeterData.BL.MeterReadPayload
{
    public class MeterReadValidator : AbstractValidator<MeterRead>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMeterReadRepository _meterReadRepository;
        public MeterReadValidator(IAccountRepository accountRepository, IMeterReadRepository meterReadRepository)
        {
            _accountRepository = accountRepository;
            _meterReadRepository = meterReadRepository;

            RuleFor(mr => mr.AccountId).NotEmpty().WithMessage("AccountId is required.").NotNull();
            RuleFor(mr => mr.ReadValue).NotEmpty().WithMessage("ReadValue is required.").NotNull().GreaterThan(9999).LessThan(100000);
        }
    }
}

