namespace WebServer.Server.Http.Contracts
{
    using Enums;

    public interface IHttpResponse
    {
        HttpHeaderCollection Headers { get; }

        HttpResponseStatusCode StatusCode { get; }
    }
}