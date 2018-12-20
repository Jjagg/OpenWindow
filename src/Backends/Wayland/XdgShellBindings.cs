// This file was generated from an xml Wayland protocol specification
// by WaylandSharpGen. https://github.com/Jjagg/OpenWindow/tree/master/generators/WaylandSharpGen

// Protocol: xdg_shell

using System;
using System.Runtime.InteropServices;
using SMarshal = System.Runtime.InteropServices.Marshal;

namespace OpenWindow.Backends.Wayland
{
    internal static partial class XdgShellBindings
    {
        private static bool _initialized;

        public static void Initialize()
        {
            if (_initialized)
                return;
            _initialized = true;

            XdgWmBase.Initialize();
            XdgPositioner.Initialize();
            XdgSurface.Initialize();
            XdgToplevel.Initialize();
            XdgPopup.Initialize();
        }
        public static void Free()
        {
            if (!_initialized)
                return;
            _initialized = false;

            XdgWmBase.Interface.Dispose();
            XdgPositioner.Interface.Dispose();
            XdgSurface.Interface.Dispose();
            XdgToplevel.Interface.Dispose();
            XdgPopup.Interface.Dispose();
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
    internal partial class XdgWmBase : WlProxy
    {
        #region Opcodes

        private const int DestroyOp = 0;
        private const int CreatePositionerOp = 1;
        private const int GetXdgSurfaceOp = 2;
        private const int PongOp = 3;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("xdg_wm_base", 2, 4, 1);
        public const string InterfaceName = "xdg_wm_base";

        internal static void Initialize()
        {
            Interface.SetRequests(new []
            {
                new WlMessage("destroy", "", new IntPtr[0]),
                new WlMessage("create_positioner", "n", new [] {XdgPositioner.Interface.Pointer}),
                new WlMessage("get_xdg_surface", "no", new [] {XdgSurface.Interface.Pointer, WlSurface.Interface.Pointer}),
                new WlMessage("pong", "u", new [] {IntPtr.Zero}),
            });
            Interface.SetEvents(new []
            {
                new WlMessage("ping", "u", new [] {IntPtr.Zero}),
            });
            Interface.Finish();
        }


        #endregion

        public XdgWmBase(IntPtr pointer)
            : base(pointer) { }

        #region Events

        /// <param name="serial">pass this to the pong request</param>
        public delegate void PingHandler(IntPtr data, IntPtr iface, uint serial);

        private IntPtr _listener;
        private bool _setListener;

        /// <summary>
        /// <p>
        /// The ping event asks the client if it's still alive. Pass the
        /// serial specified in the event back to the compositor by sending
        /// a "pong" request back with the specified serial. See xdg_wm_base.ping.
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
        public PingHandler Ping;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            _listener = SMarshal.AllocHGlobal(IntPtr.Size * 1);
            if (Ping != null)
                SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Ping));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        #endregion

        #region Requests

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
        public void Destroy()
        {
            Destroy(Pointer);
        }

        public static void Destroy(IntPtr pointer)
        {
            Marshal(pointer, DestroyOp);
        }

        /// <summary>
        /// <p>
        /// Create a positioner object. A positioner object is used to position
        /// surfaces relative to some parent surface. See the interface description
        /// and xdg_surface.get_popup for details.
        /// </p>
        /// </summary>
        public XdgPositioner CreatePositioner()
        {
            return CreatePositioner(Pointer);
        }

        public static XdgPositioner CreatePositioner(IntPtr pointer)
        {
            var args = new ArgumentStruct[] { 0 };
            var ptr = MarshalArrayConstructor(pointer, CreatePositionerOp, args, XdgPositioner.Interface.Pointer);
            return new XdgPositioner(ptr);
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
        public XdgSurface GetXdgSurface(WlSurface surface)
        {
            return GetXdgSurface(Pointer, surface);
        }

        public static XdgSurface GetXdgSurface(IntPtr pointer, WlSurface surface)
        {
            var args = new ArgumentStruct[] { 0, surface };
            var ptr = MarshalArrayConstructor(pointer, GetXdgSurfaceOp, args, XdgSurface.Interface.Pointer);
            return new XdgSurface(ptr);
        }

        /// <summary>
        /// <p>
        /// A client must respond to a ping event with a pong request or
        /// the client may be deemed unresponsive. See xdg_wm_base.ping.
        /// </p>
        /// </summary>
        /// <param name="serial">serial of the ping event</param>
        public void Pong(uint serial)
        {
            Pong(Pointer, serial);
        }

        public static void Pong(IntPtr pointer, uint serial)
        {
            Marshal(pointer, PongOp);
        }

        #endregion

        #region Enums

        public enum ErrorEnum
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

        #endregion
    }

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
    internal partial class XdgPositioner : WlProxy
    {
        #region Opcodes

        private const int DestroyOp = 0;
        private const int SetSizeOp = 1;
        private const int SetAnchorRectOp = 2;
        private const int SetAnchorOp = 3;
        private const int SetGravityOp = 4;
        private const int SetConstraintAdjustmentOp = 5;
        private const int SetOffsetOp = 6;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("xdg_positioner", 2, 7, 0);
        public const string InterfaceName = "xdg_positioner";

        internal static void Initialize()
        {
            Interface.SetRequests(new []
            {
                new WlMessage("destroy", "", new IntPtr[0]),
                new WlMessage("set_size", "ii", new [] {IntPtr.Zero, IntPtr.Zero}),
                new WlMessage("set_anchor_rect", "iiii", new [] {IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero}),
                new WlMessage("set_anchor", "u", new [] {IntPtr.Zero}),
                new WlMessage("set_gravity", "u", new [] {IntPtr.Zero}),
                new WlMessage("set_constraint_adjustment", "u", new [] {IntPtr.Zero}),
                new WlMessage("set_offset", "ii", new [] {IntPtr.Zero, IntPtr.Zero}),
            });
            Interface.SetEvents(new WlMessage[0]);
            Interface.Finish();
        }


        #endregion

        public XdgPositioner(IntPtr pointer)
            : base(pointer) { }

        #region Requests

        /// <summary>
        /// <p>
        /// Notify the compositor that the xdg_positioner will no longer be used.
        /// </p>
        /// </summary>
        public void Destroy()
        {
            Destroy(Pointer);
        }

        public static void Destroy(IntPtr pointer)
        {
            Marshal(pointer, DestroyOp);
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
        public void SetSize(int width, int height)
        {
            SetSize(Pointer, width, height);
        }

        public static void SetSize(IntPtr pointer, int width, int height)
        {
            var args = new ArgumentStruct[] { width, height };
            MarshalArray(pointer, SetSizeOp, args);
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
        public void SetAnchorRect(int x, int y, int width, int height)
        {
            SetAnchorRect(Pointer, x, y, width, height);
        }

        public static void SetAnchorRect(IntPtr pointer, int x, int y, int width, int height)
        {
            var args = new ArgumentStruct[] { x, y, width, height };
            MarshalArray(pointer, SetAnchorRectOp, args);
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
        public void SetAnchor(AnchorEnum anchor)
        {
            SetAnchor(Pointer, anchor);
        }

        public static void SetAnchor(IntPtr pointer, AnchorEnum anchor)
        {
            Marshal(pointer, SetAnchorOp);
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
        public void SetGravity(GravityEnum gravity)
        {
            SetGravity(Pointer, gravity);
        }

        public static void SetGravity(IntPtr pointer, GravityEnum gravity)
        {
            Marshal(pointer, SetGravityOp);
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
        public void SetConstraintAdjustment(uint constraint_adjustment)
        {
            SetConstraintAdjustment(Pointer, constraint_adjustment);
        }

        public static void SetConstraintAdjustment(IntPtr pointer, uint constraint_adjustment)
        {
            Marshal(pointer, SetConstraintAdjustmentOp);
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
        public void SetOffset(int x, int y)
        {
            SetOffset(Pointer, x, y);
        }

        public static void SetOffset(IntPtr pointer, int x, int y)
        {
            var args = new ArgumentStruct[] { x, y };
            MarshalArray(pointer, SetOffsetOp, args);
        }

        #endregion

        #region Enums

        public enum ErrorEnum
        {
            /// <summary>
            /// invalid input provided
            /// </summary>
            InvalidInput = 0,

        }

        public enum AnchorEnum
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

        public enum GravityEnum
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
        public enum ConstraintAdjustmentEnum
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

        #endregion
    }

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
    internal partial class XdgSurface : WlProxy
    {
        #region Opcodes

        private const int DestroyOp = 0;
        private const int GetToplevelOp = 1;
        private const int GetPopupOp = 2;
        private const int SetWindowGeometryOp = 3;
        private const int AckConfigureOp = 4;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("xdg_surface", 2, 5, 1);
        public const string InterfaceName = "xdg_surface";

        internal static void Initialize()
        {
            Interface.SetRequests(new []
            {
                new WlMessage("destroy", "", new IntPtr[0]),
                new WlMessage("get_toplevel", "n", new [] {XdgToplevel.Interface.Pointer}),
                new WlMessage("get_popup", "n?oo", new [] {XdgPopup.Interface.Pointer, XdgSurface.Interface.Pointer, XdgPositioner.Interface.Pointer}),
                new WlMessage("set_window_geometry", "iiii", new [] {IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero}),
                new WlMessage("ack_configure", "u", new [] {IntPtr.Zero}),
            });
            Interface.SetEvents(new []
            {
                new WlMessage("configure", "u", new [] {IntPtr.Zero}),
            });
            Interface.Finish();
        }


        #endregion

        public XdgSurface(IntPtr pointer)
            : base(pointer) { }

        #region Events

        /// <param name="serial">serial of the configure event</param>
        public delegate void ConfigureHandler(IntPtr data, IntPtr iface, uint serial);

        private IntPtr _listener;
        private bool _setListener;

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
        public ConfigureHandler Configure;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            _listener = SMarshal.AllocHGlobal(IntPtr.Size * 1);
            if (Configure != null)
                SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Configure));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        #endregion

        #region Requests

        /// <summary>
        /// <p>
        /// Destroy the xdg_surface object. An xdg_surface must only be destroyed
        /// after its role object has been destroyed.
        /// </p>
        /// </summary>
        public void Destroy()
        {
            Destroy(Pointer);
        }

        public static void Destroy(IntPtr pointer)
        {
            Marshal(pointer, DestroyOp);
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
        public XdgToplevel GetToplevel()
        {
            return GetToplevel(Pointer);
        }

        public static XdgToplevel GetToplevel(IntPtr pointer)
        {
            var args = new ArgumentStruct[] { 0 };
            var ptr = MarshalArrayConstructor(pointer, GetToplevelOp, args, XdgToplevel.Interface.Pointer);
            return new XdgToplevel(ptr);
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
        public XdgPopup GetPopup(XdgSurface parent, XdgPositioner positioner)
        {
            return GetPopup(Pointer, parent, positioner);
        }

        public static XdgPopup GetPopup(IntPtr pointer, XdgSurface parent, XdgPositioner positioner)
        {
            var args = new ArgumentStruct[] { 0, parent, positioner };
            var ptr = MarshalArrayConstructor(pointer, GetPopupOp, args, XdgPopup.Interface.Pointer);
            return new XdgPopup(ptr);
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
        public void SetWindowGeometry(int x, int y, int width, int height)
        {
            SetWindowGeometry(Pointer, x, y, width, height);
        }

        public static void SetWindowGeometry(IntPtr pointer, int x, int y, int width, int height)
        {
            var args = new ArgumentStruct[] { x, y, width, height };
            MarshalArray(pointer, SetWindowGeometryOp, args);
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
        public void AckConfigure(uint serial)
        {
            AckConfigure(Pointer, serial);
        }

        public static void AckConfigure(IntPtr pointer, uint serial)
        {
            Marshal(pointer, AckConfigureOp);
        }

        #endregion

        #region Enums

        public enum ErrorEnum
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

        #endregion
    }

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
    internal partial class XdgToplevel : WlProxy
    {
        #region Opcodes

        private const int DestroyOp = 0;
        private const int SetParentOp = 1;
        private const int SetTitleOp = 2;
        private const int SetAppIdOp = 3;
        private const int ShowWindowMenuOp = 4;
        private const int MoveOp = 5;
        private const int ResizeOp = 6;
        private const int SetMaxSizeOp = 7;
        private const int SetMinSizeOp = 8;
        private const int SetMaximizedOp = 9;
        private const int UnsetMaximizedOp = 10;
        private const int SetFullscreenOp = 11;
        private const int UnsetFullscreenOp = 12;
        private const int SetMinimizedOp = 13;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("xdg_toplevel", 2, 14, 2);
        public const string InterfaceName = "xdg_toplevel";

        internal static void Initialize()
        {
            Interface.SetRequests(new []
            {
                new WlMessage("destroy", "", new IntPtr[0]),
                new WlMessage("set_parent", "?o", new [] {XdgToplevel.Interface.Pointer}),
                new WlMessage("set_title", "s", new [] {IntPtr.Zero}),
                new WlMessage("set_app_id", "s", new [] {IntPtr.Zero}),
                new WlMessage("show_window_menu", "ouii", new [] {WlSeat.Interface.Pointer, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero}),
                new WlMessage("move", "ou", new [] {WlSeat.Interface.Pointer, IntPtr.Zero}),
                new WlMessage("resize", "ouu", new [] {WlSeat.Interface.Pointer, IntPtr.Zero, IntPtr.Zero}),
                new WlMessage("set_max_size", "ii", new [] {IntPtr.Zero, IntPtr.Zero}),
                new WlMessage("set_min_size", "ii", new [] {IntPtr.Zero, IntPtr.Zero}),
                new WlMessage("set_maximized", "", new IntPtr[0]),
                new WlMessage("unset_maximized", "", new IntPtr[0]),
                new WlMessage("set_fullscreen", "?o", new [] {WlOutput.Interface.Pointer}),
                new WlMessage("unset_fullscreen", "", new IntPtr[0]),
                new WlMessage("set_minimized", "", new IntPtr[0]),
            });
            Interface.SetEvents(new []
            {
                new WlMessage("configure", "iia", new [] {IntPtr.Zero, IntPtr.Zero, IntPtr.Zero}),
                new WlMessage("close", "", new IntPtr[0]),
            });
            Interface.Finish();
        }


        #endregion

        public XdgToplevel(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void ConfigureHandler(IntPtr data, IntPtr iface, int width, int height, WlArray states);

        public delegate void CloseHandler(IntPtr data, IntPtr iface);

        private IntPtr _listener;
        private bool _setListener;

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
        public ConfigureHandler Configure;

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
        public CloseHandler Close;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            _listener = SMarshal.AllocHGlobal(IntPtr.Size * 2);
            if (Configure != null)
                SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Configure));
            if (Close != null)
                SMarshal.WriteIntPtr(_listener, 1 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Close));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        #endregion

        #region Requests

        /// <summary>
        /// <p>
        /// This request destroys the role surface and unmaps the surface;
        /// see "Unmapping" behavior in interface section for details.
        /// </p>
        /// </summary>
        public void Destroy()
        {
            Destroy(Pointer);
        }

        public static void Destroy(IntPtr pointer)
        {
            Marshal(pointer, DestroyOp);
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
        public void SetParent(XdgToplevel parent)
        {
            SetParent(Pointer, parent);
        }

        public static void SetParent(IntPtr pointer, XdgToplevel parent)
        {
            Marshal(pointer, SetParentOp);
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
        public void SetTitle(string title)
        {
            SetTitle(Pointer, title);
        }

        public static void SetTitle(IntPtr pointer, string title)
        {
            var titleStr = SMarshal.StringToHGlobalAnsi(title);
            Marshal(pointer, SetTitleOp);
            SMarshal.FreeHGlobal(titleStr);
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
        public void SetAppId(string app_id)
        {
            SetAppId(Pointer, app_id);
        }

        public static void SetAppId(IntPtr pointer, string app_id)
        {
            var app_idStr = SMarshal.StringToHGlobalAnsi(app_id);
            Marshal(pointer, SetAppIdOp);
            SMarshal.FreeHGlobal(app_idStr);
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
        public void ShowWindowMenu(WlSeat seat, uint serial, int x, int y)
        {
            ShowWindowMenu(Pointer, seat, serial, x, y);
        }

        public static void ShowWindowMenu(IntPtr pointer, WlSeat seat, uint serial, int x, int y)
        {
            var args = new ArgumentStruct[] { seat, serial, x, y };
            MarshalArray(pointer, ShowWindowMenuOp, args);
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
        public void Move(WlSeat seat, uint serial)
        {
            Move(Pointer, seat, serial);
        }

        public static void Move(IntPtr pointer, WlSeat seat, uint serial)
        {
            var args = new ArgumentStruct[] { seat, serial };
            MarshalArray(pointer, MoveOp, args);
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
        public void Resize(WlSeat seat, uint serial, uint edges)
        {
            Resize(Pointer, seat, serial, edges);
        }

        public static void Resize(IntPtr pointer, WlSeat seat, uint serial, uint edges)
        {
            var args = new ArgumentStruct[] { seat, serial, edges };
            MarshalArray(pointer, ResizeOp, args);
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
        public void SetMaxSize(int width, int height)
        {
            SetMaxSize(Pointer, width, height);
        }

        public static void SetMaxSize(IntPtr pointer, int width, int height)
        {
            var args = new ArgumentStruct[] { width, height };
            MarshalArray(pointer, SetMaxSizeOp, args);
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
        public void SetMinSize(int width, int height)
        {
            SetMinSize(Pointer, width, height);
        }

        public static void SetMinSize(IntPtr pointer, int width, int height)
        {
            var args = new ArgumentStruct[] { width, height };
            MarshalArray(pointer, SetMinSizeOp, args);
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
        public void SetMaximized()
        {
            SetMaximized(Pointer);
        }

        public static void SetMaximized(IntPtr pointer)
        {
            Marshal(pointer, SetMaximizedOp);
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
        public void UnsetMaximized()
        {
            UnsetMaximized(Pointer);
        }

        public static void UnsetMaximized(IntPtr pointer)
        {
            Marshal(pointer, UnsetMaximizedOp);
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
        public void SetFullscreen(WlOutput output)
        {
            SetFullscreen(Pointer, output);
        }

        public static void SetFullscreen(IntPtr pointer, WlOutput output)
        {
            Marshal(pointer, SetFullscreenOp);
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
        public void UnsetFullscreen()
        {
            UnsetFullscreen(Pointer);
        }

        public static void UnsetFullscreen(IntPtr pointer)
        {
            Marshal(pointer, UnsetFullscreenOp);
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
        public void SetMinimized()
        {
            SetMinimized(Pointer);
        }

        public static void SetMinimized(IntPtr pointer)
        {
            Marshal(pointer, SetMinimizedOp);
        }

        #endregion

        #region Enums

        /// <summary>
        /// <p>
        /// These values are used to indicate which edge of a surface
        /// is being dragged in a resize operation.
        /// </p>
        /// </summary>
        public enum ResizeEdgeEnum
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
        public enum StateEnum
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

        #endregion
    }

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
    internal partial class XdgPopup : WlProxy
    {
        #region Opcodes

        private const int DestroyOp = 0;
        private const int GrabOp = 1;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("xdg_popup", 2, 2, 2);
        public const string InterfaceName = "xdg_popup";

        internal static void Initialize()
        {
            Interface.SetRequests(new []
            {
                new WlMessage("destroy", "", new IntPtr[0]),
                new WlMessage("grab", "ou", new [] {WlSeat.Interface.Pointer, IntPtr.Zero}),
            });
            Interface.SetEvents(new []
            {
                new WlMessage("configure", "iiii", new [] {IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero}),
                new WlMessage("popup_done", "", new IntPtr[0]),
            });
            Interface.Finish();
        }


        #endregion

        public XdgPopup(IntPtr pointer)
            : base(pointer) { }

        #region Events

        /// <param name="x">x position relative to parent surface window geometry</param>
        /// <param name="y">y position relative to parent surface window geometry</param>
        /// <param name="width">window geometry width</param>
        /// <param name="height">window geometry height</param>
        public delegate void ConfigureHandler(IntPtr data, IntPtr iface, int x, int y, int width, int height);

        public delegate void PopupDoneHandler(IntPtr data, IntPtr iface);

        private IntPtr _listener;
        private bool _setListener;

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
        public ConfigureHandler Configure;

        /// <summary>
        /// <p>
        /// The popup_done event is sent out when a popup is dismissed by the
        /// compositor. The client should destroy the xdg_popup object at this
        /// point.
        /// </p>
        /// </summary>
        public PopupDoneHandler PopupDone;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            _listener = SMarshal.AllocHGlobal(IntPtr.Size * 2);
            if (Configure != null)
                SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Configure));
            if (PopupDone != null)
                SMarshal.WriteIntPtr(_listener, 1 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(PopupDone));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        #endregion

        #region Requests

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
        public void Destroy()
        {
            Destroy(Pointer);
        }

        public static void Destroy(IntPtr pointer)
        {
            Marshal(pointer, DestroyOp);
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
        public void Grab(WlSeat seat, uint serial)
        {
            Grab(Pointer, seat, serial);
        }

        public static void Grab(IntPtr pointer, WlSeat seat, uint serial)
        {
            var args = new ArgumentStruct[] { seat, serial };
            MarshalArray(pointer, GrabOp, args);
        }

        #endregion

        #region Enums

        public enum ErrorEnum
        {
            /// <summary>
            /// tried to grab after being mapped
            /// </summary>
            InvalidGrab = 0,

        }

        #endregion
    }
}
