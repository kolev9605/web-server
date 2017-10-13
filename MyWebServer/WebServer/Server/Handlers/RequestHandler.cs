namespace WebServer.Server.Handlers
{
    using System;
    using Handlers.Contracts;
    using Http;
    using Http.Contracts;

    public abstract class RequestHandler : IRequestHandler
    {
        private readonly Func<IHttpRequest, IHttpResponse> func;

        protected RequestHandler(Func<IHttpRequest, IHttpResponse> func)
        {
            this.func = func;
        }

        public IHttpResponse Handle(IHttpContext context)
        {
            IHttpResponse response = func(context.Request);

            response.Headers.Add(new HttpHeader("Content-Type", "text/html"));

            return response;
        }
    }
}
