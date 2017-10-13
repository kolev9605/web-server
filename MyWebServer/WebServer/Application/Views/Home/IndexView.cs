namespace WebServer.Application.Views.Home
{
    using Server.Contracts;

    public class IndexView : IView
    {
        public string View()
        {
            return @"<body>
<form method=""POST"">
    <input type=""text"" name=""name"" />
    <input type=""submit"" />
</form>
</body>";
        }
    }
}
