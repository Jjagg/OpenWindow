// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal class Registry
    {
        [DllImport("libwayland-client.so", EntryPoint = "wl_registry_add_listener")]
        public static extern void AddListener(object registry, Structs.EventListener listener, object data);
    }
}