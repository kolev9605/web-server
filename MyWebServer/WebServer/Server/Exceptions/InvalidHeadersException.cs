namespace WebServer.Server.Exceptions
{
    using System;

    public class InvalidHeadersException : Exception
    {
        public InvalidHeadersException(string message) 
            : base(message)
        {
        }
    }
}