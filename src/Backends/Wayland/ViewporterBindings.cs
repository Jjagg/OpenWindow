// This file was generated from an xml Wayland protocol specification
// by WaylandSharpGen. https://github.com/Jjagg/OpenWindow/tree/master/generators/WaylandSharpGen

#pragma warning disable CS0649

// Protocol: viewporter

using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.Wayland
{
    internal static unsafe partial class ViewporterBindings
    {
        private static bool _loaded;

        public static wl_interface* Interfaces;
        private static wl_message* _messages;
        private static wl_interface** _signatureTypes;

        private static readonly int InterfaceCount = 2;
        private static readonly int MessageCount = 5;

        public const string wp_viewporter_name = "wp_viewporter";
        public const string wp_viewport_name = "wp_viewport";

        public static void Load()
        {
            if (_loaded)
                return;
            _loaded = true;

            Interfaces = (wl_interface*) Marshal.AllocHGlobal(sizeof(wl_interface) * InterfaceCount);
            _messages = (wl_message*) Marshal.AllocHGlobal(sizeof(wl_message) * MessageCount);


            Util.CreateInterface(&Interfaces[0], "wp_viewporter", 1, 2, 0);
            Util.CreateInterface(&Interfaces[1], "wp_viewport", 1, 3, 0);

            _signatureTypes = (wl_interface**) Marshal.AllocHGlobal(sizeof(void*) * 6);
            _signatureTypes[0] = null;
            _signatureTypes[1] = null;
            _signatureTypes[2] = null;
            _signatureTypes[3] = null;
            _signatureTypes[4] = wp_viewport.Interface;
            _signatureTypes[5] = wl_surface.Interface;

            Util.CreateMessage(&_messages[0], "destroy", "", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[1], "get_viewport", "no", &_signatureTypes[4]);
            Util.CreateMessage(&_messages[2], "destroy", "", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[3], "set_source", "ffff", &_signatureTypes[0]);
            Util.CreateMessage(&_messages[4], "set_destination", "ii", &_signatureTypes[0]);

            Interfaces[0].Requests = &_messages[0];
            Interfaces[0].Events = null;
            Interfaces[1].Requests = &_messages[2];
            Interfaces[1].Events = null;
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
        /// Informs the server that the client will not be using this
        /// protocol object anymore. This does not affect any other objects,
        /// wp_viewport objects included.
        /// </p>
        /// </summary>
        public static void wp_viewporter_destroy(wp_viewporter* pointer)
        {
            WaylandClient.wl_proxy_marshal((wl_proxy*) pointer, 0);
        }

        /// <summary>
        /// <p>
        /// Instantiate an interface extension for the given wl_surface to
        /// crop and scale its content. If the given wl_surface already has
        /// a wp_viewport object associated, the viewport_exists
        /// protocol error is raised.
        /// </p>
        /// </summary>
        /// <param name="id">the new viewport interface id</param>
        /// <param name="surface">the surface</param>
        public static wp_viewport* wp_viewporter_get_viewport(wp_viewporter* pointer, wl_surface* surface)
        {
            var args = stackalloc wl_argument[2];
            args[0] = 0;
            args[1] = surface;
            var ptr = WaylandClient.wl_proxy_marshal_array_constructor((wl_proxy*) pointer, 1, args, wp_viewport.Interface);
            return (wp_viewport*) ptr;
        }

        /// <summary>
        /// <p>
        /// The associated wl_surface's crop and scale state is removed.
        /// The change is applied on the next wl_surface.commit.
        /// </p>
        /// </summary>
        public static void wp_viewport_destroy(wp_viewport* pointer)
        {
            WaylandClient.wl_proxy_marshal((wl_proxy*) pointer, 0);
        }

        /// <summary>
        /// <p>
        /// Set the source rectangle of the associated wl_surface. See
        /// wp_viewport for the description, and relation to the wl_buffer
        /// size.
        /// </p>
        /// <p>
        /// If all of x, y, width and height are -1.0, the source rectangle is
        /// unset instead. Any other set of values where width or height are zero
        /// or negative, or x or y are negative, raise the bad_value protocol
        /// error.
        /// </p>
        /// <p>
        /// The crop and scale state is double-buffered state, and will be
        /// applied on the next wl_surface.commit.
        /// </p>
        /// </summary>
        /// <param name="x">source rectangle x</param>
        /// <param name="y">source rectangle y</param>
        /// <param name="width">source rectangle width</param>
        /// <param name="height">source rectangle height</param>
        public static void wp_viewport_set_source(wp_viewport* pointer, wl_fixed x, wl_fixed y, wl_fixed width, wl_fixed height)
        {
            var args = stackalloc wl_argument[4];
            args[0] = x;
            args[1] = y;
            args[2] = width;
            args[3] = height;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 1, args);
        }

        /// <summary>
        /// <p>
        /// Set the destination size of the associated wl_surface. See
        /// wp_viewport for the description, and relation to the wl_buffer
        /// size.
        /// </p>
        /// <p>
        /// If width is -1 and height is -1, the destination size is unset
        /// instead. Any other pair of values for width and height that
        /// contains zero or negative values raises the bad_value protocol
        /// error.
        /// </p>
        /// <p>
        /// The crop and scale state is double-buffered state, and will be
        /// applied on the next wl_surface.commit.
        /// </p>
        /// </summary>
        /// <param name="width">surface width</param>
        /// <param name="height">surface height</param>
        public static void wp_viewport_set_destination(wp_viewport* pointer, int width, int height)
        {
            var args = stackalloc wl_argument[2];
            args[0] = width;
            args[1] = height;
            WaylandClient.wl_proxy_marshal_array((wl_proxy*) pointer, 2, args);
        }

    }

    /// <summary>
    /// <p>
    /// The global interface exposing surface cropping and scaling
    /// capabilities is used to instantiate an interface extension for a
    /// wl_surface object. This extended interface will then allow
    /// cropping and scaling the surface contents, effectively
    /// disconnecting the direct relationship between the buffer and the
    /// surface size.
    /// </p>
    /// </summary>
    internal struct wp_viewporter { public static unsafe wl_interface* Interface => &ViewporterBindings.Interfaces[0]; }
    /// <summary>
    /// <p>
    /// An additional interface to a wl_surface object, which allows the
    /// client to specify the cropping and scaling of the surface
    /// contents.
    /// </p>
    /// <p>
    /// This interface works with two concepts: the source rectangle (src_x,
    /// src_y, src_width, src_height), and the destination size (dst_width,
    /// dst_height). The contents of the source rectangle are scaled to the
    /// destination size, and content outside the source rectangle is ignored.
    /// This state is double-buffered, and is applied on the next
    /// wl_surface.commit.
    /// </p>
    /// <p>
    /// The two parts of crop and scale state are independent: the source
    /// rectangle, and the destination size. Initially both are unset, that
    /// is, no scaling is applied. The whole of the current wl_buffer is
    /// used as the source, and the surface size is as defined in
    /// wl_surface.attach.
    /// </p>
    /// <p>
    /// If the destination size is set, it causes the surface size to become
    /// dst_width, dst_height. The source (rectangle) is scaled to exactly
    /// this size. This overrides whatever the attached wl_buffer size is,
    /// unless the wl_buffer is NULL. If the wl_buffer is NULL, the surface
    /// has no content and therefore no size. Otherwise, the size is always
    /// at least 1x1 in surface local coordinates.
    /// </p>
    /// <p>
    /// If the source rectangle is set, it defines what area of the wl_buffer is
    /// taken as the source. If the source rectangle is set and the destination
    /// size is not set, then src_width and src_height must be integers, and the
    /// surface size becomes the source rectangle size. This results in cropping
    /// without scaling. If src_width or src_height are not integers and
    /// destination size is not set, the bad_size protocol error is raised when
    /// the surface state is applied.
    /// </p>
    /// <p>
    /// The coordinate transformations from buffer pixel coordinates up to
    /// the surface-local coordinates happen in the following order:
    /// 1. buffer_transform (wl_surface.set_buffer_transform)
    /// 2. buffer_scale (wl_surface.set_buffer_scale)
    /// 3. crop and scale (wp_viewport.set*)
    /// This means, that the source rectangle coordinates of crop and scale
    /// are given in the coordinates after the buffer transform and scale,
    /// i.e. in the coordinates that would be the surface-local coordinates
    /// if the crop and scale was not applied.
    /// </p>
    /// <p>
    /// If src_x or src_y are negative, the bad_value protocol error is raised.
    /// Otherwise, if the source rectangle is partially or completely outside of
    /// the non-NULL wl_buffer, then the out_of_buffer protocol error is raised
    /// when the surface state is applied. A NULL wl_buffer does not raise the
    /// out_of_buffer error.
    /// </p>
    /// <p>
    /// The x, y arguments of wl_surface.attach are applied as normal to
    /// the surface. They indicate how many pixels to remove from the
    /// surface size from the left and the top. In other words, they are
    /// still in the surface-local coordinate system, just like dst_width
    /// and dst_height are.
    /// </p>
    /// <p>
    /// If the wl_surface associated with the wp_viewport is destroyed,
    /// all wp_viewport requests except 'destroy' raise the protocol error
    /// no_surface.
    /// </p>
    /// <p>
    /// If the wp_viewport object is destroyed, the crop and scale
    /// state is removed from the wl_surface. The change will be applied
    /// on the next wl_surface.commit.
    /// </p>
    /// </summary>
    internal struct wp_viewport { public static unsafe wl_interface* Interface => &ViewporterBindings.Interfaces[1]; }


    internal enum wp_viewporter_error
    {
        /// <summary>
        /// the surface already has a viewport object associated
        /// </summary>
        ViewportExists = 0,

    }

    internal enum wp_viewport_error
    {
        /// <summary>
        /// negative or zero values in width or height
        /// </summary>
        BadValue = 0,

        /// <summary>
        /// destination size is not integer
        /// </summary>
        BadSize = 1,

        /// <summary>
        /// source rectangle extends outside of the content area
        /// </summary>
        OutOfBuffer = 2,

        /// <summary>
        /// the wl_surface was destroyed
        /// </summary>
        NoSurface = 3,

    }
}

namespace OpenWindow.Backends.Wayland.Managed
{
    internal unsafe partial struct WpViewporter
    {
        public static IntPtr Interface => (IntPtr) wp_viewporter.Interface;
        public static WpViewporter Null => new WpViewporter();
        public readonly wp_viewporter* Pointer;
        public bool IsNull => Pointer == null;
        public WpViewporter(wp_viewporter* ptr) { Pointer = ptr; }
        public static implicit operator WpViewporter(wp_viewporter* ptr) => new WpViewporter(ptr);
        public static explicit operator WpViewporter(wl_proxy* ptr) => new WpViewporter((wp_viewporter*) ptr);
        public void Destroy() => ViewporterBindings.wp_viewporter_destroy(Pointer);
        public WpViewport GetViewport(in WlSurface surface) => ViewporterBindings.wp_viewporter_get_viewport(Pointer, surface.Pointer);
    }
    internal unsafe partial struct WpViewport
    {
        public static IntPtr Interface => (IntPtr) wp_viewport.Interface;
        public static WpViewport Null => new WpViewport();
        public readonly wp_viewport* Pointer;
        public bool IsNull => Pointer == null;
        public WpViewport(wp_viewport* ptr) { Pointer = ptr; }
        public static implicit operator WpViewport(wp_viewport* ptr) => new WpViewport(ptr);
        public static explicit operator WpViewport(wl_proxy* ptr) => new WpViewport((wp_viewport*) ptr);
        public void Destroy() => ViewporterBindings.wp_viewport_destroy(Pointer);
        public void SetSource(wl_fixed x, wl_fixed y, wl_fixed width, wl_fixed height) => ViewporterBindings.wp_viewport_set_source(Pointer, x, y, width, height);
        public void SetDestination(int width, int height) => ViewporterBindings.wp_viewport_set_destination(Pointer, width, height);
    }
}
