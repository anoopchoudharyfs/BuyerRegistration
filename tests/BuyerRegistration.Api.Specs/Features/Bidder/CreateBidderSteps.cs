using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BuyerRegistration.Api.Specs.MockConfiguration;
using BuyerRegistration.Api.Validators;
using BuyerRegistration.Domain;
using BuyerRegistration.Specs.Features;
using BuyerRegistration.Specs.MockConfiguration;
using FluentAssertions;
using Moq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using TechTalk.SpecFlow.Infrastructure;

namespace BuyerRegistration.Api.Specs.Features.Bidder
{
    [Binding]
    public class CreateBidderSteps
    {
        private readonly CustomWebApplicationFactory<MockStartup> _factory;
        private readonly ISpecFlowOutputHelper _outputHelper;
        private ScenarioContext _scenarioContext;
        private Mock<IBidderRepository> _bidderRepository;
        private string RequestBody { get { return _scenarioContext["RequestBody"].ToString(); } set { _scenarioContext["RequestBody"] = value; } }
        
        public CreateBidderSteps(CustomWebApplicationFactory<MockStartup> factory, ISpecFlowOutputHelper outputHelper, ScenarioContext scenarioContext)
        {
            _factory = factory;
            _scenarioContext = scenarioContext;
            _bidderRepository = new Mock<IBidderRepository>();
            _bidderRepository.Setup(x => x.Persist(It.IsAny<BidderData>()));            
        }

        [When(@"you send a put bidder request to (.*)")] 
        public async Task WhenYouSendACreateBidderRequestT(string url)
        {
            var httpClient = GetClient(_factory);
            _scenarioContext["Response"] = await httpClient.PutAsync(url, new StringContent(RequestBody, System.Text.Encoding.UTF8, "application/json"));
        }

        [Then(@"the bidder should be persisted in the database as")]
        public void ThenTheBidderIsPersistedInTheDatabase(Table table)
        {
            var bidderDataExpected = table.CreateInstance<BidderData>();
            _bidderRepository.Verify(   x => x.Persist( It.Is<BidderData>(b => CheckBidderData(b, bidderDataExpected))), Times.Once    );
        }

        private static bool CheckBidderData(BidderData x, BidderData bidderDataExpected)
        {
            x.Id.Should().Be(bidderDataExpected.Id);
            x.PartitionKey.Should().Be(bidderDataExpected.PartitionKey);
            x.CustomerId.Should().Be(bidderDataExpected.CustomerId);
            x.BuyerId.Should().Be(bidderDataExpected.BuyerId);
            x.MarketIdentityCode.Should().Be(bidderDataExpected.MarketIdentityCode);
            x.TenderHouseId.Should().Be(bidderDataExpected.TenderHouseId);
            x.TenderId.Should().Be(bidderDataExpected.TenderId);
            x.Status.Should().Be(bidderDataExpected.Status);
            x.Action.Should().Be(bidderDataExpected.Action);            
            return true;
        }

        private HttpClient GetClient(CustomWebApplicationFactory<MockStartup> factory)
        {
            var specsHttpClientFactory = new SpecsHttpClientFactory();
            specsHttpClientFactory.AddStub(_bidderRepository.Object);
            return specsHttpClientFactory.GetClient(factory, _scenarioContext);
        }
    }
}
