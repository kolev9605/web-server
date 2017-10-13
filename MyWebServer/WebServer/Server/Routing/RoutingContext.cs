namespace WebServer.Server.Routing
{
    using Common;
    using Handlers;
    using Routing.Contracts;
    using System.Collections.Generic;

    public class RoutingContext : IRoutingContext
    {
        public RoutingContext(IEnumerable<string> parameters, RequestHandler handler)
        {
            CoreValidator.ThrowIfNull(parameters, nameof(parameters));
            CoreValidator.ThrowIfNull(handler, nameof(handler));

            this.Parameters = parameters;
            this.RequestHandler = handler;
        }

        public IEnumerable<string> Parameters { get; private set; }

        public RequestHandler RequestHandler { get; private set; }
    }
}