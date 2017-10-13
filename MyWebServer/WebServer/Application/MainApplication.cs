namespace WebServer.Application
{
    using Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;
    using Server.Handlers;

    class MainApplication : IApplication
    {
        public void Configurate(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.Get("/", context => new HomeController().Index());

            appRouteConfig.Post("/", context => new HomeController().Index(context.FormData["name"]));
        }
    }
}
