namespace WebServer.Server.Handlers
{
    using Common;
    using Handlers.Contracts;
    using Http.Contracts;
    using Http.Response;
    using Routing.Contracts;
    using System.Text.RegularExpressions;

    public class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig serverRouteConfig;

        public HttpHandler(IServerRouteConfig serverRouteConfig)
        {
            CoreValidator.ThrowIfNull(serverRouteConfig, nameof(serverRouteConfig));

            this.serverRouteConfig = serverRouteConfig;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            foreach (var kvp in this.serverRouteConfig.Routes[context.Request.RequestMethod])
            {
                string pattern = kvp.Key;
                Regex regex = new Regex(pattern);
                Match match = regex.Match(context.Request.Path);

                if (!match.Success)
                {
                    continue;
                }

                foreach(var parameter in kvp.Value.Parameters)
                {
                    context.Request.AddUrlParameter(parameter, match.Groups[parameter].Value);
                }

                return kvp.Value.RequestHandler.Handle(context);
            }

            return new NotFoundResponse();
        }
    }
}
