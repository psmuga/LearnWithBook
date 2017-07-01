using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter16
{
    class MockHandler : HttpMessageHandler
    {
        Func<HttpRequestMessage, HttpResponseMessage> _responseGenerator;

        public MockHandler
            (Func<HttpRequestMessage, HttpResponseMessage> responseGenerator)
        {
            _responseGenerator = responseGenerator;
        }

        protected override Task<HttpResponseMessage> SendAsync
            (HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var response = _responseGenerator(request);
            response.RequestMessage = request;
            return Task.FromResult(response);
        }
    }
}
