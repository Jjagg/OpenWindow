// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace OpenWindow
{
    public static class OpenWindow
    {
        internal static WindowingService Service { get; private set; }
        internal static bool Initialized { get; private set; }

        /// <summary>
        /// Initialize OpenWindow.
        /// </summary>
        public static void Initialize()
        {
            Service = DetermineService();

            Initialized = true;
        }

        private static WindowingService DetermineService()
        {
            return WindowingService.Windows;
        }
    }
}
