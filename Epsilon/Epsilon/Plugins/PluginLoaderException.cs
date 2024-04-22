using System;

namespace Host
{
    class PluginLoaderException : ApplicationException
    {
        public PluginLoaderException(string message) : base(message)
        {
        }

        public PluginLoaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
