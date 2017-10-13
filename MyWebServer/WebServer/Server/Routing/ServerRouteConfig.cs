namespace WebServer.Server.Routing
{
    using Enums;
    using Routing.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    class ServerRouteConfig : IServerRouteConfig
    {
        public ServerRouteConfig(IAppRouteConfig appRouteConfig)
        {
            this.Routes = new Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>>();

            var availableMethods = Enum
               .GetValues(typeof(HttpRequestMethod))
               .Cast<HttpRequestMethod>();

            foreach (var method in availableMethods)
            {
                this.Routes[method] = new Dictionary<string, IRoutingContext>();
            }

            this.InitializeServerConfig(appRouteConfig);
        }

        private void InitializeServerConfig(IAppRouteConfig appRouteConfig)
        {
            foreach (var route in appRouteConfig.Routes)
            {
                foreach (var requestHandler in route.Value)
                {

                    List<string> args = new List<string>();

                    string parsedRegex = this.ParseRoute(requestHandler.Key, args);

                    IRoutingContext routingContext = new RoutingContext(args, requestHandler.Value);

                    this.Routes[route.Key].Add(parsedRegex, routingContext);
                }
            }
        }

        private string ParseRoute(string requestHandlerKey, List<string> args)
        {
            StringBuilder parsedRegex = new StringBuilder();
            parsedRegex.Append("^");
            if (requestHandlerKey == "/")
            {
                parsedRegex.Append($"{requestHandlerKey}$");
                return parsedRegex.ToString();
            }

            var tokens = requestHandlerKey.Split('/');
            this.ParseTokens(args, tokens, parsedRegex);

            return parsedRegex.ToString();
        }

        private void ParseTokens(List<string> args, string[] tokens, StringBuilder parsedRegex)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                string end = i == tokens.Length - 1 ? "?" : "/";
                if (!tokens[i].StartsWith("{") && !tokens[i].EndsWith("}"))
                {
                    parsedRegex.Append($"{tokens[i]}{end}");
                    continue;
                }

                string pattern = "<\\w>";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(tokens[i]);

                if (!match.Success)
                {
                    continue;
                }

                string paramName = match
                    .Groups[0].Value
                    .Substring(1, match.Groups[0].Length - 2);

                args.Add(paramName);
                parsedRegex.Append($"{tokens[i].Substring(1, tokens[i].Length - 2)}{end}");
            }
        }

        public Dictionary<HttpRequestMethod, Dictionary<string, IRoutingContext>> Routes { get; private set; }
    }
}
