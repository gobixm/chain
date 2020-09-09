using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Gobi.Chain.Core.Mediators;
using Gobi.Chain.Core.Mediators.Handlers;
using Gobi.Chain.Core.Mediators.Middlewares;
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

        private class TestContext
        {
            public List<string> Calls { get; } = new List<string>();
        }

        [Fact]
        public async Task RequestAsync_Handler_Called()
        {
            // arrange
            StaticServiceFactory serviceFactory = new StaticServiceFactory();
            Mediator<TestContext> mediator = new Mediator<TestContext>(serviceFactory);
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

        [Fact]
        public async Task RequestAsync_Middlewares_Called()
        {
            // arrange
            TestContext sharedContext = null;
            StaticServiceFactory serviceFactory = new StaticServiceFactory();
            Mediator<TestContext> mediator = new Mediator<TestContext>(
                serviceFactory,
                configuration => configuration
                    .AddMiddleware(
                        () => new DelegateMiddleware<TestContext>(async (context, next, ct) =>
                        {
                            context.Calls.Add("pre-outer");
                            await next();
                            context.Calls.Add("post-outer");
                            sharedContext = context;
                        })
                    ).AddMiddleware(
                        () => new DelegateMiddleware<TestContext>(async (context, next, ct) =>
                        {
                            context.Calls.Add("pre-inner");
                            await next();
                            context.Calls.Add("post-inner");
                            sharedContext = context;
                        })
                    )
            );
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
            await mediator.RequestAsync(new TestRequest("foo"));

            // assert
            sharedContext.Should().NotBeNull();
            sharedContext.Calls.Should().BeEquivalentTo(new List<string>
            {
                "pre-outer", "pre-inner", "post-inner", "post-outer"
            });
        }
    }
}
