using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal class WlProxy : WlObject
    {
        protected WlProxy(IntPtr pointer)
            : base(pointer)
        {
        }

        public void Destroy()
        {
            Destroy(Pointer);
        }

        //[DllImport("libwayland-client.so", EntryPoint = "wl_proxy_create")]
        //public static extern IntPtr Create(IntPtr proxy);
        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_destroy")]
        private static extern IntPtr Destroy(IntPtr proxy);
        
        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_dispatcher")]
        public static extern void AddDispatcher();
        
        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        public static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);
                   
        //[DllImport("libwayland-client.so", EntryPoint = "wl_proxy_get_class")]
        //public static extern IntPtr GetClass(IntPtr proxy);
        
        //[DllImport("libwayland-client.so", EntryPoint = "wl_proxy_get_id")]
        //public static extern int GetId(IntPtr proxy);
        
        //[DllImport("libwayland-client.so", EntryPoint = "wl_proxy_get_listener")]
        //public static extern IntPtr GetListener(IntPtr proxy);
        
        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_get_user_data")]
        public static extern IntPtr GetUserData(IntPtr proxy);
        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_set_user_data")]
        public static extern void SetUserData(IntPtr proxy, IntPtr data);
        
        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_get_version")]
        public static extern uint GetVersion(IntPtr proxy);
        
        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_marshal")]
        public static extern void Marshal(IntPtr proxy, int opcode);
        
        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_marshal_array")]
        public static extern void MarshalArray(IntPtr proxy, int opcode, IntPtr data);
        
        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_marshal_constructor")]
        public static extern IntPtr MarshalConstructor(IntPtr proxy, int opcode, IntPtr iface, IntPtr data);
        //[DllImport("libwayland-client.so", EntryPoint = "wl_proxy_marshal_constructor_versioned")]
        //public static extern IntPtr MarshalConstructorVersioned(IntPtr proxy, int opcode, IntPtr iface,
        //    uint version, uint name, uint ifaceName, uint ifaceVersion, IntPtr data);
        
        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_marshal_array_constructor")]
        public static extern IntPtr MarshalArrayConstructor(IntPtr proxy, int opcode, IntPtr args, IntPtr iface);
        // TODO use this version when we have something to test arguments
        //public static extern IntPtr MarshalArrayConstructor(IntPtr proxy, int opcode, ArgumentList.ArgumentStruct[] args, IntPtr iface);
        //[DllImport("libwayland-client.so", EntryPoint = "wl_proxy_marshal_array_constructor_versioned")]
        //public static extern IntPtr MarshalArrayConstructorVersioned(IntPtr proxy, int opcode, IntPtr iface,
        //    uint version, uint name, uint ifaceName, uint ifaceVersion, IntPtr data);
        
        //[DllImport("libwayland-client.so", EntryPoint = "wl_proxy_set_queue")]
        //public static extern void SetQueue(IntPtr proxy);
        
        //[DllImport("libwayland-client.so", EntryPoint = "wl_proxy_create_wrapper")]
        //public static extern IntPtr CreateWrapper(IntPtr proxy);
        //[DllImport("libwayland-client.so", EntryPoint = "wl_proxy_wrapper_destroy")]
        //public static extern void WrapperDestroy(IntPtr proxy, IntPtr wrapper);
    }
}