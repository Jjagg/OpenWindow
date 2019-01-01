// This file was generated from an xml Wayland protocol specification
// by WaylandSharpGen. https://github.com/Jjagg/OpenWindow/tree/master/generators/WaylandSharpGen

#pragma warning disable CS0649

// Protocol: xdg_decoration_unstable_v1

using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal static unsafe partial class XdgDecorationUnstableV1Bindings
    {
        private static bool _loaded;

        public static wl_interface* Interfaces;
        private static wl_message* _messages;
        private static wl_interface** _signatureTypes;

        private static readonly int InterfaceCount = 2;
        private static readonly int MessageCount = 6;

        public const string zxdg_decoration_manager_v1_name = "zxdg_decoration_manager_v1";
        public const string zxdg_toplevel_decoration_v1_name = "zxdg_toplevel_decoration_v1";

        public static void Load()
        {
            if (_loaded)
                return;
            _loaded = true;

            Interfaces = (wl_interface*) Marshal.AllocHGlobal(sizeof(wl_interface) * InterfaceCount);
            _messages = (wl_message*) Marshal.AllocHGlobal(sizeof(wl_message) * MessageCount);


            Util.CreateInterface(&Interfaces[0], "zxdg_decoration_manager_v1", 1, 2, 0);
            Util.CreateInterface(&Interfaces[1], "zxdg_toplevel_decoration_v1", 1, 3, 1);

            _signatureTypes = (wl_interface**) Marshal.AllocHGlobal(sizeof(void*) * 3);
            _signatureTypes[0] = zxdg_toplevel_decoration_v1.Interface;
            _signatureTypes[1] = xdg_toplevel.Interface;
            _signatureTypes[2] = null;

            Util.CreateMessage(&_messages[0], "destroy", "", &_signatureTypes[2]);
            Util.CreateMessage(&_messages[1], "get_toplevel_decoration", "no", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[2], "destroy", "", &_signatureTypes[2]);
            Util.CreateMessage(&_messages[3], "set_mode", "u", &_signatureTypes[2]);
            Util.CreateMessage(&_messages[4], "unset_mode", "", &_signatureTypes[2]);
            Util.CreateMessage(&_messages[5], "configure", "u", &_signatureTypes[2]);

            Interfaces[0].Requests = &_messages[0];
            Interfaces[0].Events = null;
            Interfaces[1].Requests = &_messages[2];
            Interfaces[1].Events = &_messages[5];
        }

        public static void Unload()
        {
            if (!_loaded)
                return;
            _loaded = false;

            for (var i = 0; i < InterfaceCount; i++)
                Marshal.FreeHGlobal((IntPtr) Interfaces[i].Name);

            for (var i = 0; i < MessageCount; i++)
            {
                Marshal.FreeHGlobal((IntPtr) _messages[i].Name);
                Marshal.FreeHGlobal((IntPtr) _messages[i].Signature);
            }

            Marshal.FreeHGlobal((IntPtr) _messages);
            Marshal.FreeHGlobal((IntPtr) _signatureTypes);
            Marshal.FreeHGlobal((IntPtr) Interfaces);
        }

        /// <summary>
        /// <p>
        /// Destroy the decoration manager. This doesn't destroy objects created
        /// with the manager.
        /// </p>
        /// </summary>
        public static void zxdg_decoration_manager_v1_destroy(zxdg_decoration_manager_v1* pointer)
        {
            WaylandClient.wl_proxy_marshal((wl_proxy*) pointer, 0);
        }

        /// <summary>
        /// <p>
        /// Create a new decoration object associated with the given toplevel.
        /// </p>
        /// <p>
        /// Creating an xdg_toplevel_decoration from an xdg_toplevel which has a
        /// buffer attached or committed is a client error, and any attempts by a
        /// client to attach or manipulate a buffer prior to the first
        /// xdg_toplevel_decoration.configure event must also be treated as
        /// errors.
        /// </p>
        /// </summary>
        public static zxdg_toplevel_decoration_v1* zxdg_decoration_manager_v1_get_toplevel_decoration(zxdg_decoration_manager_v1* pointer, xdg_toplevel* toplevel)
        {
            var args = stackalloc wl_argument[2];
            args[0] = 0;
            args[1] = toplevel;
            var ptr = WaylandClient.wl_proxy_marshal_array_constructor((wl_proxy*) pointer, 1, args, zxdg_toplevel_decoration_v1.Interface);
            return (zxdg_toplevel_decoration_v1*) ptr;
        }

        /// <summary>
        /// <p>
        /// Switch back to a mode without any server-side decorations at the next
        /// commit.
        /// </p>
        /// </summary>
        public static void zxdg_toplevel_decoration_v1_destroy(zxdg_toplevel_decoration_v1* pointer)
        {
            WaylandClient.wl_proxy_marshal((wl_proxy*) pointer, 0);
        }

        /// <summary>
        /// <p>
        /// Set the toplevel surface decoration mode. This informs the compositor
        /// that the client prefers the provided decoration mode.
        /// </p>
        /// <p>
        /// After requesting a decoration mode, the compositor will respond by
        /// emitting a xdg_surface.configure event. The client should then update
        /// its content, drawing it without decorations if the received mode is
        /// server-side decorations. The client must also acknowledge the configure
        /// when committing the new content (see xdg_surface.ack_configure).
        /// </p>
        /// <p>
        /// The compositor can decide not to use the client's mode and enforce a
        /// different mode instead.
        /// </p>
        /// <p>
        /// Clients whose decoration mode depend on the xdg_toplevel state may send
        /// a set_mode request in response to a xdg_surface.configure event and wait
        /// for the next xdg_surface.configure event to prevent unwanted state.
        /// Such clients are responsible for preventing configure loops and must
        /// make sure not to send multiple successive set_mode requests with the
        /// same decoration mode.
        /// </p>
        /// </summary>
        /// <param name="mode">the decoration mode</param>
        public static void zxdg_toplevel_decoration_v1_set_mode(zxdg_toplevel_decoration_v1* pointer, zxdg_toplevel_decoration_v1_mode mode)
        {
            var args = stackalloc wl_argument[1];
            args[0] = (int) mode;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 1, args);
        }

        /// <summary>
        /// <p>
        /// Unset the toplevel surface decoration mode. This informs the compositor
        /// that the client doesn't prefer a particular decoration mode.
        /// </p>
        /// <p>
        /// This request has the same semantics as set_mode.
        /// </p>
        /// </summary>
        public static void zxdg_toplevel_decoration_v1_unset_mode(zxdg_toplevel_decoration_v1* pointer)
        {
            WaylandClient.wl_proxy_marshal((wl_proxy*) pointer, 2);
        }

        /// <summary>
        /// <p>
        /// The configure event asks the client to change its decoration mode. The
        /// configured state should not be applied immediately. Clients must send an
        /// ack_configure in response to this event. See xdg_surface.configure and
        /// xdg_surface.ack_configure for details.
        /// </p>
        /// <p>
        /// A configure event can be sent at any time. The specified mode must be
        /// obeyed by the client.
        /// </p>
        /// </summary>
        public delegate void zxdg_toplevel_decoration_v1_configure_delegate(void* data, zxdg_toplevel_decoration_v1* proxy, zxdg_toplevel_decoration_v1_mode mode);

        internal struct zxdg_toplevel_decoration_v1_listener
        {
            public IntPtr configure;

            public static zxdg_toplevel_decoration_v1_listener* Alloc(
                zxdg_toplevel_decoration_v1_configure_delegate configure)
            {
                var ret = (zxdg_toplevel_decoration_v1_listener*) Marshal.AllocHGlobal(sizeof(zxdg_toplevel_decoration_v1_listener));
                Set(ret, configure);
                return ret;
            }

            public static void Set(zxdg_toplevel_decoration_v1_listener* listener
            ,
                zxdg_toplevel_decoration_v1_configure_delegate configure)
            {
                if (configure != null) listener->configure = Marshal.GetFunctionPointerForDelegate<zxdg_toplevel_decoration_v1_configure_delegate>(configure);
            }
        }

        /// <summary>
        /// Set the callbacks for the given <see cref="zxdg_toplevel_decoration_v1"/>.
        /// </summary>
        /// <param name="mode">the decoration mode</param>
        public static int zxdg_toplevel_decoration_v1_add_listener(zxdg_toplevel_decoration_v1* iface, zxdg_toplevel_decoration_v1_listener* listener)
        {
            return WaylandClient.wl_proxy_add_listener((wl_proxy*) iface, listener, null);
        }
    }

    /// <summary>
    /// <p>
    /// This interface allows a compositor to announce support for server-side
    /// decorations.
    /// </p>
    /// <p>
    /// A window decoration is a set of window controls as deemed appropriate by
    /// the party managing them, such as user interface components used to move,
    /// resize and change a window's state.
    /// </p>
    /// <p>
    /// A client can use this protocol to request being decorated by a supporting
    /// compositor.
    /// </p>
    /// <p>
    /// If compositor and client do not negotiate the use of a server-side
    /// decoration using this protocol, clients continue to self-decorate as they
    /// see fit.
    /// </p>
    /// <p>
    /// Warning! The protocol described in this file is experimental and
    /// backward incompatible changes may be made. Backward compatible changes
    /// may be added together with the corresponding interface version bump.
    /// Backward incompatible changes are done by bumping the version number in
    /// the protocol and interface names and resetting the interface version.
    /// Once the protocol is to be declared stable, the 'z' prefix and the
    /// version number in the protocol and interface names are removed and the
    /// interface version number is reset.
    /// </p>
    /// </summary>
    internal struct zxdg_decoration_manager_v1 { public static unsafe wl_interface* Interface => &XdgDecorationUnstableV1Bindings.Interfaces[0]; }
    /// <summary>
    /// <p>
    /// The decoration object allows the compositor to toggle server-side window
    /// decorations for a toplevel surface. The client can request to switch to
    /// another mode.
    /// </p>
    /// <p>
    /// The xdg_toplevel_decoration object must be destroyed before its
    /// xdg_toplevel.
    /// </p>
    /// </summary>
    internal struct zxdg_toplevel_decoration_v1 { public static unsafe wl_interface* Interface => &XdgDecorationUnstableV1Bindings.Interfaces[1]; }


    internal enum zxdg_toplevel_decoration_v1_error
    {
        /// <summary>
        /// xdg_toplevel has a buffer attached before configure
        /// </summary>
        UnconfiguredBuffer = 0,

        /// <summary>
        /// xdg_toplevel already has a decoration object
        /// </summary>
        AlreadyConstructed = 1,

        /// <summary>
        /// xdg_toplevel destroyed before the decoration object
        /// </summary>
        Orphaned = 2,

    }

    /// <summary>
    /// <p>
    /// These values describe window decoration modes.
    /// </p>
    /// </summary>
    internal enum zxdg_toplevel_decoration_v1_mode
    {
        /// <summary>
        /// no server-side window decoration
        /// </summary>
        ClientSide = 1,

        /// <summary>
        /// server-side window decoration
        /// </summary>
        ServerSide = 2,

    }
}

namespace OpenWindow.Backends.Wayland.Managed
{
    internal unsafe partial struct ZxdgDecorationManagerV1
    {
        public static IntPtr Interface => (IntPtr) zxdg_decoration_manager_v1.Interface;
        public static ZxdgDecorationManagerV1 Null => new ZxdgDecorationManagerV1();
        public readonly zxdg_decoration_manager_v1* Pointer;
        public bool IsNull => Pointer == null;
        public ZxdgDecorationManagerV1(zxdg_decoration_manager_v1* ptr) { Pointer = ptr; }
        public static implicit operator ZxdgDecorationManagerV1(zxdg_decoration_manager_v1* ptr) => new ZxdgDecorationManagerV1(ptr);
        public static explicit operator ZxdgDecorationManagerV1(wl_proxy* ptr) => new ZxdgDecorationManagerV1((zxdg_decoration_manager_v1*) ptr);
        public void Destroy() => XdgDecorationUnstableV1Bindings.zxdg_decoration_manager_v1_destroy(Pointer);
        public ZxdgToplevelDecorationV1 GetToplevelDecoration(in XdgToplevel toplevel) => XdgDecorationUnstableV1Bindings.zxdg_decoration_manager_v1_get_toplevel_decoration(Pointer, toplevel.Pointer);
    }
    internal unsafe partial struct ZxdgToplevelDecorationV1
    {
        public static IntPtr Interface => (IntPtr) zxdg_toplevel_decoration_v1.Interface;
        public static ZxdgToplevelDecorationV1 Null => new ZxdgToplevelDecorationV1();
        public readonly zxdg_toplevel_decoration_v1* Pointer;
        public bool IsNull => Pointer == null;
        private XdgDecorationUnstableV1Bindings.zxdg_toplevel_decoration_v1_configure_delegate _configure;
        private XdgDecorationUnstableV1Bindings.zxdg_toplevel_decoration_v1_listener* _listener;
        public ZxdgToplevelDecorationV1(zxdg_toplevel_decoration_v1* ptr) { Pointer = ptr; _listener = null; _configure = null; }
        public static implicit operator ZxdgToplevelDecorationV1(zxdg_toplevel_decoration_v1* ptr) => new ZxdgToplevelDecorationV1(ptr);
        public static explicit operator ZxdgToplevelDecorationV1(wl_proxy* ptr) => new ZxdgToplevelDecorationV1((zxdg_toplevel_decoration_v1*) ptr);
        public void Destroy() => XdgDecorationUnstableV1Bindings.zxdg_toplevel_decoration_v1_destroy(Pointer);
        public void SetMode(zxdg_toplevel_decoration_v1_mode mode) => XdgDecorationUnstableV1Bindings.zxdg_toplevel_decoration_v1_set_mode(Pointer, mode);
        public void UnsetMode() => XdgDecorationUnstableV1Bindings.zxdg_toplevel_decoration_v1_unset_mode(Pointer);
        public void SetListener(
            XdgDecorationUnstableV1Bindings.zxdg_toplevel_decoration_v1_configure_delegate configure)
        {
            _configure = configure;
            _listener = XdgDecorationUnstableV1Bindings.zxdg_toplevel_decoration_v1_listener.Alloc(configure);
            XdgDecorationUnstableV1Bindings.zxdg_toplevel_decoration_v1_add_listener(Pointer, _listener);
        }
        public void FreeListener() { if (_listener != null) Marshal.FreeHGlobal((IntPtr) _listener); }
    }
}
