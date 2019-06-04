using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal static unsafe class WaylandClient
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


        [DllImport("libwayland-egl.so")]
        public static extern wl_egl_window* wl_egl_window_create(wl_surface* surface, int width, int height);

        [DllImport("libwayland-egl.so")]
        public static extern void wl_egl_window_destroy(wl_egl_window* egl_window);

        [DllImport("libwayland-egl.so")]
        public static extern void wl_egl_window_resize(wl_egl_window* egl_window, int width, int height, int dx, int dy);

        [DllImport("libwayland-egl.so")]
        public static extern void wl_egl_window_get_attached_size(wl_egl_window* egl_window, out int width, out int height, int dx, int dy);
    }

    internal static unsafe class Libc
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

    internal struct xkb_context { }
    internal struct xkb_keymap { }
    internal struct xkb_state { }

    internal enum xkb_state_component { }

    internal static unsafe class XkbCommon
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
        public static extern xkb_state_component xkb_state_update_key(xkb_state* state, uint key, wl_keyboard_key_state down);

        [DllImport("libxkbcommon.so")]
        public static extern xkb_state_component xkb_state_update_mask(xkb_state* state, uint depressed_mods, uint latched_mods, uint locked_mods, uint depressed_layout, uint latched_layout, uint locked_layout);

        [DllImport("libxkbcommon.so")]
        public static extern uint xkb_state_key_get_one_sym(xkb_state* state, uint key);

        [DllImport("libxkbcommon.so")]
        public static extern int xkb_state_key_get_utf32(xkb_state* state, uint key);

        [DllImport("libxkbcommon.so")]
        public static extern int xkb_state_key_get_utf8(xkb_state* state, uint key, byte* buffer, int size);

        [DllImport("libxkbcommon.so")]
        public static extern void xkb_state_unref(xkb_state *state);
    }

    internal static unsafe class Egl
    {
        public static void Load()
        {
            // These are exposed directly, but that's not portable according to the spec
            // so we behave nicely and use eglGetProcAddress
            GetDisplay = LoadFunction<GetDisplayDelegate>("eglGetDisplay");
            Terminate = LoadFunction<TerminateDelegate>("eglTerminate");
            Initialize = LoadFunction<InitializeDelegate>("eglInitialize");
            BindAPI = LoadFunction<BindAPIDelegate>("eglBindAPI");
            ChooseConfig = LoadFunction<ChooseConfigDelegate>("eglChooseConfig");
            CreateWindowSurface = LoadFunction<CreateWindowSurfaceDelegate>("eglCreateWindowSurface");
            DestroySurface = LoadFunction<DestroySurfaceDelegate>("eglDestroySurface");
            GetError = LoadFunction<GetErrorDelegate>("eglGetError");
        }

        private static T LoadFunction<T>(string str) where T : Delegate
        {
            var ptr = GetProcAddress(str);
            if (ptr == IntPtr.Zero)
                return null;

            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        [DllImport("libEGL.so", EntryPoint="eglGetProcAddress")]
        public static extern IntPtr GetProcAddress(string procName);


        public static int TRUE = 1;
        public static int FALSE = 0;

        public static int OPENGL_ES_API = 0x30A0;
        public static int OPENGL_API = 0x30A2;

        public static int OPENGL_ES_BIT = 0x0001;
        public static int OPENGL_BIT = 0x0008;

        public static int RED_SIZE = 0x3024;
        public static int GREEN_SIZE = 0x3023;
        public static int BLUE_SIZE = 0x3022;
        public static int ALPHA_SIZE = 0x3021;
        public static int DEPTH_SIZE = 0x3025;
        public static int STENCIL_SIZE = 0x3026;
        public static int SAMPLES = 0x3031;
        public static int SAMPLE_BUFFERS = 0x3032;
        public static int NONE = 0x3038;
        public static int BIND_TO_TEXTURE_RGB = 0x3039;
        public static int BIND_TO_TEXTURE_RGBA = 0x303A;

        public static int RENDERABLE_TYPE = 0x3040;

        public static int BACK_BUFFER = 0x3084;
        public static int SINGLE_BUFFER = 0x3085;
        public static int RENDER_BUFFER = 0x3086;

        public delegate EGLDisplay* GetDisplayDelegate(wl_display* dpy);
        public static GetDisplayDelegate GetDisplay;

        public delegate bool InitializeDelegate(EGLDisplay* display, out int major, out int minor);
        public static InitializeDelegate Initialize;

        public delegate bool TerminateDelegate(EGLDisplay* display);
        public static TerminateDelegate Terminate;

        public delegate bool BindAPIDelegate(int api);
        public static BindAPIDelegate BindAPI;

        public delegate bool ChooseConfigDelegate(EGLDisplay* display, int[] attribList, IntPtr[] configs, int configSize, out int numConfigs);
        public static ChooseConfigDelegate ChooseConfig;

        public delegate EGLSurface* CreateWindowSurfaceDelegate(EGLDisplay* display, EGLConfig* config, wl_egl_window* eglWindow, int[] attribList);
        public static CreateWindowSurfaceDelegate CreateWindowSurface;

        public delegate bool DestroySurfaceDelegate(EGLDisplay* display, EGLSurface* surface);
        public static DestroySurfaceDelegate DestroySurface;

        public delegate int GetErrorDelegate();
        public static GetErrorDelegate GetError;
    }
}

