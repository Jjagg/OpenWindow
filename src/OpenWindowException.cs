using System;

namespace OpenWindow
{
    /// <summary>
    /// Exception thrown by OpenWindow when no built in exception is suitable.
    /// </summary>
    public class OpenWindowException : Exception
    {
        /// <inheritdoc />
        public OpenWindowException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public OpenWindowException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
