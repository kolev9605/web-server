namespace WebServer.Server.Routing
{
    using Enums;
    using Handlers;
    using Routing.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using WebServer.Server.Http.Contracts;

    public class AppRouteConfig : IAppRouteConfig
    {
        private Dictionary<HttpRequestMethod, Dictionary<string, RequestHandler>> routes;

        public AppRouteConfig()
        {
            this.routes = new Dictionary<HttpRequestMethod, Dictionary<string, RequestHandler>>();

            var requestMethods = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>();

            foreach (var requestMethod in requestMethods)
            {
                this.routes.Add(requestMethod, new Dictionary<string, RequestHandler>());
            }
        }

        public IReadOnlyDictionary<HttpRequestMethod, Dictionary<string, RequestHandler>> Routes => this.routes;

        public void Get(string route, Func<IHttpRequest, IHttpResponse> func)
        {
            this.routes[HttpRequestMethod.Get].Add(route, new GetRequestHandler(func));
        }

        public void Post(string route, Func<IHttpRequest, IHttpResponse> func)
        {
            this.routes[HttpRequestMethod.Post].Add(route, new PostRequestHandler(func));
        }
    }
}