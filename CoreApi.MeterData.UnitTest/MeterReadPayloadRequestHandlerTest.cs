using AutoMapper;
using CoreApi.MeterData.BL;
using CoreApi.MeterData.Dal;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CoreApi.MeterData.UnitTest
{
    public class MeterReadPayloadRequestHandlerTest
    {
        private readonly Mock<IAccountRepository> _mockAccountRepo;
        private readonly Mock<IMeterReadRepository> _mockMeterReadRepo;

        private readonly MeterReadPayloadRequestHandler _meterReadPayloadRequestHandler;
        private readonly IMapper _mapper;

        public MeterReadPayloadRequestHandlerTest()
        {
            _mockAccountRepo = new Mock<IAccountRepository>();
            _mockMeterReadRepo = new Mock<IMeterReadRepository>();

            _meterReadPayloadRequestHandler = new MeterReadPayloadRequestHandler(_mapper, _mockAccountRepo.Object, _mockMeterReadRepo.Object);

        }
        [Fact]
        public async Task Test_EmptyPaylaod()
        {
            var request = new MeterReadPayloadRequest();
            request.FileName = "";
            request.MeterReadsPayload = "";
            var response = await _meterReadPayloadRequestHandler.Handle(request, CancellationToken.None);
            Assert.True(response.Status == "Failed");
            Assert.True(response.SucceedReads == 0);
            Assert.True(response.ValidationErrors.Any());
        }

        [Fact]
        public async Task Test_MeterReadExistPaylaod()
        {
            int accountId = 2344;
            //Arrange
            var account = new Account() { 
                AccountId = 2344,
                FirstName = "Tommy",
                LastName = "Test"
            };

            DateTime meterReadingDateTime = new DateTime(2019, 04, 22, 9, 24, 00);
            int readValue = 10023;
            var meterRead = new MeterRead()
            {
                AccountId = accountId,
                MeterReadingDateTime = meterReadingDateTime,
                ReadValue = readValue
            };

            _mockAccountRepo.Setup(repo => repo.GetAccountByAccountId(accountId)).ReturnsAsync(account);

            _mockMeterReadRepo.Setup(repo => repo.GetMeterReadIfExist(accountId, meterReadingDateTime, readValue)).ReturnsAsync(meterRead);

            var request = new MeterReadPayloadRequest();
            request.FileName = "MeterRead_17Oct.csv";
            request.MeterReadsPayload = "AccountId,MeterReadingDateTime,MeterReadValue \n2344,22-04-2019 9.24,10023";
            var response = await _meterReadPayloadRequestHandler.Handle(request, CancellationToken.None);
            Assert.True(response.Status == "Failed");
            Assert.True(response.SucceedReads == 0);
            Assert.True(response.InvalidReads == 1);
        }


        [Theory]
        [InlineData("2233,22-04-2019 12.25,32356", 2233, true)]
        [InlineData("2050,22-04-2019 12.25,326", 2050, false)]
        [InlineData("2233,22-04-201912.25,32356", 2233, false)]
        [InlineData("2050,22-04-2019 12.25,Xyz6", 2050, false)]
        public async Task Test_AccountPaylaod(string payload, int accountId, bool isValid)
        {
            //Arrange
            var account = new Account()
            {
                AccountId = accountId,
                FirstName = "FirstTest",
                LastName = "LastTest"
            };

            _mockAccountRepo.Setup(repo => repo.GetAccountByAccountId(accountId)).ReturnsAsync(account);

            var request = new MeterReadPayloadRequest();
            request.FileName = "MeterRead_17Oct.csv";
            request.MeterReadsPayload = "AccountId,MeterReadingDateTime,MeterReadValue" + "\n" + payload;
            var response = await _meterReadPayloadRequestHandler.Handle(request, CancellationToken.None);
            if (isValid)
            {
                Assert.True(response.Status == "Created");
                Assert.True(response.SucceedReads == 1);
                Assert.True(response.InvalidReads == 0);
            }
            else
            {
                Assert.True(response.Status == "Failed");
                Assert.True(response.SucceedReads == 0);
                Assert.True(response.InvalidReads ==1);
            }
        }

        [Fact]
        public async Task Test_ValidMultiplePaylaod()
        {
            //Arrange
            var accountId = 2345;
            var account = new Account()
            {
                AccountId = accountId,
                FirstName = "FirstTest",
                LastName = "LastTest"
            };

            _mockAccountRepo.Setup(repo => repo.GetAccountByAccountId(accountId)).ReturnsAsync(account);

            var request = new MeterReadPayloadRequest();
            request.FileName = "MeterRead_17Oct.csv";
            request.MeterReadsPayload = "AccountId,MeterReadingDateTime,MeterReadValue" + "\n" + "2345,22-04-2019 12.25,45522" + "\n" + "2345,23-04-2019 12.25,18212" + "\n" + "2345,24-04-2019 9.24,0X765";
            var response = await _meterReadPayloadRequestHandler.Handle(request, CancellationToken.None);
            Assert.True(response.Status == "Created");
            Assert.True(response.SucceedReads == 2);
            Assert.True(response.InvalidReads == 1);
            
        }
    }
}
