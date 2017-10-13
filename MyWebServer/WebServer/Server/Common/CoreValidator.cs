namespace WebServer.Server.Common
{
    using System;

    public static class CoreValidator
    {
        public static void ThrowIfNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ThrowIfNullOrEmpty(string word, string name)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentException($"The given value cannot be null or empty.", word);
            }
        }
    }
}