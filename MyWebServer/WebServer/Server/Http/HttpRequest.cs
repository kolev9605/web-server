namespace WebServer.Server.Http
{
    using System.Collections.Generic;
    using Enums;
    using Contracts;
    using System;
    using Common;
    using Exceptions;
    using System.Net;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.Headers = new HttpHeaderCollection();
            this.UrlParameters = new Dictionary<string, string>();
            this.QueryParameters = new Dictionary<string, string>();
            this.FormData = new Dictionary<string, string>();

            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.ParseRequest(requestString);
        }

        public IDictionary<string, string> FormData { get; private set; }

        public HttpHeaderCollection Headers { get; private set; }

        public string Path { get; private set; }

        public IDictionary<string, string> QueryParameters { get; private set; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public string Url { get; private set; }

        public IDictionary<string, string> UrlParameters { get; private set; }

        public void AddUrlParameter(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            this.UrlParameters[key] = value;
        }

        private void ParseRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            var requestLines = requestString
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var requestLine = requestLines[0]
                .Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (requestLine.Length != 3 || requestLine[2].ToLower() != "http/1.1")
            {
                throw new BadRequestException("The request line is not valid.");
            }

            this.RequestMethod = ParseRequestMethod(requestLine[0].Trim().ToUpper());
            this.Url = requestLine[1];
            this.Path = this.Url
                .Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];

            this.ParseHeaders(requestLines);
            this.ParseParameters();

            if (this.RequestMethod == HttpRequestMethod.Post)
            {
                this.ParseQuery(requestLines[requestLines.Length - 1], this.FormData);
            }
        }

        private void ParseHeaders(string[] requestLines)
        {
            int endIndex = Array.IndexOf(requestLines, string.Empty);
            for (int i = 1; i < endIndex; i++)
            {
                var headerArgs = requestLines[i].Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);

                if (headerArgs.Length != 2)
                {
                    throw new BadRequestException("Invalid header.");
                }

                var headerKey = headerArgs[0];
                var headerValue = headerArgs[1];

                CoreValidator.ThrowIfNullOrEmpty(headerKey, nameof(headerKey));
                CoreValidator.ThrowIfNullOrEmpty(headerValue, nameof(headerValue));

                HttpHeader header = new HttpHeader(headerKey, headerValue);

                this.Headers.Add(header);
            }

            if (!this.Headers.ContainsKey("Host"))
            {
                throw new InvalidHeadersException("There is no Host header.");
            }
        }

        private void ParseParameters()
        {
            if (!this.Url.Contains("?"))
            {
                return;
            }

            var queryStrings = this.Url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
            if(queryStrings.Length < 2)
            {
                return;
            }

            var queryString = queryStrings[1];
            this.ParseQuery(queryString, this.QueryParameters);
        }

        private void ParseQuery(string queryString, IDictionary<string, string> queryParameters)
        {
            if (!queryString.Contains("="))
            {
                return;
            }

            var queryPairs = queryString.Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var queryPair in queryPairs)
            {
                var queryArgs = queryPair.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (queryArgs.Length != 2)
                {
                    continue;
                }

                var queryKey = WebUtility.UrlDecode(queryArgs[0]);
                var queryValue = WebUtility.UrlDecode(queryArgs[1]);

                queryParameters[queryKey] = queryValue;
            }
        }

        private HttpRequestMethod ParseRequestMethod(string requestMethod)
        {
            HttpRequestMethod parsedRequestMethod;

            try
            {
                parsedRequestMethod = (HttpRequestMethod)Enum.Parse(typeof(HttpRequestMethod), requestMethod, true);
            }
            catch (Exception)
            {
                throw new BadRequestException("The http request is not valid.");
            }

            return parsedRequestMethod;
        }
    }
}