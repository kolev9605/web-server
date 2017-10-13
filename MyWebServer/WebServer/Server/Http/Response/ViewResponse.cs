namespace WebServer.Server.Http.Response
{
    using Server.Contracts;
    using System;
    using Enums;
    using Common;

    public class ViewResponse : HttpResponse
    {
        private readonly IView view;

        public ViewResponse(HttpResponseStatusCode statusCode, IView view)
        {
            CoreValidator.ThrowIfNull(view, nameof(view));
            ValidateStatusCode(statusCode);
            this.view = view;
            this.StatusCode = statusCode;
        }

        private void ValidateStatusCode(HttpResponseStatusCode statusCode)
        {
            if ((int)statusCode >= 300 && (int)statusCode < 400)
            {
                throw new InvalidOperationException("Status code must be below 300 and above 400 to return view.");
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}{this.view.View()}";
        }
    }
}
