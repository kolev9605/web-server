namespace WebServer.Server
{
    using Server.Routing;
    using Server.Routing.Contracts;
    using Server.Contracts;
    using System.Net;
    using System.Net.Sockets;
    using System;
    using System.Threading.Tasks;

    public class MyWebServer : IRunnable
    {
        private const string LocalhostIpAddress = "127.0.0.1";

        private readonly int port;
        private readonly IServerRouteConfig serverRouteConfig;
        private readonly TcpListener tcpListener;
        private bool isRunning;

        public MyWebServer(int port, IAppRouteConfig appRouteConfig)
        {
            this.port = port;
            this.tcpListener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), port);

            this.serverRouteConfig = new ServerRouteConfig(appRouteConfig);
        }

        public void Run()
        {
            this.tcpListener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server started: Listening to TCP clients at {LocalhostIpAddress}:{this.port}");

            Task task = Task.Run(this.ListenLoop);
            task.Wait();
        }
        
        private async Task ListenLoop()
        {
            while (this.isRunning)
            {
                Socket client = await this.tcpListener.AcceptSocketAsync();
                ConnectionHandler connectionHandler = new ConnectionHandler(client, this.serverRouteConfig);
                Task connection = connectionHandler.ProcessRequestAsync();
                connection.Wait();
            }
        }
    }
}