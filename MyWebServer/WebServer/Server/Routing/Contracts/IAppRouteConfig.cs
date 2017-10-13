namespace WebServer.Server.Routing.Contracts
{
    using System.Collections.Generic;
    using Enums;
    using Handlers;
    using System;
    using WebServer.Server.Http.Contracts;

    public interface IAppRouteConfig
    {
        IReadOnlyDictionary<HttpRequestMethod, Dictionary<string, RequestHandler>> Routes { get; }
        
        void Get(string route, Func<IHttpRequest, IHttpResponse> func);

        void Post(string route, Func<IHttpRequest, IHttpResponse> func);
    }
}