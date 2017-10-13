namespace WebServer.Server
{
    using Common;
    using Handlers;
    using Http;
    using Http.Contracts;
    using Routing.Contracts;
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class ConnectionHandler
    {
        private readonly Socket client;
        private readonly IServerRouteConfig serverRouteConfig;

        public ConnectionHandler(Socket client, IServerRouteConfig serverRouteConfig)
        {
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(serverRouteConfig, nameof(serverRouteConfig));

            this.client = client;
            this.serverRouteConfig = serverRouteConfig;
        }

        public async Task ProcessRequestAsync()
        {
            string request = await this.ReadRequest();

            if (request != null)
            {

                IHttpContext httpContext = new HttpContext(request);

                IHttpResponse httpResponse = new HttpHandler(this.serverRouteConfig).Handle(httpContext);

                ArraySegment<byte> toBytes = new ArraySegment<byte>(Encoding.UTF8.GetBytes(httpResponse.ToString()));

                await this.client.SendAsync(toBytes, SocketFlags.None);

                Console.WriteLine("-----------Request-----------");
                Console.WriteLine(request);
                Console.WriteLine("-----------Response-----------");
                Console.WriteLine(httpResponse.ToString());
            }

            this.client.Shutdown(SocketShutdown.Both);
        }

        private async Task<string> ReadRequest()
        {
            var request = new StringBuilder();
            ArraySegment<byte> data = new ArraySegment<byte>(new byte[1024]);

            int numBytesRead;

            while(true)
            {
                numBytesRead = await this.client.ReceiveAsync(data, SocketFlags.None);
                if(numBytesRead == 0)
                {
                    break;
                }

                request.Append(Encoding.UTF8.GetString(data.Array, 0, numBytesRead));
                if(numBytesRead <= 1024)
                {
                    break;
                }
            }

            if(request.Length == 0)
            {
                return null;
            }

            return request.ToString();
        }
    }
}