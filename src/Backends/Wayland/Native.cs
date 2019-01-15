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

    internal unsafe static class Libc
    {
        [DllImport("libc")]
        public static extern int close(int fd);

        public const int PROT_NONE = 0;
        public const int PROT_READ = 1;
        public const int PROT_WRITE = 2;
        public const int PROT_EXEC = 4;

        public const int MAP_FILE = 0x0000;
        public const int MAP_SHARED = 0x0001;
        public const int MAP_PRIVATE = 0x0002;
        public const int MAP_FIXED = 0x0010;
        public const int MAP_ANON = 0x1000;

        [DllImport("libc")]
        public static extern byte* mmap(void *addr, uint length, int prot, int flags, int fd, int offset);

        [DllImport("libc")]
        public static extern int munmap(void *addr, uint length);
    }

    public struct xkb_context { }
    public struct xkb_keymap { }
    public struct xkb_state { }

    internal unsafe static class XkbCommon
    {
        public const int XKB_CONTEXT_NO_FLAGS = 0;

        [DllImport("libxkbcommon.so")]
        public static extern xkb_context* xkb_context_new(int flags = XKB_CONTEXT_NO_FLAGS);

        [DllImport("libxkbcommon.so")]
        public static extern void xkb_context_unref(xkb_context *ctx);

        public const int XKB_KEYMAP_COMPILE_NO_FLAGS = 0;
        public const int XKB_KEYMAP_FORMAT_TEXT_V1 = 1;

        [DllImport("libxkbcommon.so")]
        public static extern xkb_keymap* xkb_keymap_new_from_string(xkb_context* context, byte* str, int format, int flags = XKB_KEYMAP_COMPILE_NO_FLAGS);

        [DllImport("libxkbcommon.so")]
        public static extern void xkb_keymap_unref(xkb_keymap *keymap);

        [DllImport("libxkbcommon.so")]
        public static extern xkb_state* xkb_state_new(xkb_keymap* keymap);


        [DllImport("libxkbcommon.so")]
        public static extern uint xkb_state_key_get_one_sym(xkb_state* state, uint key);

        [DllImport("libxkbcommon.so")]
        public static extern int xkb_state_key_get_utf8(xkb_state* state, uint key, byte* buffer, int size);

        [DllImport("libxkbcommon.so")]
        public static extern void xkb_state_unref(xkb_state *state);
    }
}

