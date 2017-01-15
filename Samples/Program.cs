// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using OpenWindow;
using System.Diagnostics;
using System.Threading;

namespace Windows
{
    class Program
    {
        static void Main(string[] args)
        {
            var window = Window.Create(100, 100, 200, 200);

            while (true)
            {
                var message = window.GetMessage();

                if (message.Type == MessageType.Closing)
                    break;
            }
        }
    }
}
