namespace WebServer.Server.Http.Response
{
    public class NotFoundResponse : HttpResponse
    {
        public NotFoundResponse()
        {
            this.StatusCode = Enums.HttpResponseStatusCode.NotFound;
        }
    }
}
