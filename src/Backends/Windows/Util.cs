// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenWindow.Backends.Windows
{
    internal static class Util
    {
        public static Message ToMessage(this Msg nativeMessage)
        {
            var m = new Message();
            
            switch (nativeMessage.message)
            {
                case WindowMessage.Quit:
                    m.Type = MessageType.Closing;
                    break;
                default:
                    m.Type = (MessageType) nativeMessage.message;
                    break;
            }

            return m;
        }

        public static Display DisplayFromMonitorHandle(IntPtr handle)
        {
            var mi = new MonitorInfo();
            mi.Prepare();
            if (!Native.GetMonitorInfo(handle, ref mi))
                throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());

            // niceify the display name
            var sb = new StringBuilder();
            var shouldCapitalize = true;
            var lastNotDigit = false;
            foreach (var c in mi.DeviceName)
            {
                if (char.IsPunctuation(c))
                    continue;

                if (char.IsLetter(c))
                {
                    if (shouldCapitalize)
                    {
                        sb.Append(char.ToUpperInvariant(c));
                        shouldCapitalize = false;
                    }
                    else
                    {
                        sb.Append(char.ToLowerInvariant(c));
                    }

                    lastNotDigit = true;
                }
                else if (char.IsDigit(c))
                {
                    if (lastNotDigit)
                        sb.Append(' ');
                    sb.Append(c);
                }
            }

            return new Display(sb.ToString(), mi.monitorRect, mi.workAreaRect, mi.IsPrimary);
        }

    }
}
