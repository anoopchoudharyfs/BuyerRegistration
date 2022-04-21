using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BuyerRegistration.Api.Services.IService;
using BuyerRegistration.Domain;
using BuyerRegistration.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace BuyerRegistration.Specs.Steps
{
    [Binding]
    public class SharedSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<Clock> _mockClock;
        private readonly Mock<ICorrelationIdProvider> _correlationIdProvider;

        private HttpResponseMessage GetResponse() { return _scenarioContext["Response"] as HttpResponseMessage; }
      

        public SharedSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _mockClock = new Mock<Clock>();
            _correlationIdProvider = new Mock<ICorrelationIdProvider>();
        }

        [Given(@"my request body is")]
        public void GivenMyRequestBodyIs(string multilineText)
        {
            _scenarioContext["RequestBody"] = multilineText;
        }

        [Given(@"the blob will be stored at (.*)")]
        public void GivenMyPersistedBlobIs(string uri)
        {
            _scenarioContext["BlobUri"] = uri;
        }

        [Then(@"response should be 200 OK")]
        public void ThenResponseShouldBeOK()
        {
            GetResponse().StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Then(@"response should be 202 Accepted")]
        public void ThenResponseShouldBeAccepted()
        {
            GetResponse().StatusCode.Should().Be(HttpStatusCode.Accepted);
        }

        [Then(@"response should be 400 Bad Request")]
        public void ThenResponseShouldBeBadRequest()
        {
            GetResponse().StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Then(@"response should be 500 Internal Server error")]
        public void ThenResponseShouldBeInternalServerError()
        {
            GetResponse().StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Then(@"the response contains only these validation errors")]
        public async Task TheResponseShouldHaveTheseValidationErrors(Table table)
        {
            var expectedValidationResults = table.CreateSet<ValidationError>().ToList();

            var jsonBody = await GetResponse().Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<ModelBindingValidationError>(jsonBody);

            var actualValidationResultsDictionary = responseBody.ValidationResults.ToDictionary(x => x.Path);

            foreach (var expected in expectedValidationResults)
            {
                actualValidationResultsDictionary.Should().ContainKey(expected.Path);

                var actual = actualValidationResultsDictionary[expected.Path];

                actual.Value.Should().Be(expected.Value);
                actual.Path.Should().Be(expected.Path);
                actual.Description.Should().Contain(expected.Description);
            }

            actualValidationResultsDictionary.Keys.Count.Should().Be(expectedValidationResults.Count);

        }

        [Then(@"the response contains only these validation errors with error codes")]
        public async Task TheResponseShouldHaveTheseValidationWithUniqueCodes(Table table)
        {
            var expectedValidationResults = table.CreateSet<ValidationError>().ToList();

            var jsonBody = await GetResponse().Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<ModelBindingValidationError>(jsonBody);

            var actualValidationResultsDictionary = responseBody.ValidationResults.ToDictionary(x => x.Code);

            foreach (var expected in expectedValidationResults)
            {
                actualValidationResultsDictionary.Should().ContainKey(expected.Code);

                var actual = actualValidationResultsDictionary[expected.Code];

                actual.Value.Should().Be(expected.Value);
                actual.Code.Should().Be(expected.Code);
                actual.Description.Should().Be(expected.Description);

                if (!string.IsNullOrEmpty(expected.Path))
                {
                    actual.Path.Should().Be(expected.Path);
                }
            }
            actualValidationResultsDictionary.Keys.Count.Should().Be(expectedValidationResults.Count);
        }
      

        [Given(@"the current time is (.*)Z")]
        public void GivenTheCurrentTimeIs_Z(DateTime dateTime)
        {
            _mockClock.Setup(x => x.GetCurrentUtcDateTime()).Returns(dateTime);
            _scenarioContext.Add("MockClock", _mockClock);
        }

        [Given(@"the correlation id is ""(.*)""")]
        public void GivenTheCorrelationIdIs(string correlationId)
        {
            _correlationIdProvider.Setup(x => x.GetCorrelationId()).Returns(correlationId);
            _scenarioContext.Add("CorrelationId", _correlationIdProvider);
        }

       

        [StepArgumentTransformation]
        public long[] TransformToListOfLongs(string commaSeparatedList)
        {
            return commaSeparatedList.Split(",").Select(Int64.Parse).ToArray();
        }
        [Given(@"my required headers")]
        public void GivenMyRequiredHeaders(Table table)
        {
            var headers = table.CreateSet<Header>().ToList();
            _scenarioContext["Headers"] = headers;
        }


    }

    public class Header
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

}
