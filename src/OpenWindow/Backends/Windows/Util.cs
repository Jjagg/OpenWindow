using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenWindow.Backends.Windows
{
    internal static class Util
    {
        public static Display DisplayFromMonitorHandle(IntPtr handle)
        {
            var mi = new MonitorInfo();
            mi.Prepare();
            if (!Native.GetMonitorInfo(handle, ref mi))
                throw Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error());

            // niceify the display name
            // TODO document how this name is assigned
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
            return new Display(handle, sb.ToString(), mi.monitorRect, mi.workAreaRect, mi.IsPrimary);
        }

    }
}
