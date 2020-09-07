using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Gobi.Chain.Core.Mediators;
using Gobi.Chain.Core.Mediators.Handlers;
using Gobi.Chain.Core.Tests.Mediators.Helpers;
using Xunit;

namespace Gobi.Chain.Core.Tests.Mediators
{
    public class ChainMediatorTests
    {
        private class TestRequest : IRequest<TestResponse>
        {
            public TestRequest(string payload) => Payload = payload;
            public string Payload { get; }
        }

        private class TestResponse
        {
            public TestResponse(string payload) => Payload = payload;

            public string Payload { get; }
        }

        [Fact]
        public async Task RequestAsync_Handler_Called()
        {
            // arrange
            StaticServiceFactory serviceFactory = new StaticServiceFactory();
            Mediator mediator = new Mediator(serviceFactory);
            DelegateRequestHandler<TestRequest, TestResponse> handler =
                new DelegateRequestHandler<TestRequest, TestResponse>(
                    Handle
                );

            Task<TestResponse> Handle(TestRequest request, CancellationToken ct)
            {
                return Task.FromResult(new TestResponse(request.Payload));
            }

            serviceFactory.AddService<IRequestHandler<TestRequest, TestResponse>>(
                handler);

            // act
            TestResponse response = await mediator.RequestAsync(new TestRequest("foo"));

            // assert
            response.Payload.Should().Be("foo");
        }
    }
}
