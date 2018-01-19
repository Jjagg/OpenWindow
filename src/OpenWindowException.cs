// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

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