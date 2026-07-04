using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modules.Conversations.Infrastructure.Delegates
{
    public sealed class LoggingHandler : DelegatingHandler
    {
        public LoggingHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            Console.WriteLine("=== REQUEST ===");
            Console.WriteLine($"{request.Method} {request.RequestUri}");

            foreach (var header in request.Headers)
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");

            if (request.Content != null)
            {
                foreach (var header in request.Content.Headers)
                    Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");

                Console.WriteLine(await request.Content.ReadAsStringAsync(cancellationToken));
            }

            var response = await base.SendAsync(request, cancellationToken);

            Console.WriteLine("=== RESPONSE ===");
            Console.WriteLine($"{(int)response.StatusCode} {response.StatusCode}");

            foreach (var header in response.Headers)
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");

            if (response.Content != null)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync(cancellationToken));
            }

            return response;
        }
    }
}