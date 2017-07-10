// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace OpenWindow
{
    public class OpenWindowException : Exception
    {
        public OpenWindowException(string message) : base(message)
        {
        }

        public OpenWindowException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}