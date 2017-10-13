namespace WebServer.Application.Controllers
{
    using Views.Home;
    using Server.Enums;
    using Server.Http.Contracts;
    using Server.Http.Response;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            return new ViewResponse(HttpResponseStatusCode.Ok, new IndexView());
        }

        public IHttpResponse Index(string name)
        {
            return new RedirectResponse("/");
        }
    }
}
