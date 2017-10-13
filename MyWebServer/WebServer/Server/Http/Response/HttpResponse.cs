namespace WebServer.Server.Http.Response
{
    using Enums;
    using System.Text;
    using Http.Contracts;

    public abstract class HttpResponse : IHttpResponse
    {
        private string statusMessage => this.StatusCode.ToString();

        protected HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
        }

        public HttpHeaderCollection Headers { get; protected set; }

        public HttpResponseStatusCode StatusCode { get; protected set; }

        public override string ToString()
        {
            StringBuilder response = new StringBuilder();

            response.AppendLine($"HTTP/1.1 {(int)this.StatusCode} {this.statusMessage}");
            response.AppendLine(this.Headers.ToString());
            response.AppendLine();

            return response.ToString();
        }
    }
}