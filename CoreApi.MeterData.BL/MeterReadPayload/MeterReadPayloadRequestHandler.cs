using AutoMapper;
using CoreApi.MeterData.Dal;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace CoreApi.MeterData.BL
{
    public class MeterReadPayloadRequestHandler : IRequestHandler<MeterReadPayloadRequest, MeterReadPayloadResponse>
    {
        private static readonly int AccountFileIndex = 0;
        private static readonly int MeterReadingDateTimeFileIndex = 1;
        private static readonly int ReadValueFileIndex = 2;
        private static readonly int FileColsCount = 3;

        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;
        private readonly IMeterReadRepository _meterReadRepository;

        private int succeedReads = 0;
        private int totalReads = 0;

        public MeterReadPayloadRequestHandler(IMapper mapper, IAccountRepository accountRepository, IMeterReadRepository meterReadRepository)
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _meterReadRepository = meterReadRepository;
        }

        public async Task<MeterReadPayloadResponse> Handle(MeterReadPayloadRequest request, CancellationToken cancellationToken)
        {
            var payloadResponse = new MeterReadPayloadResponse() { FileName = request.FileName};
            var payloadValidator = new MeterReadPayloadValidator();
            var validationResult = await payloadValidator.ValidateAsync(request);

            if (validationResult.Errors.Count > 0)
            {
                payloadResponse.Status = "Failed";
                payloadResponse.ValidationErrors = new List<string>(validationResult.Errors.Select(t => t.ErrorMessage));
                return payloadResponse;
            }
            var errors = new List<string>();

            var meterReads = await ParsePayloadAsync(request);

            // Get valid reads 
            if (meterReads.Any())
            {
                meterReads.ForEach(t => _meterReadRepository.AddAsync(t));
            }

            // convert thme to MeterReads and save
            payloadResponse.SucceedReads = succeedReads;
            payloadResponse.InvalidReads = totalReads - succeedReads;
            payloadResponse.Status = (succeedReads > 0) ? "Created" : "Failed";
            return payloadResponse;
        }

        private async Task<List<MeterRead>> ParsePayloadAsync(MeterReadPayloadRequest request)
        {
            succeedReads = 0;
            totalReads = 0;
            var meterReads = new List<MeterRead>();

            if (string.IsNullOrEmpty(request.MeterReadsPayload) || !request.MeterReadsPayload.Contains(","))
                return meterReads;

            foreach (var line in request.MeterReadsPayload.Split("\n"))
            {
                //skip header line
                if (line.Contains("AccountId"))
                {
                    continue;
                }
                totalReads++;
                var meterRead = await TryParsePayloadLine(line);
                if (meterRead != null)
                {
                    meterReads.Add(meterRead);
                    succeedReads++;
                }
            }
            return meterReads;

        }

        private async Task<MeterRead> TryParsePayloadLine(string line)
        {
            var meterreadStr = line.CsvLineToList();

            if (meterreadStr.Count() != FileColsCount ||
                string.IsNullOrEmpty(meterreadStr[AccountFileIndex]) ||
                string.IsNullOrEmpty(meterreadStr[MeterReadingDateTimeFileIndex]) ||
                string.IsNullOrEmpty(meterreadStr[ReadValueFileIndex]) ||
                (meterreadStr[ReadValueFileIndex].Length != 5))
            {
                return null;
            }

            int accountId;
            DateTime meterReadingDateTime;
            int readValue;

            if (!int.TryParse(meterreadStr[AccountFileIndex], out accountId) || 
                !DateTime.TryParse(meterreadStr[MeterReadingDateTimeFileIndex], out meterReadingDateTime) || 
                !int.TryParse(meterreadStr[ReadValueFileIndex], out readValue))
            {
                return null;
            }

            // check if account exist
            var account = await _accountRepository.GetAccountByAccountId(accountId);
            if (account == null)
            {
                return null;
            }
            
            // Check if read already exists
            MeterRead meterRead = await _meterReadRepository.GetMeterReadIfExist(accountId, meterReadingDateTime, readValue);
            if (meterRead != null)
            {
                return null;
            }

            meterRead = new MeterRead()
            {
                AccountId = accountId,
                MeterReadingDateTime = meterReadingDateTime,
                ReadValue = readValue
            };
            return meterRead;
        }
    }
}

