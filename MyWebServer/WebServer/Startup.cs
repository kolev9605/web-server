namespace WebServer
{
    using Server;
    using Server.Contracts;
    using Server.Routing;
    using Server.Routing.Contracts;
    using WebServer.Application;

    public class Startup : IRunnable
    {
        private MyWebServer server;

        public static void Main()
        {
            new Startup().Run();
        }

        public void Run()
        {
            IApplication application = new MainApplication();
            IAppRouteConfig routeConfig = new AppRouteConfig();
            application.Configurate(routeConfig);
            this.server = new MyWebServer(1337, routeConfig);
            this.server.Run();
        }
    }
}