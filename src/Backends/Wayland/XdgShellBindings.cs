// This file was generated from an xml Wayland protocol specification
// by WaylandSharpGen. https://github.com/Jjagg/OpenWindow/tree/master/generators/WaylandSharpGen

#pragma warning disable CS0649

// Protocol: xdg_shell

using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal static unsafe partial class XdgShellBindings
    {
        private static bool _loaded;

        public static wl_interface* Interfaces;
        private static wl_message* _messages;
        private static wl_interface** _signatureTypes;

        private static readonly int InterfaceCount = 5;
        private static readonly int MessageCount = 38;

        public const string xdg_wm_base_name = "xdg_wm_base";
        public const string xdg_positioner_name = "xdg_positioner";
        public const string xdg_surface_name = "xdg_surface";
        public const string xdg_toplevel_name = "xdg_toplevel";
        public const string xdg_popup_name = "xdg_popup";

        public static void Load()
        {
            if (_loaded)
                return;
            _loaded = true;

            Interfaces = (wl_interface*) Marshal.AllocHGlobal(sizeof(wl_interface) * InterfaceCount);
            _messages = (wl_message*) Marshal.AllocHGlobal(sizeof(wl_message) * MessageCount);


            Util.CreateInterface(&Interfaces[0], "xdg_wm_base", 2, 4, 1);
            Util.CreateInterface(&Interfaces[1], "xdg_positioner", 2, 7, 0);
            Util.CreateInterface(&Interfaces[2], "xdg_surface", 2, 5, 1);
            Util.CreateInterface(&Interfaces[3], "xdg_toplevel", 2, 14, 2);
            Util.CreateInterface(&Interfaces[4], "xdg_popup", 2, 2, 2);

            _signatureTypes = (wl_interface**) Marshal.AllocHGlobal(sizeof(void*) * 15);
            _signatureTypes[0] = null;
            _signatureTypes[1] = null;
            _signatureTypes[2] = null;
            _signatureTypes[3] = null;
            _signatureTypes[4] = wl_seat.Interface;
            _signatureTypes[5] = null;
            _signatureTypes[6] = null;
            _signatureTypes[7] = null;
            _signatureTypes[8] = xdg_popup.Interface;
            _signatureTypes[9] = xdg_surface.Interface;
            _signatureTypes[10] = xdg_positioner.Interface;
            _signatureTypes[11] = xdg_surface.Interface;
            _signatureTypes[12] = wl_surface.Interface;
            _signatureTypes[13] = xdg_toplevel.Interface;
            _signatureTypes[14] = wl_output.Interface;

            Util.CreateMessage(&_messages[0], "destroy", "", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[1], "create_positioner", "n", &_signatureTypes[10]);
            Util.CreateMessage(&_messages[2], "get_xdg_surface", "no", &_signatureTypes[11]);
            Util.CreateMessage(&_messages[3], "pong", "u", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[4], "ping", "u", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[5], "destroy", "", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[6], "set_size", "ii", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[7], "set_anchor_rect", "iiii", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[8], "set_anchor", "u", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[9], "set_gravity", "u", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[10], "set_constraint_adjustment", "u", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[11], "set_offset", "ii", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[12], "destroy", "", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[13], "get_toplevel", "n", &_signatureTypes[13]);
            Util.CreateMessage(&_messages[14], "get_popup", "n?oo", &_signatureTypes[8]);
            Util.CreateMessage(&_messages[15], "set_window_geometry", "iiii", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[16], "ack_configure", "u", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[17], "configure", "u", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[18], "destroy", "", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[19], "set_parent", "?o", &_signatureTypes[13]);
            Util.CreateMessage(&_messages[20], "set_title", "s", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[21], "set_app_id", "s", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[22], "show_window_menu", "ouii", &_signatureTypes[4]);
            Util.CreateMessage(&_messages[23], "move", "ou", &_signatureTypes[4]);
            Util.CreateMessage(&_messages[24], "resize", "ouu", &_signatureTypes[4]);
            Util.CreateMessage(&_messages[25], "set_max_size", "ii", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[26], "set_min_size", "ii", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[27], "set_maximized", "", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[28], "unset_maximized", "", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[29], "set_fullscreen", "?o", &_signatureTypes[14]);
            Util.CreateMessage(&_messages[30], "unset_fullscreen", "", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[31], "set_minimized", "", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[32], "configure", "iia", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[33], "close", "", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[34], "destroy", "", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[35], "grab", "ou", &_signatureTypes[4]);
            Util.CreateMessage(&_messages[36], "configure", "iiii", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[37], "popup_done", "", &_signatureTypes[0]);

            Interfaces[0].Requests = &_messages[0];
            Interfaces[0].Events = &_messages[4];
            Interfaces[1].Requests = &_messages[5];
            Interfaces[1].Events = null;
            Interfaces[2].Requests = &_messages[12];
            Interfaces[2].Events = &_messages[17];
            Interfaces[3].Requests = &_messages[18];
            Interfaces[3].Events = &_messages[32];
            Interfaces[4].Requests = &_messages[34];
            Interfaces[4].Events = &_messages[36];
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
        /// Destroy this xdg_wm_base object.
        /// </p>
        /// <p>
        /// Destroying a bound xdg_wm_base object while there are surfaces
        /// still alive created by this xdg_wm_base object instance is illegal
        /// and will result in a protocol error.
        /// </p>
        /// </summary>
        public static void xdg_wm_base_destroy(xdg_wm_base* pointer)
        {
            WaylandClient.wl_proxy_marshal((wl_proxy*) pointer, 0);
        }

        /// <summary>
        /// <p>
        /// Create a positioner object. A positioner object is used to position
        /// surfaces relative to some parent surface. See the interface description
        /// and xdg_surface.get_popup for details.
        /// </p>
        /// </summary>
        public static xdg_positioner* xdg_wm_base_create_positioner(xdg_wm_base* pointer)
        {
            var ptr = WaylandClient.wl_proxy_marshal_constructor((wl_proxy*) pointer, 1, xdg_positioner.Interface, null);
            return (xdg_positioner*) ptr;
        }

        /// <summary>
        /// <p>
        /// This creates an xdg_surface for the given surface. While xdg_surface
        /// itself is not a role, the corresponding surface may only be assigned
        /// a role extending xdg_surface, such as xdg_toplevel or xdg_popup.
        /// </p>
        /// <p>
        /// This creates an xdg_surface for the given surface. An xdg_surface is
        /// used as basis to define a role to a given surface, such as xdg_toplevel
        /// or xdg_popup. It also manages functionality shared between xdg_surface
        /// based surface roles.
        /// </p>
        /// <p>
        /// See the documentation of xdg_surface for more details about what an
        /// xdg_surface is and how it is used.
        /// </p>
        /// </summary>
        public static xdg_surface* xdg_wm_base_get_xdg_surface(xdg_wm_base* pointer, wl_surface* surface)
        {
            var args = stackalloc wl_argument[2];
            args[0] = 0;
            args[1] = surface;
            var ptr = WaylandClient.wl_proxy_marshal_array_constructor((wl_proxy*) pointer, 2, args, xdg_surface.Interface);
            return (xdg_surface*) ptr;
        }

        /// <summary>
        /// <p>
        /// A client must respond to a ping event with a pong request or
        /// the client may be deemed unresponsive. See xdg_wm_base.ping.
        /// </p>
        /// </summary>
        /// <param name="serial">serial of the ping event</param>
        public static void xdg_wm_base_pong(xdg_wm_base* pointer, uint serial)
        {
            var args = stackalloc wl_argument[1];
            args[0] = serial;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 3, args);
        }

        /// <summary>
        /// <p>
        /// The ping event asks the client if it's still alive. Pass the
        /// serial specified in the event back to the compositor by sending
        /// a "pong" request back with the specified serial. See xdg_wm_base.pong.
        /// </p>
        /// <p>
        /// Compositors can use this to determine if the client is still
        /// alive. It's unspecified what will happen if the client doesn't
        /// respond to the ping request, or in what timeframe. Clients should
        /// try to respond in a reasonable amount of time.
        /// </p>
        /// <p>
        /// A compositor is free to ping in any way it wants, but a client must
        /// always respond to any xdg_wm_base object it created.
        /// </p>
        /// </summary>
        public delegate void xdg_wm_base_ping_delegate(void* data, xdg_wm_base* proxy, uint serial);

        internal struct xdg_wm_base_listener
        {
            public IntPtr ping;

            public static xdg_wm_base_listener* Alloc(
                xdg_wm_base_ping_delegate ping)
            {
                var ret = (xdg_wm_base_listener*) Marshal.AllocHGlobal(sizeof(xdg_wm_base_listener));
                Set(ret, ping);
                return ret;
            }

            public static void Set(xdg_wm_base_listener* listener
            ,
                xdg_wm_base_ping_delegate ping)
            {
                if (ping != null) listener->ping = Marshal.GetFunctionPointerForDelegate<xdg_wm_base_ping_delegate>(ping);
            }
        }

        /// <summary>
        /// Set the callbacks for the given <see cref="xdg_wm_base"/>.
        /// </summary>
        /// <param name="serial">pass this to the pong request</param>
        public static int xdg_wm_base_add_listener(xdg_wm_base* iface, xdg_wm_base_listener* listener)
        {
            return WaylandClient.wl_proxy_add_listener((wl_proxy*) iface, listener, null);
        }
        /// <summary>
        /// <p>
        /// Notify the compositor that the xdg_positioner will no longer be used.
        /// </p>
        /// </summary>
        public static void xdg_positioner_destroy(xdg_positioner* pointer)
        {
            WaylandClient.wl_proxy_marshal((wl_proxy*) pointer, 0);
        }

        /// <summary>
        /// <p>
        /// Set the size of the surface that is to be positioned with the positioner
        /// object. The size is in surface-local coordinates and corresponds to the
        /// window geometry. See xdg_surface.set_window_geometry.
        /// </p>
        /// <p>
        /// If a zero or negative size is set the invalid_input error is raised.
        /// </p>
        /// </summary>
        /// <param name="width">width of positioned rectangle</param>
        /// <param name="height">height of positioned rectangle</param>
        public static void xdg_positioner_set_size(xdg_positioner* pointer, int width, int height)
        {
            var args = stackalloc wl_argument[2];
            args[0] = width;
            args[1] = height;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 1, args);
        }

        /// <summary>
        /// <p>
        /// Specify the anchor rectangle within the parent surface that the child
        /// surface will be placed relative to. The rectangle is relative to the
        /// window geometry as defined by xdg_surface.set_window_geometry of the
        /// parent surface.
        /// </p>
        /// <p>
        /// When the xdg_positioner object is used to position a child surface, the
        /// anchor rectangle may not extend outside the window geometry of the
        /// positioned child's parent surface.
        /// </p>
        /// <p>
        /// If a negative size is set the invalid_input error is raised.
        /// </p>
        /// </summary>
        /// <param name="x">x position of anchor rectangle</param>
        /// <param name="y">y position of anchor rectangle</param>
        /// <param name="width">width of anchor rectangle</param>
        /// <param name="height">height of anchor rectangle</param>
        public static void xdg_positioner_set_anchor_rect(xdg_positioner* pointer, int x, int y, int width, int height)
        {
            var args = stackalloc wl_argument[4];
            args[0] = x;
            args[1] = y;
            args[2] = width;
            args[3] = height;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 2, args);
        }

        /// <summary>
        /// <p>
        /// Defines the anchor point for the anchor rectangle. The specified anchor
        /// is used derive an anchor point that the child surface will be
        /// positioned relative to. If a corner anchor is set (e.g. 'top_left' or
        /// 'bottom_right'), the anchor point will be at the specified corner;
        /// otherwise, the derived anchor point will be centered on the specified
        /// edge, or in the center of the anchor rectangle if no edge is specified.
        /// </p>
        /// </summary>
        /// <param name="anchor">anchor</param>
        public static void xdg_positioner_set_anchor(xdg_positioner* pointer, xdg_positioner_anchor anchor)
        {
            var args = stackalloc wl_argument[1];
            args[0] = (int) anchor;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 3, args);
        }

        /// <summary>
        /// <p>
        /// Defines in what direction a surface should be positioned, relative to
        /// the anchor point of the parent surface. If a corner gravity is
        /// specified (e.g. 'bottom_right' or 'top_left'), then the child surface
        /// will be placed towards the specified gravity; otherwise, the child
        /// surface will be centered over the anchor point on any axis that had no
        /// gravity specified.
        /// </p>
        /// </summary>
        /// <param name="gravity">gravity direction</param>
        public static void xdg_positioner_set_gravity(xdg_positioner* pointer, xdg_positioner_gravity gravity)
        {
            var args = stackalloc wl_argument[1];
            args[0] = (int) gravity;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 4, args);
        }

        /// <summary>
        /// <p>
        /// Specify how the window should be positioned if the originally intended
        /// position caused the surface to be constrained, meaning at least
        /// partially outside positioning boundaries set by the compositor. The
        /// adjustment is set by constructing a bitmask describing the adjustment to
        /// be made when the surface is constrained on that axis.
        /// </p>
        /// <p>
        /// If no bit for one axis is set, the compositor will assume that the child
        /// surface should not change its position on that axis when constrained.
        /// </p>
        /// <p>
        /// If more than one bit for one axis is set, the order of how adjustments
        /// are applied is specified in the corresponding adjustment descriptions.
        /// </p>
        /// <p>
        /// The default adjustment is none.
        /// </p>
        /// </summary>
        /// <param name="constraint_adjustment">bit mask of constraint adjustments</param>
        public static void xdg_positioner_set_constraint_adjustment(xdg_positioner* pointer, uint constraint_adjustment)
        {
            var args = stackalloc wl_argument[1];
            args[0] = constraint_adjustment;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 5, args);
        }

        /// <summary>
        /// <p>
        /// Specify the surface position offset relative to the position of the
        /// anchor on the anchor rectangle and the anchor on the surface. For
        /// example if the anchor of the anchor rectangle is at (x, y), the surface
        /// has the gravity bottom|right, and the offset is (ox, oy), the calculated
        /// surface position will be (x + ox, y + oy). The offset position of the
        /// surface is the one used for constraint testing. See
        /// set_constraint_adjustment.
        /// </p>
        /// <p>
        /// An example use case is placing a popup menu on top of a user interface
        /// element, while aligning the user interface element of the parent surface
        /// with some user interface element placed somewhere in the popup surface.
        /// </p>
        /// </summary>
        /// <param name="x">surface position x offset</param>
        /// <param name="y">surface position y offset</param>
        public static void xdg_positioner_set_offset(xdg_positioner* pointer, int x, int y)
        {
            var args = stackalloc wl_argument[2];
            args[0] = x;
            args[1] = y;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 6, args);
        }

        /// <summary>
        /// <p>
        /// Destroy the xdg_surface object. An xdg_surface must only be destroyed
        /// after its role object has been destroyed.
        /// </p>
        /// </summary>
        public static void xdg_surface_destroy(xdg_surface* pointer)
        {
            WaylandClient.wl_proxy_marshal((wl_proxy*) pointer, 0);
        }

        /// <summary>
        /// <p>
        /// This creates an xdg_toplevel object for the given xdg_surface and gives
        /// the associated wl_surface the xdg_toplevel role.
        /// </p>
        /// <p>
        /// See the documentation of xdg_toplevel for more details about what an
        /// xdg_toplevel is and how it is used.
        /// </p>
        /// </summary>
        public static xdg_toplevel* xdg_surface_get_toplevel(xdg_surface* pointer)
        {
            var ptr = WaylandClient.wl_proxy_marshal_constructor((wl_proxy*) pointer, 1, xdg_toplevel.Interface, null);
            return (xdg_toplevel*) ptr;
        }

        /// <summary>
        /// <p>
        /// This creates an xdg_popup object for the given xdg_surface and gives
        /// the associated wl_surface the xdg_popup role.
        /// </p>
        /// <p>
        /// If null is passed as a parent, a parent surface must be specified using
        /// some other protocol, before committing the initial state.
        /// </p>
        /// <p>
        /// See the documentation of xdg_popup for more details about what an
        /// xdg_popup is and how it is used.
        /// </p>
        /// </summary>
        public static xdg_popup* xdg_surface_get_popup(xdg_surface* pointer, xdg_surface* parent, xdg_positioner* positioner)
        {
            var args = stackalloc wl_argument[3];
            args[0] = 0;
            args[1] = parent;
            args[2] = positioner;
            var ptr = WaylandClient.wl_proxy_marshal_array_constructor((wl_proxy*) pointer, 2, args, xdg_popup.Interface);
            return (xdg_popup*) ptr;
        }

        /// <summary>
        /// <p>
        /// The window geometry of a surface is its "visible bounds" from the
        /// user's perspective. Client-side decorations often have invisible
        /// portions like drop-shadows which should be ignored for the
        /// purposes of aligning, placing and constraining windows.
        /// </p>
        /// <p>
        /// The window geometry is double buffered, and will be applied at the
        /// time wl_surface.commit of the corresponding wl_surface is called.
        /// </p>
        /// <p>
        /// When maintaining a position, the compositor should treat the (x, y)
        /// coordinate of the window geometry as the top left corner of the window.
        /// A client changing the (x, y) window geometry coordinate should in
        /// general not alter the position of the window.
        /// </p>
        /// <p>
        /// Once the window geometry of the surface is set, it is not possible to
        /// unset it, and it will remain the same until set_window_geometry is
        /// called again, even if a new subsurface or buffer is attached.
        /// </p>
        /// <p>
        /// If never set, the value is the full bounds of the surface,
        /// including any subsurfaces. This updates dynamically on every
        /// commit. This unset is meant for extremely simple clients.
        /// </p>
        /// <p>
        /// The arguments are given in the surface-local coordinate space of
        /// the wl_surface associated with this xdg_surface.
        /// </p>
        /// <p>
        /// The width and height must be greater than zero. Setting an invalid size
        /// will raise an error. When applied, the effective window geometry will be
        /// the set window geometry clamped to the bounding rectangle of the
        /// combined geometry of the surface of the xdg_surface and the associated
        /// subsurfaces.
        /// </p>
        /// </summary>
        public static void xdg_surface_set_window_geometry(xdg_surface* pointer, int x, int y, int width, int height)
        {
            var args = stackalloc wl_argument[4];
            args[0] = x;
            args[1] = y;
            args[2] = width;
            args[3] = height;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 3, args);
        }

        /// <summary>
        /// <p>
        /// When a configure event is received, if a client commits the
        /// surface in response to the configure event, then the client
        /// must make an ack_configure request sometime before the commit
        /// request, passing along the serial of the configure event.
        /// </p>
        /// <p>
        /// For instance, for toplevel surfaces the compositor might use this
        /// information to move a surface to the top left only when the client has
        /// drawn itself for the maximized or fullscreen state.
        /// </p>
        /// <p>
        /// If the client receives multiple configure events before it
        /// can respond to one, it only has to ack the last configure event.
        /// </p>
        /// <p>
        /// A client is not required to commit immediately after sending
        /// an ack_configure request - it may even ack_configure several times
        /// before its next surface commit.
        /// </p>
        /// <p>
        /// A client may send multiple ack_configure requests before committing, but
        /// only the last request sent before a commit indicates which configure
        /// event the client really is responding to.
        /// </p>
        /// </summary>
        /// <param name="serial">the serial from the configure event</param>
        public static void xdg_surface_ack_configure(xdg_surface* pointer, uint serial)
        {
            var args = stackalloc wl_argument[1];
            args[0] = serial;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 4, args);
        }

        /// <summary>
        /// <p>
        /// The configure event marks the end of a configure sequence. A configure
        /// sequence is a set of one or more events configuring the state of the
        /// xdg_surface, including the final xdg_surface.configure event.
        /// </p>
        /// <p>
        /// Where applicable, xdg_surface surface roles will during a configure
        /// sequence extend this event as a latched state sent as events before the
        /// xdg_surface.configure event. Such events should be considered to make up
        /// a set of atomically applied configuration states, where the
        /// xdg_surface.configure commits the accumulated state.
        /// </p>
        /// <p>
        /// Clients should arrange their surface for the new states, and then send
        /// an ack_configure request with the serial sent in this configure event at
        /// some point before committing the new surface.
        /// </p>
        /// <p>
        /// If the client receives multiple configure events before it can respond
        /// to one, it is free to discard all but the last event it received.
        /// </p>
        /// </summary>
        public delegate void xdg_surface_configure_delegate(void* data, xdg_surface* proxy, uint serial);

        internal struct xdg_surface_listener
        {
            public IntPtr configure;

            public static xdg_surface_listener* Alloc(
                xdg_surface_configure_delegate configure)
            {
                var ret = (xdg_surface_listener*) Marshal.AllocHGlobal(sizeof(xdg_surface_listener));
                Set(ret, configure);
                return ret;
            }

            public static void Set(xdg_surface_listener* listener
            ,
                xdg_surface_configure_delegate configure)
            {
                if (configure != null) listener->configure = Marshal.GetFunctionPointerForDelegate<xdg_surface_configure_delegate>(configure);
            }
        }

        /// <summary>
        /// Set the callbacks for the given <see cref="xdg_surface"/>.
        /// </summary>
        /// <param name="serial">serial of the configure event</param>
        public static int xdg_surface_add_listener(xdg_surface* iface, xdg_surface_listener* listener)
        {
            return WaylandClient.wl_proxy_add_listener((wl_proxy*) iface, listener, null);
        }
        /// <summary>
        /// <p>
        /// This request destroys the role surface and unmaps the surface;
        /// see "Unmapping" behavior in interface section for details.
        /// </p>
        /// </summary>
        public static void xdg_toplevel_destroy(xdg_toplevel* pointer)
        {
            WaylandClient.wl_proxy_marshal((wl_proxy*) pointer, 0);
        }

        /// <summary>
        /// <p>
        /// Set the "parent" of this surface. This surface should be stacked
        /// above the parent surface and all other ancestor surfaces.
        /// </p>
        /// <p>
        /// Parent windows should be set on dialogs, toolboxes, or other
        /// "auxiliary" surfaces, so that the parent is raised when the dialog
        /// is raised.
        /// </p>
        /// <p>
        /// Setting a null parent for a child window removes any parent-child
        /// relationship for the child. Setting a null parent for a window which
        /// currently has no parent is a no-op.
        /// </p>
        /// <p>
        /// If the parent is unmapped then its children are managed as
        /// though the parent of the now-unmapped parent has become the
        /// parent of this surface. If no parent exists for the now-unmapped
        /// parent then the children are managed as though they have no
        /// parent surface.
        /// </p>
        /// </summary>
        public static void xdg_toplevel_set_parent(xdg_toplevel* pointer, xdg_toplevel* parent)
        {
            var args = stackalloc wl_argument[1];
            args[0] = parent;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 1, args);
        }

        /// <summary>
        /// <p>
        /// Set a short title for the surface.
        /// </p>
        /// <p>
        /// This string may be used to identify the surface in a task bar,
        /// window list, or other user interface elements provided by the
        /// compositor.
        /// </p>
        /// <p>
        /// The string must be encoded in UTF-8.
        /// </p>
        /// </summary>
        public static void xdg_toplevel_set_title(xdg_toplevel* pointer, string title)
        {
            var titleByteCount = System.Text.Encoding.UTF8.GetByteCount(title);
            var titleBytes = stackalloc byte[titleByteCount + 1];
            Util.StringToUtf8(title, titleBytes, titleByteCount);
            titleBytes[titleByteCount] = 0;
            var args = stackalloc wl_argument[1];
            args[0] = titleBytes;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 2, args);
        }

        /// <summary>
        /// <p>
        /// Set an application identifier for the surface.
        /// </p>
        /// <p>
        /// The app ID identifies the general class of applications to which
        /// the surface belongs. The compositor can use this to group multiple
        /// surfaces together, or to determine how to launch a new application.
        /// </p>
        /// <p>
        /// For D-Bus activatable applications, the app ID is used as the D-Bus
        /// service name.
        /// </p>
        /// <p>
        /// The compositor shell will try to group application surfaces together
        /// by their app ID. As a best practice, it is suggested to select app
        /// ID's that match the basename of the application's .desktop file.
        /// For example, "org.freedesktop.FooViewer" where the .desktop file is
        /// "org.freedesktop.FooViewer.desktop".
        /// </p>
        /// <p>
        /// See the desktop-entry specification [0] for more details on
        /// application identifiers and how they relate to well-known D-Bus
        /// names and .desktop files.
        /// </p>
        /// <p>
        /// [0] http://standards.freedesktop.org/desktop-entry-spec/
        /// </p>
        /// </summary>
        public static void xdg_toplevel_set_app_id(xdg_toplevel* pointer, string app_id)
        {
            var app_idByteCount = System.Text.Encoding.UTF8.GetByteCount(app_id);
            var app_idBytes = stackalloc byte[app_idByteCount + 1];
            Util.StringToUtf8(app_id, app_idBytes, app_idByteCount);
            app_idBytes[app_idByteCount] = 0;
            var args = stackalloc wl_argument[1];
            args[0] = app_idBytes;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 3, args);
        }

        /// <summary>
        /// <p>
        /// Clients implementing client-side decorations might want to show
        /// a context menu when right-clicking on the decorations, giving the
        /// user a menu that they can use to maximize or minimize the window.
        /// </p>
        /// <p>
        /// This request asks the compositor to pop up such a window menu at
        /// the given position, relative to the local surface coordinates of
        /// the parent surface. There are no guarantees as to what menu items
        /// the window menu contains.
        /// </p>
        /// <p>
        /// This request must be used in response to some sort of user action
        /// like a button press, key press, or touch down event.
        /// </p>
        /// </summary>
        /// <param name="seat">the wl_seat of the user event</param>
        /// <param name="serial">the serial of the user event</param>
        /// <param name="x">the x position to pop up the window menu at</param>
        /// <param name="y">the y position to pop up the window menu at</param>
        public static void xdg_toplevel_show_window_menu(xdg_toplevel* pointer, wl_seat* seat, uint serial, int x, int y)
        {
            var args = stackalloc wl_argument[4];
            args[0] = seat;
            args[1] = serial;
            args[2] = x;
            args[3] = y;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 4, args);
        }

        /// <summary>
        /// <p>
        /// Start an interactive, user-driven move of the surface.
        /// </p>
        /// <p>
        /// This request must be used in response to some sort of user action
        /// like a button press, key press, or touch down event. The passed
        /// serial is used to determine the type of interactive move (touch,
        /// pointer, etc).
        /// </p>
        /// <p>
        /// The server may ignore move requests depending on the state of
        /// the surface (e.g. fullscreen or maximized), or if the passed serial
        /// is no longer valid.
        /// </p>
        /// <p>
        /// If triggered, the surface will lose the focus of the device
        /// (wl_pointer, wl_touch, etc) used for the move. It is up to the
        /// compositor to visually indicate that the move is taking place, such as
        /// updating a pointer cursor, during the move. There is no guarantee
        /// that the device focus will return when the move is completed.
        /// </p>
        /// </summary>
        /// <param name="seat">the wl_seat of the user event</param>
        /// <param name="serial">the serial of the user event</param>
        public static void xdg_toplevel_move(xdg_toplevel* pointer, wl_seat* seat, uint serial)
        {
            var args = stackalloc wl_argument[2];
            args[0] = seat;
            args[1] = serial;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 5, args);
        }

        /// <summary>
        /// <p>
        /// Start a user-driven, interactive resize of the surface.
        /// </p>
        /// <p>
        /// This request must be used in response to some sort of user action
        /// like a button press, key press, or touch down event. The passed
        /// serial is used to determine the type of interactive resize (touch,
        /// pointer, etc).
        /// </p>
        /// <p>
        /// The server may ignore resize requests depending on the state of
        /// the surface (e.g. fullscreen or maximized).
        /// </p>
        /// <p>
        /// If triggered, the client will receive configure events with the
        /// "resize" state enum value and the expected sizes. See the "resize"
        /// enum value for more details about what is required. The client
        /// must also acknowledge configure events using "ack_configure". After
        /// the resize is completed, the client will receive another "configure"
        /// event without the resize state.
        /// </p>
        /// <p>
        /// If triggered, the surface also will lose the focus of the device
        /// (wl_pointer, wl_touch, etc) used for the resize. It is up to the
        /// compositor to visually indicate that the resize is taking place,
        /// such as updating a pointer cursor, during the resize. There is no
        /// guarantee that the device focus will return when the resize is
        /// completed.
        /// </p>
        /// <p>
        /// The edges parameter specifies how the surface should be resized,
        /// and is one of the values of the resize_edge enum. The compositor
        /// may use this information to update the surface position for
        /// example when dragging the top left corner. The compositor may also
        /// use this information to adapt its behavior, e.g. choose an
        /// appropriate cursor image.
        /// </p>
        /// </summary>
        /// <param name="seat">the wl_seat of the user event</param>
        /// <param name="serial">the serial of the user event</param>
        /// <param name="edges">which edge or corner is being dragged</param>
        public static void xdg_toplevel_resize(xdg_toplevel* pointer, wl_seat* seat, uint serial, uint edges)
        {
            var args = stackalloc wl_argument[3];
            args[0] = seat;
            args[1] = serial;
            args[2] = edges;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 6, args);
        }

        /// <summary>
        /// <p>
        /// Set a maximum size for the window.
        /// </p>
        /// <p>
        /// The client can specify a maximum size so that the compositor does
        /// not try to configure the window beyond this size.
        /// </p>
        /// <p>
        /// The width and height arguments are in window geometry coordinates.
        /// See xdg_surface.set_window_geometry.
        /// </p>
        /// <p>
        /// Values set in this way are double-buffered. They will get applied
        /// on the next commit.
        /// </p>
        /// <p>
        /// The compositor can use this information to allow or disallow
        /// different states like maximize or fullscreen and draw accurate
        /// animations.
        /// </p>
        /// <p>
        /// Similarly, a tiling window manager may use this information to
        /// place and resize client windows in a more effective way.
        /// </p>
        /// <p>
        /// The client should not rely on the compositor to obey the maximum
        /// size. The compositor may decide to ignore the values set by the
        /// client and request a larger size.
        /// </p>
        /// <p>
        /// If never set, or a value of zero in the request, means that the
        /// client has no expected maximum size in the given dimension.
        /// As a result, a client wishing to reset the maximum size
        /// to an unspecified state can use zero for width and height in the
        /// request.
        /// </p>
        /// <p>
        /// Requesting a maximum size to be smaller than the minimum size of
        /// a surface is illegal and will result in a protocol error.
        /// </p>
        /// <p>
        /// The width and height must be greater than or equal to zero. Using
        /// strictly negative values for width and height will result in a
        /// protocol error.
        /// </p>
        /// </summary>
        public static void xdg_toplevel_set_max_size(xdg_toplevel* pointer, int width, int height)
        {
            var args = stackalloc wl_argument[2];
            args[0] = width;
            args[1] = height;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 7, args);
        }

        /// <summary>
        /// <p>
        /// Set a minimum size for the window.
        /// </p>
        /// <p>
        /// The client can specify a minimum size so that the compositor does
        /// not try to configure the window below this size.
        /// </p>
        /// <p>
        /// The width and height arguments are in window geometry coordinates.
        /// See xdg_surface.set_window_geometry.
        /// </p>
        /// <p>
        /// Values set in this way are double-buffered. They will get applied
        /// on the next commit.
        /// </p>
        /// <p>
        /// The compositor can use this information to allow or disallow
        /// different states like maximize or fullscreen and draw accurate
        /// animations.
        /// </p>
        /// <p>
        /// Similarly, a tiling window manager may use this information to
        /// place and resize client windows in a more effective way.
        /// </p>
        /// <p>
        /// The client should not rely on the compositor to obey the minimum
        /// size. The compositor may decide to ignore the values set by the
        /// client and request a smaller size.
        /// </p>
        /// <p>
        /// If never set, or a value of zero in the request, means that the
        /// client has no expected minimum size in the given dimension.
        /// As a result, a client wishing to reset the minimum size
        /// to an unspecified state can use zero for width and height in the
        /// request.
        /// </p>
        /// <p>
        /// Requesting a minimum size to be larger than the maximum size of
        /// a surface is illegal and will result in a protocol error.
        /// </p>
        /// <p>
        /// The width and height must be greater than or equal to zero. Using
        /// strictly negative values for width and height will result in a
        /// protocol error.
        /// </p>
        /// </summary>
        public static void xdg_toplevel_set_min_size(xdg_toplevel* pointer, int width, int height)
        {
            var args = stackalloc wl_argument[2];
            args[0] = width;
            args[1] = height;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 8, args);
        }

        /// <summary>
        /// <p>
        /// Maximize the surface.
        /// </p>
        /// <p>
        /// After requesting that the surface should be maximized, the compositor
        /// will respond by emitting a configure event. Whether this configure
        /// actually sets the window maximized is subject to compositor policies.
        /// The client must then update its content, drawing in the configured
        /// state. The client must also acknowledge the configure when committing
        /// the new content (see ack_configure).
        /// </p>
        /// <p>
        /// It is up to the compositor to decide how and where to maximize the
        /// surface, for example which output and what region of the screen should
        /// be used.
        /// </p>
        /// <p>
        /// If the surface was already maximized, the compositor will still emit
        /// a configure event with the "maximized" state.
        /// </p>
        /// <p>
        /// If the surface is in a fullscreen state, this request has no direct
        /// effect. It may alter the state the surface is returned to when
        /// unmaximized unless overridden by the compositor.
        /// </p>
        /// </summary>
        public static void xdg_toplevel_set_maximized(xdg_toplevel* pointer)
        {
            WaylandClient.wl_proxy_marshal((wl_proxy*) pointer, 9);
        }

        /// <summary>
        /// <p>
        /// Unmaximize the surface.
        /// </p>
        /// <p>
        /// After requesting that the surface should be unmaximized, the compositor
        /// will respond by emitting a configure event. Whether this actually
        /// un-maximizes the window is subject to compositor policies.
        /// If available and applicable, the compositor will include the window
        /// geometry dimensions the window had prior to being maximized in the
        /// configure event. The client must then update its content, drawing it in
        /// the configured state. The client must also acknowledge the configure
        /// when committing the new content (see ack_configure).
        /// </p>
        /// <p>
        /// It is up to the compositor to position the surface after it was
        /// unmaximized; usually the position the surface had before maximizing, if
        /// applicable.
        /// </p>
        /// <p>
        /// If the surface was already not maximized, the compositor will still
        /// emit a configure event without the "maximized" state.
        /// </p>
        /// <p>
        /// If the surface is in a fullscreen state, this request has no direct
        /// effect. It may alter the state the surface is returned to when
        /// unmaximized unless overridden by the compositor.
        /// </p>
        /// </summary>
        public static void xdg_toplevel_unset_maximized(xdg_toplevel* pointer)
        {
            WaylandClient.wl_proxy_marshal((wl_proxy*) pointer, 10);
        }

        /// <summary>
        /// <p>
        /// Make the surface fullscreen.
        /// </p>
        /// <p>
        /// After requesting that the surface should be fullscreened, the
        /// compositor will respond by emitting a configure event. Whether the
        /// client is actually put into a fullscreen state is subject to compositor
        /// policies. The client must also acknowledge the configure when
        /// committing the new content (see ack_configure).
        /// </p>
        /// <p>
        /// The output passed by the request indicates the client's preference as
        /// to which display it should be set fullscreen on. If this value is NULL,
        /// it's up to the compositor to choose which display will be used to map
        /// this surface.
        /// </p>
        /// <p>
        /// If the surface doesn't cover the whole output, the compositor will
        /// position the surface in the center of the output and compensate with
        /// with border fill covering the rest of the output. The content of the
        /// border fill is undefined, but should be assumed to be in some way that
        /// attempts to blend into the surrounding area (e.g. solid black).
        /// </p>
        /// <p>
        /// If the fullscreened surface is not opaque, the compositor must make
        /// sure that other screen content not part of the same surface tree (made
        /// up of subsurfaces, popups or similarly coupled surfaces) are not
        /// visible below the fullscreened surface.
        /// </p>
        /// </summary>
        public static void xdg_toplevel_set_fullscreen(xdg_toplevel* pointer, wl_output* output)
        {
            var args = stackalloc wl_argument[1];
            args[0] = output;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 11, args);
        }

        /// <summary>
        /// <p>
        /// Make the surface no longer fullscreen.
        /// </p>
        /// <p>
        /// After requesting that the surface should be unfullscreened, the
        /// compositor will respond by emitting a configure event.
        /// Whether this actually removes the fullscreen state of the client is
        /// subject to compositor policies.
        /// </p>
        /// <p>
        /// Making a surface unfullscreen sets states for the surface based on the following:
        /// * the state(s) it may have had before becoming fullscreen
        /// * any state(s) decided by the compositor
        /// * any state(s) requested by the client while the surface was fullscreen
        /// </p>
        /// <p>
        /// The compositor may include the previous window geometry dimensions in
        /// the configure event, if applicable.
        /// </p>
        /// <p>
        /// The client must also acknowledge the configure when committing the new
        /// content (see ack_configure).
        /// </p>
        /// </summary>
        public static void xdg_toplevel_unset_fullscreen(xdg_toplevel* pointer)
        {
            WaylandClient.wl_proxy_marshal((wl_proxy*) pointer, 12);
        }

        /// <summary>
        /// <p>
        /// Request that the compositor minimize your surface. There is no
        /// way to know if the surface is currently minimized, nor is there
        /// any way to unset minimization on this surface.
        /// </p>
        /// <p>
        /// If you are looking to throttle redrawing when minimized, please
        /// instead use the wl_surface.frame event for this, as this will
        /// also work with live previews on windows in Alt-Tab, Expose or
        /// similar compositor features.
        /// </p>
        /// </summary>
        public static void xdg_toplevel_set_minimized(xdg_toplevel* pointer)
        {
            WaylandClient.wl_proxy_marshal((wl_proxy*) pointer, 13);
        }

        /// <summary>
        /// <p>
        /// This configure event asks the client to resize its toplevel surface or
        /// to change its state. The configured state should not be applied
        /// immediately. See xdg_surface.configure for details.
        /// </p>
        /// <p>
        /// The width and height arguments specify a hint to the window
        /// about how its surface should be resized in window geometry
        /// coordinates. See set_window_geometry.
        /// </p>
        /// <p>
        /// If the width or height arguments are zero, it means the client
        /// should decide its own window dimension. This may happen when the
        /// compositor needs to configure the state of the surface but doesn't
        /// have any information about any previous or expected dimension.
        /// </p>
        /// <p>
        /// The states listed in the event specify how the width/height
        /// arguments should be interpreted, and possibly how it should be
        /// drawn.
        /// </p>
        /// <p>
        /// Clients must send an ack_configure in response to this event. See
        /// xdg_surface.configure and xdg_surface.ack_configure for details.
        /// </p>
        /// </summary>
        public delegate void xdg_toplevel_configure_delegate(void* data, xdg_toplevel* proxy, int width, int height, wl_array* states);

        /// <summary>
        /// <p>
        /// The close event is sent by the compositor when the user
        /// wants the surface to be closed. This should be equivalent to
        /// the user clicking the close button in client-side decorations,
        /// if your application has any.
        /// </p>
        /// <p>
        /// This is only a request that the user intends to close the
        /// window. The client may choose to ignore this request, or show
        /// a dialog to ask the user to save their data, etc.
        /// </p>
        /// </summary>
        public delegate void xdg_toplevel_close_delegate(void* data, xdg_toplevel* proxy);

        internal struct xdg_toplevel_listener
        {
            public IntPtr configure;
            public IntPtr close;

            public static xdg_toplevel_listener* Alloc(
                xdg_toplevel_configure_delegate configure,
                xdg_toplevel_close_delegate close)
            {
                var ret = (xdg_toplevel_listener*) Marshal.AllocHGlobal(sizeof(xdg_toplevel_listener));
                Set(ret, configure,close);
                return ret;
            }

            public static void Set(xdg_toplevel_listener* listener
            ,
                xdg_toplevel_configure_delegate configure,
                xdg_toplevel_close_delegate close)
            {
                if (configure != null) listener->configure = Marshal.GetFunctionPointerForDelegate<xdg_toplevel_configure_delegate>(configure);
                if (close != null) listener->close = Marshal.GetFunctionPointerForDelegate<xdg_toplevel_close_delegate>(close);
            }
        }

        /// <summary>
        /// Set the callbacks for the given <see cref="xdg_toplevel"/>.
        /// </summary>
        public static int xdg_toplevel_add_listener(xdg_toplevel* iface, xdg_toplevel_listener* listener)
        {
            return WaylandClient.wl_proxy_add_listener((wl_proxy*) iface, listener, null);
        }
        /// <summary>
        /// <p>
        /// This destroys the popup. Explicitly destroying the xdg_popup
        /// object will also dismiss the popup, and unmap the surface.
        /// </p>
        /// <p>
        /// If this xdg_popup is not the "topmost" popup, a protocol error
        /// will be sent.
        /// </p>
        /// </summary>
        public static void xdg_popup_destroy(xdg_popup* pointer)
        {
            WaylandClient.wl_proxy_marshal((wl_proxy*) pointer, 0);
        }

        /// <summary>
        /// <p>
        /// This request makes the created popup take an explicit grab. An explicit
        /// grab will be dismissed when the user dismisses the popup, or when the
        /// client destroys the xdg_popup. This can be done by the user clicking
        /// outside the surface, using the keyboard, or even locking the screen
        /// through closing the lid or a timeout.
        /// </p>
        /// <p>
        /// If the compositor denies the grab, the popup will be immediately
        /// dismissed.
        /// </p>
        /// <p>
        /// This request must be used in response to some sort of user action like a
        /// button press, key press, or touch down event. The serial number of the
        /// event should be passed as 'serial'.
        /// </p>
        /// <p>
        /// The parent of a grabbing popup must either be an xdg_toplevel surface or
        /// another xdg_popup with an explicit grab. If the parent is another
        /// xdg_popup it means that the popups are nested, with this popup now being
        /// the topmost popup.
        /// </p>
        /// <p>
        /// Nested popups must be destroyed in the reverse order they were created
        /// in, e.g. the only popup you are allowed to destroy at all times is the
        /// topmost one.
        /// </p>
        /// <p>
        /// When compositors choose to dismiss a popup, they may dismiss every
        /// nested grabbing popup as well. When a compositor dismisses popups, it
        /// will follow the same dismissing order as required from the client.
        /// </p>
        /// <p>
        /// The parent of a grabbing popup must either be another xdg_popup with an
        /// active explicit grab, or an xdg_popup or xdg_toplevel, if there are no
        /// explicit grabs already taken.
        /// </p>
        /// <p>
        /// If the topmost grabbing popup is destroyed, the grab will be returned to
        /// the parent of the popup, if that parent previously had an explicit grab.
        /// </p>
        /// <p>
        /// If the parent is a grabbing popup which has already been dismissed, this
        /// popup will be immediately dismissed. If the parent is a popup that did
        /// not take an explicit grab, an error will be raised.
        /// </p>
        /// <p>
        /// During a popup grab, the client owning the grab will receive pointer
        /// and touch events for all their surfaces as normal (similar to an
        /// "owner-events" grab in X11 parlance), while the top most grabbing popup
        /// will always have keyboard focus.
        /// </p>
        /// </summary>
        /// <param name="seat">the wl_seat of the user event</param>
        /// <param name="serial">the serial of the user event</param>
        public static void xdg_popup_grab(xdg_popup* pointer, wl_seat* seat, uint serial)
        {
            var args = stackalloc wl_argument[2];
            args[0] = seat;
            args[1] = serial;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 1, args);
        }

        /// <summary>
        /// <p>
        /// This event asks the popup surface to configure itself given the
        /// configuration. The configured state should not be applied immediately.
        /// See xdg_surface.configure for details.
        /// </p>
        /// <p>
        /// The x and y arguments represent the position the popup was placed at
        /// given the xdg_positioner rule, relative to the upper left corner of the
        /// window geometry of the parent surface.
        /// </p>
        /// </summary>
        public delegate void xdg_popup_configure_delegate(void* data, xdg_popup* proxy, int x, int y, int width, int height);

        /// <summary>
        /// <p>
        /// The popup_done event is sent out when a popup is dismissed by the
        /// compositor. The client should destroy the xdg_popup object at this
        /// point.
        /// </p>
        /// </summary>
        public delegate void xdg_popup_popup_done_delegate(void* data, xdg_popup* proxy);

        internal struct xdg_popup_listener
        {
            public IntPtr configure;
            public IntPtr popup_done;

            public static xdg_popup_listener* Alloc(
                xdg_popup_configure_delegate configure,
                xdg_popup_popup_done_delegate popup_done)
            {
                var ret = (xdg_popup_listener*) Marshal.AllocHGlobal(sizeof(xdg_popup_listener));
                Set(ret, configure,popup_done);
                return ret;
            }

            public static void Set(xdg_popup_listener* listener
            ,
                xdg_popup_configure_delegate configure,
                xdg_popup_popup_done_delegate popup_done)
            {
                if (configure != null) listener->configure = Marshal.GetFunctionPointerForDelegate<xdg_popup_configure_delegate>(configure);
                if (popup_done != null) listener->popup_done = Marshal.GetFunctionPointerForDelegate<xdg_popup_popup_done_delegate>(popup_done);
            }
        }

        /// <summary>
        /// Set the callbacks for the given <see cref="xdg_popup"/>.
        /// </summary>
        /// <param name="x">x position relative to parent surface window geometry</param>
        /// <param name="y">y position relative to parent surface window geometry</param>
        /// <param name="width">window geometry width</param>
        /// <param name="height">window geometry height</param>
        public static int xdg_popup_add_listener(xdg_popup* iface, xdg_popup_listener* listener)
        {
            return WaylandClient.wl_proxy_add_listener((wl_proxy*) iface, listener, null);
        }
    }

    /// <summary>
    /// <p>
    /// The xdg_wm_base interface is exposed as a global object enabling clients
    /// to turn their wl_surfaces into windows in a desktop environment. It
    /// defines the basic functionality needed for clients and the compositor to
    /// create windows that can be dragged, resized, maximized, etc, as well as
    /// creating transient windows such as popup menus.
    /// </p>
    /// </summary>
    internal struct xdg_wm_base { public static unsafe wl_interface* Interface => &XdgShellBindings.Interfaces[0]; }
    /// <summary>
    /// <p>
    /// The xdg_positioner provides a collection of rules for the placement of a
    /// child surface relative to a parent surface. Rules can be defined to ensure
    /// the child surface remains within the visible area's borders, and to
    /// specify how the child surface changes its position, such as sliding along
    /// an axis, or flipping around a rectangle. These positioner-created rules are
    /// constrained by the requirement that a child surface must intersect with or
    /// be at least partially adjacent to its parent surface.
    /// </p>
    /// <p>
    /// See the various requests for details about possible rules.
    /// </p>
    /// <p>
    /// At the time of the request, the compositor makes a copy of the rules
    /// specified by the xdg_positioner. Thus, after the request is complete the
    /// xdg_positioner object can be destroyed or reused; further changes to the
    /// object will have no effect on previous usages.
    /// </p>
    /// <p>
    /// For an xdg_positioner object to be considered complete, it must have a
    /// non-zero size set by set_size, and a non-zero anchor rectangle set by
    /// set_anchor_rect. Passing an incomplete xdg_positioner object when
    /// positioning a surface raises an error.
    /// </p>
    /// </summary>
    internal struct xdg_positioner { public static unsafe wl_interface* Interface => &XdgShellBindings.Interfaces[1]; }
    /// <summary>
    /// <p>
    /// An interface that may be implemented by a wl_surface, for
    /// implementations that provide a desktop-style user interface.
    /// </p>
    /// <p>
    /// It provides a base set of functionality required to construct user
    /// interface elements requiring management by the compositor, such as
    /// toplevel windows, menus, etc. The types of functionality are split into
    /// xdg_surface roles.
    /// </p>
    /// <p>
    /// Creating an xdg_surface does not set the role for a wl_surface. In order
    /// to map an xdg_surface, the client must create a role-specific object
    /// using, e.g., get_toplevel, get_popup. The wl_surface for any given
    /// xdg_surface can have at most one role, and may not be assigned any role
    /// not based on xdg_surface.
    /// </p>
    /// <p>
    /// A role must be assigned before any other requests are made to the
    /// xdg_surface object.
    /// </p>
    /// <p>
    /// The client must call wl_surface.commit on the corresponding wl_surface
    /// for the xdg_surface state to take effect.
    /// </p>
    /// <p>
    /// Creating an xdg_surface from a wl_surface which has a buffer attached or
    /// committed is a client error, and any attempts by a client to attach or
    /// manipulate a buffer prior to the first xdg_surface.configure call must
    /// also be treated as errors.
    /// </p>
    /// <p>
    /// Mapping an xdg_surface-based role surface is defined as making it
    /// possible for the surface to be shown by the compositor. Note that
    /// a mapped surface is not guaranteed to be visible once it is mapped.
    /// </p>
    /// <p>
    /// For an xdg_surface to be mapped by the compositor, the following
    /// conditions must be met:
    /// (1) the client has assigned an xdg_surface-based role to the surface
    /// (2) the client has set and committed the xdg_surface state and the
    /// role-dependent state to the surface
    /// (3) the client has committed a buffer to the surface
    /// </p>
    /// <p>
    /// A newly-unmapped surface is considered to have met condition (1) out
    /// of the 3 required conditions for mapping a surface if its role surface
    /// has not been destroyed.
    /// </p>
    /// </summary>
    internal struct xdg_surface { public static unsafe wl_interface* Interface => &XdgShellBindings.Interfaces[2]; }
    /// <summary>
    /// <p>
    /// This interface defines an xdg_surface role which allows a surface to,
    /// among other things, set window-like properties such as maximize,
    /// fullscreen, and minimize, set application-specific metadata like title and
    /// id, and well as trigger user interactive operations such as interactive
    /// resize and move.
    /// </p>
    /// <p>
    /// Unmapping an xdg_toplevel means that the surface cannot be shown
    /// by the compositor until it is explicitly mapped again.
    /// All active operations (e.g., move, resize) are canceled and all
    /// attributes (e.g. title, state, stacking, ...) are discarded for
    /// an xdg_toplevel surface when it is unmapped.
    /// </p>
    /// <p>
    /// Attaching a null buffer to a toplevel unmaps the surface.
    /// </p>
    /// </summary>
    internal struct xdg_toplevel { public static unsafe wl_interface* Interface => &XdgShellBindings.Interfaces[3]; }
    /// <summary>
    /// <p>
    /// A popup surface is a short-lived, temporary surface. It can be used to
    /// implement for example menus, popovers, tooltips and other similar user
    /// interface concepts.
    /// </p>
    /// <p>
    /// A popup can be made to take an explicit grab. See xdg_popup.grab for
    /// details.
    /// </p>
    /// <p>
    /// When the popup is dismissed, a popup_done event will be sent out, and at
    /// the same time the surface will be unmapped. See the xdg_popup.popup_done
    /// event for details.
    /// </p>
    /// <p>
    /// Explicitly destroying the xdg_popup object will also dismiss the popup and
    /// unmap the surface. Clients that want to dismiss the popup when another
    /// surface of their own is clicked should dismiss the popup using the destroy
    /// request.
    /// </p>
    /// <p>
    /// A newly created xdg_popup will be stacked on top of all previously created
    /// xdg_popup surfaces associated with the same xdg_toplevel.
    /// </p>
    /// <p>
    /// The parent of an xdg_popup must be mapped (see the xdg_surface
    /// description) before the xdg_popup itself.
    /// </p>
    /// <p>
    /// The x and y arguments passed when creating the popup object specify
    /// where the top left of the popup should be placed, relative to the
    /// local surface coordinates of the parent surface. See
    /// xdg_surface.get_popup. An xdg_popup must intersect with or be at least
    /// partially adjacent to its parent surface.
    /// </p>
    /// <p>
    /// The client must call wl_surface.commit on the corresponding wl_surface
    /// for the xdg_popup state to take effect.
    /// </p>
    /// </summary>
    internal struct xdg_popup { public static unsafe wl_interface* Interface => &XdgShellBindings.Interfaces[4]; }


    internal enum xdg_wm_base_error
    {
        /// <summary>
        /// given wl_surface has another role
        /// </summary>
        Role = 0,

        /// <summary>
        /// xdg_wm_base was destroyed before children
        /// </summary>
        DefunctSurfaces = 1,

        /// <summary>
        /// the client tried to map or destroy a non-topmost popup
        /// </summary>
        NotTheTopmostPopup = 2,

        /// <summary>
        /// the client specified an invalid popup parent surface
        /// </summary>
        InvalidPopupParent = 3,

        /// <summary>
        /// the client provided an invalid surface state
        /// </summary>
        InvalidSurfaceState = 4,

        /// <summary>
        /// the client provided an invalid positioner
        /// </summary>
        InvalidPositioner = 5,

    }

    internal enum xdg_positioner_error
    {
        /// <summary>
        /// invalid input provided
        /// </summary>
        InvalidInput = 0,

    }

    internal enum xdg_positioner_anchor
    {
        /// <summary>
        /// </summary>
        None = 0,

        /// <summary>
        /// </summary>
        Top = 1,

        /// <summary>
        /// </summary>
        Bottom = 2,

        /// <summary>
        /// </summary>
        Left = 3,

        /// <summary>
        /// </summary>
        Right = 4,

        /// <summary>
        /// </summary>
        TopLeft = 5,

        /// <summary>
        /// </summary>
        BottomLeft = 6,

        /// <summary>
        /// </summary>
        TopRight = 7,

        /// <summary>
        /// </summary>
        BottomRight = 8,

    }

    internal enum xdg_positioner_gravity
    {
        /// <summary>
        /// </summary>
        None = 0,

        /// <summary>
        /// </summary>
        Top = 1,

        /// <summary>
        /// </summary>
        Bottom = 2,

        /// <summary>
        /// </summary>
        Left = 3,

        /// <summary>
        /// </summary>
        Right = 4,

        /// <summary>
        /// </summary>
        TopLeft = 5,

        /// <summary>
        /// </summary>
        BottomLeft = 6,

        /// <summary>
        /// </summary>
        TopRight = 7,

        /// <summary>
        /// </summary>
        BottomRight = 8,

    }

    /// <summary>
    /// <p>
    /// The constraint adjustment value define ways the compositor will adjust
    /// the position of the surface, if the unadjusted position would result
    /// in the surface being partly constrained.
    /// </p>
    /// <p>
    /// Whether a surface is considered 'constrained' is left to the compositor
    /// to determine. For example, the surface may be partly outside the
    /// compositor's defined 'work area', thus necessitating the child surface's
    /// position be adjusted until it is entirely inside the work area.
    /// </p>
    /// <p>
    /// The adjustments can be combined, according to a defined precedence: 1)
    /// Flip, 2) Slide, 3) Resize.
    /// </p>
    /// </summary>
    [Flags]
    internal enum xdg_positioner_constraint_adjustment
    {
        /// <summary>
        /// </summary>
        None = 0,

        /// <summary>
        /// </summary>
        SlideX = 1,

        /// <summary>
        /// </summary>
        SlideY = 2,

        /// <summary>
        /// </summary>
        FlipX = 4,

        /// <summary>
        /// </summary>
        FlipY = 8,

        /// <summary>
        /// </summary>
        ResizeX = 16,

        /// <summary>
        /// </summary>
        ResizeY = 32,

    }

    internal enum xdg_surface_error
    {
        /// <summary>
        /// </summary>
        NotConstructed = 1,

        /// <summary>
        /// </summary>
        AlreadyConstructed = 2,

        /// <summary>
        /// </summary>
        UnconfiguredBuffer = 3,

    }

    /// <summary>
    /// <p>
    /// These values are used to indicate which edge of a surface
    /// is being dragged in a resize operation.
    /// </p>
    /// </summary>
    internal enum xdg_toplevel_resize_edge
    {
        /// <summary>
        /// </summary>
        None = 0,

        /// <summary>
        /// </summary>
        Top = 1,

        /// <summary>
        /// </summary>
        Bottom = 2,

        /// <summary>
        /// </summary>
        Left = 4,

        /// <summary>
        /// </summary>
        TopLeft = 5,

        /// <summary>
        /// </summary>
        BottomLeft = 6,

        /// <summary>
        /// </summary>
        Right = 8,

        /// <summary>
        /// </summary>
        TopRight = 9,

        /// <summary>
        /// </summary>
        BottomRight = 10,

    }

    /// <summary>
    /// <p>
    /// The different state values used on the surface. This is designed for
    /// state values like maximized, fullscreen. It is paired with the
    /// configure event to ensure that both the client and the compositor
    /// setting the state can be synchronized.
    /// </p>
    /// <p>
    /// States set in this way are double-buffered. They will get applied on
    /// the next commit.
    /// </p>
    /// </summary>
    internal enum xdg_toplevel_state
    {
        /// <summary>
        /// the surface is maximized
        /// </summary>
        Maximized = 1,

        /// <summary>
        /// the surface is fullscreen
        /// </summary>
        Fullscreen = 2,

        /// <summary>
        /// the surface is being resized
        /// </summary>
        Resizing = 3,

        /// <summary>
        /// the surface is now activated
        /// </summary>
        Activated = 4,

        /// <summary>
        /// </summary>
        TiledLeft = 5,

        /// <summary>
        /// </summary>
        TiledRight = 6,

        /// <summary>
        /// </summary>
        TiledTop = 7,

        /// <summary>
        /// </summary>
        TiledBottom = 8,

    }

    internal enum xdg_popup_error
    {
        /// <summary>
        /// tried to grab after being mapped
        /// </summary>
        InvalidGrab = 0,

    }
}

namespace OpenWindow.Backends.Wayland.Managed
{
    internal unsafe partial struct XdgWmBase
    {
        public static IntPtr Interface => (IntPtr) xdg_wm_base.Interface;
        public static XdgWmBase Null => new XdgWmBase();
        public readonly xdg_wm_base* Pointer;
        public bool IsNull => Pointer == null;
        private XdgShellBindings.xdg_wm_base_ping_delegate _ping;
        private XdgShellBindings.xdg_wm_base_listener* _listener;
        public XdgWmBase(xdg_wm_base* ptr) { Pointer = ptr; _listener = null; _ping = null; }
        public static implicit operator XdgWmBase(xdg_wm_base* ptr) => new XdgWmBase(ptr);
        public static explicit operator XdgWmBase(wl_proxy* ptr) => new XdgWmBase((xdg_wm_base*) ptr);
        public void Destroy() => XdgShellBindings.xdg_wm_base_destroy(Pointer);
        public XdgPositioner CreatePositioner() => XdgShellBindings.xdg_wm_base_create_positioner(Pointer);
        public XdgSurface GetXdgSurface(in WlSurface surface) => XdgShellBindings.xdg_wm_base_get_xdg_surface(Pointer, surface.Pointer);
        public void Pong(uint serial) => XdgShellBindings.xdg_wm_base_pong(Pointer, serial);
        public void SetListener(
            XdgShellBindings.xdg_wm_base_ping_delegate ping)
        {
            _ping = ping;
            _listener = XdgShellBindings.xdg_wm_base_listener.Alloc(ping);
            XdgShellBindings.xdg_wm_base_add_listener(Pointer, _listener);
        }
        public void FreeListener() { if (_listener != null) Marshal.FreeHGlobal((IntPtr) _listener); }
    }
    internal unsafe partial struct XdgPositioner
    {
        public static IntPtr Interface => (IntPtr) xdg_positioner.Interface;
        public static XdgPositioner Null => new XdgPositioner();
        public readonly xdg_positioner* Pointer;
        public bool IsNull => Pointer == null;
        public XdgPositioner(xdg_positioner* ptr) { Pointer = ptr; }
        public static implicit operator XdgPositioner(xdg_positioner* ptr) => new XdgPositioner(ptr);
        public static explicit operator XdgPositioner(wl_proxy* ptr) => new XdgPositioner((xdg_positioner*) ptr);
        public void Destroy() => XdgShellBindings.xdg_positioner_destroy(Pointer);
        public void SetSize(int width, int height) => XdgShellBindings.xdg_positioner_set_size(Pointer, width, height);
        public void SetAnchorRect(int x, int y, int width, int height) => XdgShellBindings.xdg_positioner_set_anchor_rect(Pointer, x, y, width, height);
        public void SetAnchor(xdg_positioner_anchor anchor) => XdgShellBindings.xdg_positioner_set_anchor(Pointer, anchor);
        public void SetGravity(xdg_positioner_gravity gravity) => XdgShellBindings.xdg_positioner_set_gravity(Pointer, gravity);
        public void SetConstraintAdjustment(uint constraint_adjustment) => XdgShellBindings.xdg_positioner_set_constraint_adjustment(Pointer, constraint_adjustment);
        public void SetOffset(int x, int y) => XdgShellBindings.xdg_positioner_set_offset(Pointer, x, y);
    }
    internal unsafe partial struct XdgSurface
    {
        public static IntPtr Interface => (IntPtr) xdg_surface.Interface;
        public static XdgSurface Null => new XdgSurface();
        public readonly xdg_surface* Pointer;
        public bool IsNull => Pointer == null;
        private XdgShellBindings.xdg_surface_configure_delegate _configure;
        private XdgShellBindings.xdg_surface_listener* _listener;
        public XdgSurface(xdg_surface* ptr) { Pointer = ptr; _listener = null; _configure = null; }
        public static implicit operator XdgSurface(xdg_surface* ptr) => new XdgSurface(ptr);
        public static explicit operator XdgSurface(wl_proxy* ptr) => new XdgSurface((xdg_surface*) ptr);
        public void Destroy() => XdgShellBindings.xdg_surface_destroy(Pointer);
        public XdgToplevel GetToplevel() => XdgShellBindings.xdg_surface_get_toplevel(Pointer);
        public XdgPopup GetPopup(in XdgSurface parent, in XdgPositioner positioner) => XdgShellBindings.xdg_surface_get_popup(Pointer, parent.Pointer, positioner.Pointer);
        public void SetWindowGeometry(int x, int y, int width, int height) => XdgShellBindings.xdg_surface_set_window_geometry(Pointer, x, y, width, height);
        public void AckConfigure(uint serial) => XdgShellBindings.xdg_surface_ack_configure(Pointer, serial);
        public void SetListener(
            XdgShellBindings.xdg_surface_configure_delegate configure)
        {
            _configure = configure;
            _listener = XdgShellBindings.xdg_surface_listener.Alloc(configure);
            XdgShellBindings.xdg_surface_add_listener(Pointer, _listener);
        }
        public void FreeListener() { if (_listener != null) Marshal.FreeHGlobal((IntPtr) _listener); }
    }
    internal unsafe partial struct XdgToplevel
    {
        public static IntPtr Interface => (IntPtr) xdg_toplevel.Interface;
        public static XdgToplevel Null => new XdgToplevel();
        public readonly xdg_toplevel* Pointer;
        public bool IsNull => Pointer == null;
        private XdgShellBindings.xdg_toplevel_configure_delegate _configure;
        private XdgShellBindings.xdg_toplevel_close_delegate _close;
        private XdgShellBindings.xdg_toplevel_listener* _listener;
        public XdgToplevel(xdg_toplevel* ptr) { Pointer = ptr; _listener = null; _configure = null; _close = null; }
        public static implicit operator XdgToplevel(xdg_toplevel* ptr) => new XdgToplevel(ptr);
        public static explicit operator XdgToplevel(wl_proxy* ptr) => new XdgToplevel((xdg_toplevel*) ptr);
        public void Destroy() => XdgShellBindings.xdg_toplevel_destroy(Pointer);
        public void SetParent(in XdgToplevel parent) => XdgShellBindings.xdg_toplevel_set_parent(Pointer, parent.Pointer);
        public void SetTitle(string title) => XdgShellBindings.xdg_toplevel_set_title(Pointer, title);
        public void SetAppId(string app_id) => XdgShellBindings.xdg_toplevel_set_app_id(Pointer, app_id);
        public void ShowWindowMenu(in WlSeat seat, uint serial, int x, int y) => XdgShellBindings.xdg_toplevel_show_window_menu(Pointer, seat.Pointer, serial, x, y);
        public void Move(in WlSeat seat, uint serial) => XdgShellBindings.xdg_toplevel_move(Pointer, seat.Pointer, serial);
        public void Resize(in WlSeat seat, uint serial, uint edges) => XdgShellBindings.xdg_toplevel_resize(Pointer, seat.Pointer, serial, edges);
        public void SetMaxSize(int width, int height) => XdgShellBindings.xdg_toplevel_set_max_size(Pointer, width, height);
        public void SetMinSize(int width, int height) => XdgShellBindings.xdg_toplevel_set_min_size(Pointer, width, height);
        public void SetMaximized() => XdgShellBindings.xdg_toplevel_set_maximized(Pointer);
        public void UnsetMaximized() => XdgShellBindings.xdg_toplevel_unset_maximized(Pointer);
        public void SetFullscreen(in WlOutput output) => XdgShellBindings.xdg_toplevel_set_fullscreen(Pointer, output.Pointer);
        public void UnsetFullscreen() => XdgShellBindings.xdg_toplevel_unset_fullscreen(Pointer);
        public void SetMinimized() => XdgShellBindings.xdg_toplevel_set_minimized(Pointer);
        public void SetListener(
            XdgShellBindings.xdg_toplevel_configure_delegate configure,
            XdgShellBindings.xdg_toplevel_close_delegate close)
        {
            _configure = configure;
            _close = close;
            _listener = XdgShellBindings.xdg_toplevel_listener.Alloc(configure, close);
            XdgShellBindings.xdg_toplevel_add_listener(Pointer, _listener);
        }
        public void FreeListener() { if (_listener != null) Marshal.FreeHGlobal((IntPtr) _listener); }
    }
    internal unsafe partial struct XdgPopup
    {
        public static IntPtr Interface => (IntPtr) xdg_popup.Interface;
        public static XdgPopup Null => new XdgPopup();
        public readonly xdg_popup* Pointer;
        public bool IsNull => Pointer == null;
        private XdgShellBindings.xdg_popup_configure_delegate _configure;
        private XdgShellBindings.xdg_popup_popup_done_delegate _popup_done;
        private XdgShellBindings.xdg_popup_listener* _listener;
        public XdgPopup(xdg_popup* ptr) { Pointer = ptr; _listener = null; _configure = null; _popup_done = null; }
        public static implicit operator XdgPopup(xdg_popup* ptr) => new XdgPopup(ptr);
        public static explicit operator XdgPopup(wl_proxy* ptr) => new XdgPopup((xdg_popup*) ptr);
        public void Destroy() => XdgShellBindings.xdg_popup_destroy(Pointer);
        public void Grab(in WlSeat seat, uint serial) => XdgShellBindings.xdg_popup_grab(Pointer, seat.Pointer, serial);
        public void SetListener(
            XdgShellBindings.xdg_popup_configure_delegate configure,
            XdgShellBindings.xdg_popup_popup_done_delegate popup_done)
        {
            _configure = configure;
            _popup_done = popup_done;
            _listener = XdgShellBindings.xdg_popup_listener.Alloc(configure, popup_done);
            XdgShellBindings.xdg_popup_add_listener(Pointer, _listener);
        }
        public void FreeListener() { if (_listener != null) Marshal.FreeHGlobal((IntPtr) _listener); }
    }
}
