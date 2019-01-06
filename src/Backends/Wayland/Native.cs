using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal unsafe static class WaylandClient
    {
        [DllImport("libwayland-client.so")]
        public static extern wl_display* wl_display_connect(byte* strConn);

        [DllImport("libwayland-client.so")]
        public static extern void wl_display_disconnect(wl_display* dpy);

        [DllImport("libwayland-client.so")]
        public static extern int wl_display_roundtrip(wl_display* dpy);
        
        [DllImport("libwayland-client.so")]
        public static extern int wl_display_flush(wl_display* dpy);
        
        [DllImport("libwayland-client.so")]
        public static extern int wl_display_dispatch(wl_display* dpy);
        
        [DllImport("libwayland-client.so")]
        public static extern int wl_display_dispatch_pending(wl_display* dpy);

        [DllImport("libwayland-client.so")]
        public static extern int wl_display_get_error(wl_display* dpy);
 

        //public static extern IntPtr Create(IntPtr proxy);
        [DllImport("libwayland-client.so")]
        public static extern void wl_proxy_destroy(wl_proxy* proxy);
        
        //[DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_dispatcher")]
        //public static extern void AddDispatcher();
        
        //[DllImport("libwayland-client.so", EntryPoint = "wl_proxy_get_class")]
        //public static extern IntPtr GetClass(IntPtr proxy);
        
        //[DllImport("libwayland-client.so", EntryPoint = "wl_proxy_get_id")]
        //public static extern int GetId(IntPtr proxy);
        
        [DllImport("libwayland-client.so")]
        public static extern int wl_proxy_add_listener(wl_proxy* proxy, void* listener, void* data);

        [DllImport("libwayland-client.so")]
        public static extern void* wl_proxy_get_listener(wl_proxy* proxy);
        
        [DllImport("libwayland-client.so")]
        private static extern void* wl_proxy_get_user_data(wl_proxy* proxy);
        [DllImport("libwayland-client.so")]
        public static extern void wl_proxy_set_user_data(wl_proxy* proxy, void* data);
        
        [DllImport("libwayland-client.so")]
        public static extern uint wl_proxy_get_version(wl_proxy* proxy);
        
        [DllImport("libwayland-client.so")]
        public static extern void wl_proxy_marshal(wl_proxy* proxy, int opcode);
        
        [DllImport("libwayland-client.so")]
        public static extern void wl_proxy_marshal_array(wl_proxy* proxy, int opcode, wl_argument* data);
        
        [DllImport("libwayland-client.so")]
        public static extern wl_proxy* wl_proxy_marshal_constructor(wl_proxy* proxy, int opcode, wl_interface* iface, void* data);
        //[DllImport("libwayland-client.so")]
        //public static extern wl_proxy* wl_proxy_marshal_constructor_versioned(wl_proxy* proxy, int opcode, wl_interface* iface,
        //    uint version, uint name, uint ifaceName, uint ifaceVersion, IntPtr data);
        
        [DllImport("libwayland-client.so")]
        public static extern wl_proxy* wl_proxy_marshal_array_constructor(wl_proxy* proxy, int opcode, wl_argument* args, wl_interface* iface);
        [DllImport("libwayland-client.so")]
        public static extern wl_proxy* wl_proxy_marshal_array_constructor_versioned(wl_proxy* proxy, int opcode, wl_argument* args, wl_interface* iface, uint version);
        
        //[DllImport("libwayland-client.so")]
        //public static extern void wl_proxy_set_queue(wl_proxy* proxy);
        
        //[DllImport("libwayland-client.so", EntryPoint = "wl_proxy_create_wrapper")]
        //public static extern IntPtr CreateWrapper(IntPtr proxy);
        //[DllImport("libwayland-client.so", EntryPoint = "wl_proxy_wrapper_destroy")]


        [DllImport("libwayland-client.so")]
        public static extern void wl_array_init(wl_array* array);

        [DllImport("libwayland-client.so")]
        public static extern void wl_array_release(wl_array* array);

        [DllImport("libwayland-client.so")]
        public static extern void* wl_array_add(wl_array* array, uint size);

        [DllImport("libwayland-client.so")]
        public static extern int wl_array_copy(wl_array* array, wl_array* source);
    }

    internal static class WlEgl
    {

        [DllImport("libwayland-egl.so", EntryPoint = "wl_egl_window_create")]
        public static extern IntPtr WindowCreate(IntPtr surface, int width, int height);

        [DllImport("libwayland-egl.so", EntryPoint = "wl_egl_window_destroy")]
        public static extern void WindowDestroy(IntPtr egl_window);
        [DllImport("libwayland-egl.so", EntryPoint = "wl_egl_window_resize")]
        public static extern void WindowResize(IntPtr egl_window, int width, int height, int dx, int dy);
        [DllImport("libwayland-egl.so", EntryPoint = "wl_egl_window_get_attached_size")]
        public static extern void WindowGetAttachedSize(IntPtr egl_window, out int width, out int height);
    }
}

