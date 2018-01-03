// This file was generated from an xml Wayland protocol specification
// by WaylandSharpGen. https://github.com/Jjagg/OpenWindow/tree/master/generators/WaylandSharpGen

// Protocol: wayland

using System;
using System.Runtime.InteropServices;
using SMarshal = System.Runtime.InteropServices.Marshal;

namespace OpenWindow.Backends.Wayland
{
    internal static partial class WaylandInterfaces
    {
        public static void CleanUp()
        {
            WlDisplay.CleanUp();
            WlRegistry.CleanUp();
            WlCallback.CleanUp();
            WlCompositor.CleanUp();
            WlShmPool.CleanUp();
            WlShm.CleanUp();
            WlBuffer.CleanUp();
            WlDataOffer.CleanUp();
            WlDataSource.CleanUp();
            WlDataDevice.CleanUp();
            WlDataDeviceManager.CleanUp();
            WlShell.CleanUp();
            WlShellSurface.CleanUp();
            WlSurface.CleanUp();
            WlSeat.CleanUp();
            WlPointer.CleanUp();
            WlKeyboard.CleanUp();
            WlTouch.CleanUp();
            WlOutput.CleanUp();
            WlRegion.CleanUp();
            WlSubcompositor.CleanUp();
            WlSubsurface.CleanUp();
        }
    }

    /// <summary>
    /// The core global object.  This is a special singleton object.  It
    /// is used for internal Wayland protocol features.
    /// </summary>
    internal partial class WlDisplay : WlProxy
    {
        #region Opcodes

        private const int SyncOp = 0;
        private const int GetRegistryOp = 1;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_display", 1, 2, 2);
        public const string InterfaceName = "wl_display";

        private static readonly WlMessage SyncMsg = new WlMessage("sync", "n", new [] {WlCallback.Interface});
        private static readonly WlMessage GetRegistryMsg = new WlMessage("get_registry", "n", new [] {WlRegistry.Interface});

        static WlDisplay()
        {
            Interface.SetRequests(new [] {SyncMsg, GetRegistryMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            SyncMsg.Dispose();
            GetRegistryMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlDisplay(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void ErrorHandler(IntPtr data, IntPtr iface, IntPtr @object_id, uint @code, string @message);
        public delegate void DeleteIdHandler(IntPtr data, IntPtr iface, uint @id);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 2);
        private bool _setListener;

        /// <summary>
        /// The error event is sent out when a fatal (non-recoverable)
        /// error has occurred.  The object_id argument is the object
        /// where the error occurred, most often in response to a request
        /// to that object.  The code identifies the error and is defined
        /// by the object interface.  As such, each interface defines its
        /// own set of error codes.  The message is a brief description
        /// of the error, for (debugging) convenience.
        /// </summary>
        /// <param name="object_id">object where the error occurred</param>
        /// <param name="code">error code</param>
        /// <param name="message">error description</param>
        public ErrorHandler Error;
        /// <summary>
        /// This event is used internally by the object ID management
        /// logic.  When a client deletes an object, the server will send
        /// this event to acknowledge that it has seen the delete request.
        /// When the client receives this event, it will know that it can
        /// safely reuse the object ID.
        /// </summary>
        /// <param name="id">deleted object ID</param>
        public DeleteIdHandler DeleteId;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Error));
            SMarshal.WriteIntPtr(_listener, 1 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(DeleteId));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        /// <summary>
        /// The sync request asks the server to emit the 'done' event
        /// on the returned wl_callback object.  Since requests are
        /// handled in-order and events are delivered in-order, this can
        /// be used as a barrier to ensure all previous requests and the
        /// resulting events have been handled.
        /// 
        /// The object returned by this request will be destroyed by the
        /// compositor after the callback is fired and as such the client must not
        /// attempt to use it after that point.
        /// 
        /// The callback_data passed in the callback is the event serial.
        /// </summary>
        /// <param name="callback">callback object for the sync request</param>
        public WlCallback Sync()
        {
            return Sync(Pointer);
        }

        public static WlCallback Sync(IntPtr pointer)
        {
            var ptr = MarshalConstructor(pointer, SyncOp, WlCallback.Interface.Pointer, IntPtr.Zero);
            return new WlCallback(ptr);
        }

        /// <summary>
        /// This request creates a registry object that allows the client
        /// to list and bind the global objects available from the
        /// compositor.
        /// </summary>
        /// <param name="registry">global registry object</param>
        public WlRegistry GetRegistry()
        {
            return GetRegistry(Pointer);
        }

        public static WlRegistry GetRegistry(IntPtr pointer)
        {
            var ptr = MarshalConstructor(pointer, GetRegistryOp, WlRegistry.Interface.Pointer, IntPtr.Zero);
            return new WlRegistry(ptr);
        }

        #endregion
    }

    /// <summary>
    /// The singleton global registry object.  The server has a number of
    /// global objects that are available to all clients.  These objects
    /// typically represent an actual object in the server (for example,
    /// an input device) or they are singleton objects that provide
    /// extension functionality.
    /// 
    /// When a client creates a registry object, the registry object
    /// will emit a global event for each global currently in the
    /// registry.  Globals come and go as a result of device or
    /// monitor hotplugs, reconfiguration or other events, and the
    /// registry will send out global and global_remove events to
    /// keep the client up to date with the changes.  To mark the end
    /// of the initial burst of events, the client can use the
    /// wl_display.sync request immediately after calling
    /// wl_display.get_registry.
    /// 
    /// A client can bind to a global object by using the bind
    /// request.  This creates a client-side handle that lets the object
    /// emit events to the client and lets the client invoke requests on
    /// the object.
    /// </summary>
    internal partial class WlRegistry : WlProxy
    {
        #region Opcodes

        private const int BindOp = 0;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_registry", 1, 1, 2);
        public const string InterfaceName = "wl_registry";

        private static readonly WlMessage BindMsg = new WlMessage("bind", "un", new WlInterface [0]);

        static WlRegistry()
        {
            Interface.SetRequests(new [] {BindMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            BindMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlRegistry(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void GlobalHandler(IntPtr data, IntPtr iface, uint @name, string @interface, uint @version);
        public delegate void GlobalRemoveHandler(IntPtr data, IntPtr iface, uint @name);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 2);
        private bool _setListener;

        /// <summary>
        /// Notify the client of global objects.
        /// 
        /// The event notifies the client that a global object with
        /// the given name is now available, and it implements the
        /// given version of the given interface.
        /// </summary>
        /// <param name="name">numeric name of the global object</param>
        /// <param name="interface">interface implemented by the object</param>
        /// <param name="version">interface version</param>
        public GlobalHandler Global;
        /// <summary>
        /// Notify the client of removed global objects.
        /// 
        /// This event notifies the client that the global identified
        /// by name is no longer available.  If the client bound to
        /// the global using the bind request, the client should now
        /// destroy that object.
        /// 
        /// The object remains valid and requests to the object will be
        /// ignored until the client destroys it, to avoid races between
        /// the global going away and a client sending a request to it.
        /// </summary>
        /// <param name="name">numeric name of the global object</param>
        public GlobalRemoveHandler GlobalRemove;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Global));
            SMarshal.WriteIntPtr(_listener, 1 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(GlobalRemove));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        /// <summary>
        /// Binds a new, client-created object to the server using the
        /// specified name as the identifier.
        /// </summary>
        /// <param name="name">unique numeric name of the object</param>
        /// <param name="id">bounded object</param>
        public T Bind<T>(uint name, WlInterface iface)
            where T : WlObject
        {
            return Bind<T>(Pointer, name, iface);
        }

        public static T Bind<T>(IntPtr pointer, uint name, WlInterface iface)
            where T : WlObject
        {
            var args = new ArgumentList(name);
            var ptr = MarshalArrayConstructor(pointer, BindOp, args.Pointer, iface.Pointer);
            args.Dispose();
            return (T) Activator.CreateInstance(typeof(T), new [] { ptr });
        }

        #endregion
    }

    /// <summary>
    /// Clients can handle the 'done' event to get notified when
    /// the related request is done.
    /// </summary>
    internal partial class WlCallback : WlObject
    {
        #region Opcodes


        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_callback", 1, 0, 1);
        public const string InterfaceName = "wl_callback";


        static WlCallback()
        {
            Interface.SetRequests(new WlMessage[0]);
            Interface.Finish();
        }

        public static void CleanUp()
        {
            Interface.Dispose();
        }

        #endregion

        public WlCallback(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void DoneHandler(IntPtr data, IntPtr iface, uint @callback_data);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 1);
        private bool _setListener;

        /// <summary>
        /// Notify the client when the related request is done.
        /// </summary>
        /// <param name="callback_data">request-specific data for the callback</param>
        public DoneHandler Done;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Done));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        #endregion
    }

    /// <summary>
    /// A compositor.  This object is a singleton global.  The
    /// compositor is in charge of combining the contents of multiple
    /// surfaces into one displayable output.
    /// </summary>
    internal partial class WlCompositor : WlProxy
    {
        #region Opcodes

        private const int CreateSurfaceOp = 0;
        private const int CreateRegionOp = 1;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_compositor", 4, 2, 0);
        public const string InterfaceName = "wl_compositor";

        private static readonly WlMessage CreateSurfaceMsg = new WlMessage("create_surface", "n", new [] {WlSurface.Interface});
        private static readonly WlMessage CreateRegionMsg = new WlMessage("create_region", "n", new [] {WlRegion.Interface});

        static WlCompositor()
        {
            Interface.SetRequests(new [] {CreateSurfaceMsg, CreateRegionMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            CreateSurfaceMsg.Dispose();
            CreateRegionMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlCompositor(IntPtr pointer)
            : base(pointer) { }

        #region Events


        #endregion

        #region Requests

        /// <summary>
        /// Ask the compositor to create a new surface.
        /// </summary>
        /// <param name="id">the new surface</param>
        public WlSurface CreateSurface()
        {
            return CreateSurface(Pointer);
        }

        public static WlSurface CreateSurface(IntPtr pointer)
        {
            var ptr = MarshalConstructor(pointer, CreateSurfaceOp, WlSurface.Interface.Pointer, IntPtr.Zero);
            return new WlSurface(ptr);
        }

        /// <summary>
        /// Ask the compositor to create a new region.
        /// </summary>
        /// <param name="id">the new region</param>
        public WlRegion CreateRegion()
        {
            return CreateRegion(Pointer);
        }

        public static WlRegion CreateRegion(IntPtr pointer)
        {
            var ptr = MarshalConstructor(pointer, CreateRegionOp, WlRegion.Interface.Pointer, IntPtr.Zero);
            return new WlRegion(ptr);
        }

        #endregion
    }

    /// <summary>
    /// The wl_shm_pool object encapsulates a piece of memory shared
    /// between the compositor and client.  Through the wl_shm_pool
    /// object, the client can allocate shared memory wl_buffer objects.
    /// All objects created through the same pool share the same
    /// underlying mapped memory. Reusing the mapped memory avoids the
    /// setup/teardown overhead and is useful when interactively resizing
    /// a surface or for many small buffers.
    /// </summary>
    internal partial class WlShmPool : WlProxy
    {
        #region Opcodes

        private const int CreateBufferOp = 0;
        private const int DestroyOp = 1;
        private const int ResizeOp = 2;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_shm_pool", 1, 3, 0);
        public const string InterfaceName = "wl_shm_pool";

        private static readonly WlMessage CreateBufferMsg = new WlMessage("create_buffer", "niiiiu", new [] {WlBuffer.Interface});
        private static readonly WlMessage DestroyMsg = new WlMessage("destroy", "", new WlInterface [0]);
        private static readonly WlMessage ResizeMsg = new WlMessage("resize", "i", new WlInterface [0]);

        static WlShmPool()
        {
            Interface.SetRequests(new [] {CreateBufferMsg, DestroyMsg, ResizeMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            CreateBufferMsg.Dispose();
            DestroyMsg.Dispose();
            ResizeMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlShmPool(IntPtr pointer)
            : base(pointer) { }

        #region Events


        #endregion

        #region Requests

        /// <summary>
        /// Create a wl_buffer object from the pool.
        /// 
        /// The buffer is created offset bytes into the pool and has
        /// width and height as specified.  The stride argument specifies
        /// the number of bytes from the beginning of one row to the beginning
        /// of the next.  The format is the pixel format of the buffer and
        /// must be one of those advertised through the wl_shm.format event.
        /// 
        /// A buffer will keep a reference to the pool it was created from
        /// so it is valid to destroy the pool immediately after creating
        /// a buffer from it.
        /// </summary>
        /// <param name="id">buffer to create</param>
        /// <param name="offset">buffer byte offset within the pool</param>
        /// <param name="width">buffer width, in pixels</param>
        /// <param name="height">buffer height, in pixels</param>
        /// <param name="stride">number of bytes from the beginning of one row to the beginning of the next row</param>
        /// <param name="format">buffer pixel format</param>
        public WlBuffer CreateBuffer(int offset, int width, int height, int stride, uint format)
        {
            return CreateBuffer(Pointer, offset, width, height, stride, format);
        }

        public static WlBuffer CreateBuffer(IntPtr pointer, int offset, int width, int height, int stride, uint format)
        {
            var args = new ArgumentList(offset, width, height, stride, format);
            var ptr = MarshalArrayConstructor(pointer, CreateBufferOp, args.Pointer, WlBuffer.Interface.Pointer);
            args.Dispose();
            return new WlBuffer(ptr);
        }

        /// <summary>
        /// Destroy the shared memory pool.
        /// 
        /// The mmapped memory will be released when all
        /// buffers that have been created from this pool
        /// are gone.
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
        /// This request will cause the server to remap the backing memory
        /// for the pool from the file descriptor passed when the pool was
        /// created, but using the new size.  This request can only be
        /// used to make the pool bigger.
        /// </summary>
        /// <param name="size">new size of the pool, in bytes</param>
        public void Resize(int size)
        {
            Resize(Pointer, size);
        }

        public static void Resize(IntPtr pointer, int size)
        {
            Marshal(pointer, ResizeOp);
        }

        #endregion
    }

    /// <summary>
    /// A singleton global object that provides support for shared
    /// memory.
    /// 
    /// Clients can create wl_shm_pool objects using the create_pool
    /// request.
    /// 
    /// At connection setup time, the wl_shm object emits one or more
    /// format events to inform clients about the valid pixel formats
    /// that can be used for buffers.
    /// </summary>
    internal partial class WlShm : WlProxy
    {
        #region Opcodes

        private const int CreatePoolOp = 0;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_shm", 1, 1, 1);
        public const string InterfaceName = "wl_shm";

        private static readonly WlMessage CreatePoolMsg = new WlMessage("create_pool", "nhi", new [] {WlShmPool.Interface});

        static WlShm()
        {
            Interface.SetRequests(new [] {CreatePoolMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            CreatePoolMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlShm(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void FormatHandler(IntPtr data, IntPtr iface, uint @format);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 1);
        private bool _setListener;

        /// <summary>
        /// Informs the client about a valid pixel format that
        /// can be used for buffers. Known formats include
        /// argb8888 and xrgb8888.
        /// </summary>
        /// <param name="format">buffer pixel format</param>
        public FormatHandler Format;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Format));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        /// <summary>
        /// Create a new wl_shm_pool object.
        /// 
        /// The pool can be used to create shared memory based buffer
        /// objects.  The server will mmap size bytes of the passed file
        /// descriptor, to use as backing memory for the pool.
        /// </summary>
        /// <param name="id">pool to create</param>
        /// <param name="fd">file descriptor for the pool</param>
        /// <param name="size">pool size, in bytes</param>
        public WlShmPool CreatePool(int fd, int size)
        {
            return CreatePool(Pointer, fd, size);
        }

        public static WlShmPool CreatePool(IntPtr pointer, int fd, int size)
        {
            var args = new ArgumentList(fd, size);
            var ptr = MarshalArrayConstructor(pointer, CreatePoolOp, args.Pointer, WlShmPool.Interface.Pointer);
            args.Dispose();
            return new WlShmPool(ptr);
        }

        #endregion
    }

    /// <summary>
    /// A buffer provides the content for a wl_surface. Buffers are
    /// created through factory interfaces such as wl_drm, wl_shm or
    /// similar. It has a width and a height and can be attached to a
    /// wl_surface, but the mechanism by which a client provides and
    /// updates the contents is defined by the buffer factory interface.
    /// </summary>
    internal partial class WlBuffer : WlProxy
    {
        #region Opcodes

        private const int DestroyOp = 0;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_buffer", 1, 1, 1);
        public const string InterfaceName = "wl_buffer";

        private static readonly WlMessage DestroyMsg = new WlMessage("destroy", "", new WlInterface [0]);

        static WlBuffer()
        {
            Interface.SetRequests(new [] {DestroyMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            DestroyMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlBuffer(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void ReleaseHandler(IntPtr data, IntPtr iface);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 1);
        private bool _setListener;

        /// <summary>
        /// Sent when this wl_buffer is no longer used by the compositor.
        /// The client is now free to reuse or destroy this buffer and its
        /// backing storage.
        /// 
        /// If a client receives a release event before the frame callback
        /// requested in the same wl_surface.commit that attaches this
        /// wl_buffer to a surface, then the client is immediately free to
        /// reuse the buffer and its backing storage, and does not need a
        /// second buffer for the next surface content update. Typically
        /// this is possible, when the compositor maintains a copy of the
        /// wl_surface contents, e.g. as a GL texture. This is an important
        /// optimization for GL(ES) compositors with wl_shm clients.
        /// </summary>
        public ReleaseHandler Release;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Release));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        /// <summary>
        /// Destroy a buffer. If and how you need to release the backing
        /// storage is defined by the buffer factory interface.
        /// 
        /// For possible side-effects to a surface, see wl_surface.attach.
        /// </summary>
        public void Destroy()
        {
            Destroy(Pointer);
        }

        public static void Destroy(IntPtr pointer)
        {
            Marshal(pointer, DestroyOp);
        }

        #endregion
    }

    /// <summary>
    /// A wl_data_offer represents a piece of data offered for transfer
    /// by another client (the source client).  It is used by the
    /// copy-and-paste and drag-and-drop mechanisms.  The offer
    /// describes the different mime types that the data can be
    /// converted to and provides the mechanism for transferring the
    /// data directly from the source client.
    /// </summary>
    internal partial class WlDataOffer : WlProxy
    {
        #region Opcodes

        private const int AcceptOp = 0;
        private const int ReceiveOp = 1;
        private const int DestroyOp = 2;
        private const int FinishOp = 3;
        private const int SetActionsOp = 4;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_data_offer", 3, 5, 3);
        public const string InterfaceName = "wl_data_offer";

        private static readonly WlMessage AcceptMsg = new WlMessage("accept", "u?s", new WlInterface [0]);
        private static readonly WlMessage ReceiveMsg = new WlMessage("receive", "sh", new WlInterface [0]);
        private static readonly WlMessage DestroyMsg = new WlMessage("destroy", "", new WlInterface [0]);
        private static readonly WlMessage FinishMsg = new WlMessage("finish", "", new WlInterface [0]);
        private static readonly WlMessage SetActionsMsg = new WlMessage("set_actions", "uu", new WlInterface [0]);

        static WlDataOffer()
        {
            Interface.SetRequests(new [] {AcceptMsg, ReceiveMsg, DestroyMsg, FinishMsg, SetActionsMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            AcceptMsg.Dispose();
            ReceiveMsg.Dispose();
            DestroyMsg.Dispose();
            FinishMsg.Dispose();
            SetActionsMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlDataOffer(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void OfferHandler(IntPtr data, IntPtr iface, string @mime_type);
        public delegate void SourceActionsHandler(IntPtr data, IntPtr iface, uint @source_actions);
        public delegate void ActionHandler(IntPtr data, IntPtr iface, uint @dnd_action);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 3);
        private bool _setListener;

        /// <summary>
        /// Sent immediately after creating the wl_data_offer object.  One
        /// event per offered mime type.
        /// </summary>
        /// <param name="mime_type">offered mime type</param>
        public OfferHandler Offer;
        /// <summary>
        /// This event indicates the actions offered by the data source. It
        /// will be sent right after wl_data_device.enter, or anytime the source
        /// side changes its offered actions through wl_data_source.set_actions.
        /// </summary>
        /// <param name="source_actions">actions offered by the data source</param>
        public SourceActionsHandler SourceActions;
        /// <summary>
        /// This event indicates the action selected by the compositor after
        /// matching the source/destination side actions. Only one action (or
        /// none) will be offered here.
        /// 
        /// This event can be emitted multiple times during the drag-and-drop
        /// operation in response to destination side action changes through
        /// wl_data_offer.set_actions.
        /// 
        /// This event will no longer be emitted after wl_data_device.drop
        /// happened on the drag-and-drop destination, the client must
        /// honor the last action received, or the last preferred one set
        /// through wl_data_offer.set_actions when handling an "ask" action.
        /// 
        /// Compositors may also change the selected action on the fly, mainly
        /// in response to keyboard modifier changes during the drag-and-drop
        /// operation.
        /// 
        /// The most recent action received is always the valid one. Prior to
        /// receiving wl_data_device.drop, the chosen action may change (e.g.
        /// due to keyboard modifiers being pressed). At the time of receiving
        /// wl_data_device.drop the drag-and-drop destination must honor the
        /// last action received.
        /// 
        /// Action changes may still happen after wl_data_device.drop,
        /// especially on "ask" actions, where the drag-and-drop destination
        /// may choose another action afterwards. Action changes happening
        /// at this stage are always the result of inter-client negotiation, the
        /// compositor shall no longer be able to induce a different action.
        /// 
        /// Upon "ask" actions, it is expected that the drag-and-drop destination
        /// may potentially choose a different action and/or mime type,
        /// based on wl_data_offer.source_actions and finally chosen by the
        /// user (e.g. popping up a menu with the available options). The
        /// final wl_data_offer.set_actions and wl_data_offer.accept requests
        /// must happen before the call to wl_data_offer.finish.
        /// </summary>
        /// <param name="dnd_action">action selected by the compositor</param>
        public ActionHandler Action;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Offer));
            SMarshal.WriteIntPtr(_listener, 1 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(SourceActions));
            SMarshal.WriteIntPtr(_listener, 2 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Action));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        /// <summary>
        /// Indicate that the client can accept the given mime type, or
        /// NULL for not accepted.
        /// 
        /// For objects of version 2 or older, this request is used by the
        /// client to give feedback whether the client can receive the given
        /// mime type, or NULL if none is accepted; the feedback does not
        /// determine whether the drag-and-drop operation succeeds or not.
        /// 
        /// For objects of version 3 or newer, this request determines the
        /// final result of the drag-and-drop operation. If the end result
        /// is that no mime types were accepted, the drag-and-drop operation
        /// will be cancelled and the corresponding drag source will receive
        /// wl_data_source.cancelled. Clients may still use this event in
        /// conjunction with wl_data_source.action for feedback.
        /// </summary>
        /// <param name="serial">serial number of the accept request</param>
        /// <param name="mime_type">mime type accepted by the client</param>
        public void Accept(uint serial, string mime_type)
        {
            Accept(Pointer, serial, mime_type);
        }

        public static void Accept(IntPtr pointer, uint serial, string mime_type)
        {
            var args = new ArgumentList(serial, mime_type);
            MarshalArray(pointer, AcceptOp, args.Pointer);
            args.Dispose();
        }

        /// <summary>
        /// To transfer the offered data, the client issues this request
        /// and indicates the mime type it wants to receive.  The transfer
        /// happens through the passed file descriptor (typically created
        /// with the pipe system call).  The source client writes the data
        /// in the mime type representation requested and then closes the
        /// file descriptor.
        /// 
        /// The receiving client reads from the read end of the pipe until
        /// EOF and then closes its end, at which point the transfer is
        /// complete.
        /// 
        /// This request may happen multiple times for different mime types,
        /// both before and after wl_data_device.drop. Drag-and-drop destination
        /// clients may preemptively fetch data or examine it more closely to
        /// determine acceptance.
        /// </summary>
        /// <param name="mime_type">mime type desired by receiver</param>
        /// <param name="fd">file descriptor for data transfer</param>
        public void Receive(string mime_type, int fd)
        {
            Receive(Pointer, mime_type, fd);
        }

        public static void Receive(IntPtr pointer, string mime_type, int fd)
        {
            var args = new ArgumentList(mime_type, fd);
            MarshalArray(pointer, ReceiveOp, args.Pointer);
            args.Dispose();
        }

        /// <summary>
        /// Destroy the data offer.
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
        /// Notifies the compositor that the drag destination successfully
        /// finished the drag-and-drop operation.
        /// 
        /// Upon receiving this request, the compositor will emit
        /// wl_data_source.dnd_finished on the drag source client.
        /// 
        /// It is a client error to perform other requests than
        /// wl_data_offer.destroy after this one. It is also an error to perform
        /// this request after a NULL mime type has been set in
        /// wl_data_offer.accept or no action was received through
        /// wl_data_offer.action.
        /// </summary>
        public void Finish()
        {
            Finish(Pointer);
        }

        public static void Finish(IntPtr pointer)
        {
            Marshal(pointer, FinishOp);
        }

        /// <summary>
        /// Sets the actions that the destination side client supports for
        /// this operation. This request may trigger the emission of
        /// wl_data_source.action and wl_data_offer.action events if the compositor
        /// needs to change the selected action.
        /// 
        /// This request can be called multiple times throughout the
        /// drag-and-drop operation, typically in response to wl_data_device.enter
        /// or wl_data_device.motion events.
        /// 
        /// This request determines the final result of the drag-and-drop
        /// operation. If the end result is that no action is accepted,
        /// the drag source will receive wl_drag_source.cancelled.
        /// 
        /// The dnd_actions argument must contain only values expressed in the
        /// wl_data_device_manager.dnd_actions enum, and the preferred_action
        /// argument must only contain one of those values set, otherwise it
        /// will result in a protocol error.
        /// 
        /// While managing an "ask" action, the destination drag-and-drop client
        /// may perform further wl_data_offer.receive requests, and is expected
        /// to perform one last wl_data_offer.set_actions request with a preferred
        /// action other than "ask" (and optionally wl_data_offer.accept) before
        /// requesting wl_data_offer.finish, in order to convey the action selected
        /// by the user. If the preferred action is not in the
        /// wl_data_offer.source_actions mask, an error will be raised.
        /// 
        /// If the "ask" action is dismissed (e.g. user cancellation), the client
        /// is expected to perform wl_data_offer.destroy right away.
        /// 
        /// This request can only be made on drag-and-drop offers, a protocol error
        /// will be raised otherwise.
        /// </summary>
        /// <param name="dnd_actions">actions supported by the destination client</param>
        /// <param name="preferred_action">action preferred by the destination client</param>
        public void SetActions(uint dnd_actions, uint preferred_action)
        {
            SetActions(Pointer, dnd_actions, preferred_action);
        }

        public static void SetActions(IntPtr pointer, uint dnd_actions, uint preferred_action)
        {
            var args = new ArgumentList(dnd_actions, preferred_action);
            MarshalArray(pointer, SetActionsOp, args.Pointer);
            args.Dispose();
        }

        #endregion
    }

    /// <summary>
    /// The wl_data_source object is the source side of a wl_data_offer.
    /// It is created by the source client in a data transfer and
    /// provides a way to describe the offered data and a way to respond
    /// to requests to transfer the data.
    /// </summary>
    internal partial class WlDataSource : WlProxy
    {
        #region Opcodes

        private const int OfferOp = 0;
        private const int DestroyOp = 1;
        private const int SetActionsOp = 2;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_data_source", 3, 3, 6);
        public const string InterfaceName = "wl_data_source";

        private static readonly WlMessage OfferMsg = new WlMessage("offer", "s", new WlInterface [0]);
        private static readonly WlMessage DestroyMsg = new WlMessage("destroy", "", new WlInterface [0]);
        private static readonly WlMessage SetActionsMsg = new WlMessage("set_actions", "u", new WlInterface [0]);

        static WlDataSource()
        {
            Interface.SetRequests(new [] {OfferMsg, DestroyMsg, SetActionsMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            OfferMsg.Dispose();
            DestroyMsg.Dispose();
            SetActionsMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlDataSource(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void TargetHandler(IntPtr data, IntPtr iface, string @mime_type);
        public delegate void SendHandler(IntPtr data, IntPtr iface, string @mime_type, int @fd);
        public delegate void CancelledHandler(IntPtr data, IntPtr iface);
        public delegate void DndDropPerformedHandler(IntPtr data, IntPtr iface);
        public delegate void DndFinishedHandler(IntPtr data, IntPtr iface);
        public delegate void ActionHandler(IntPtr data, IntPtr iface, uint @dnd_action);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 6);
        private bool _setListener;

        /// <summary>
        /// Sent when a target accepts pointer_focus or motion events.  If
        /// a target does not accept any of the offered types, type is NULL.
        /// 
        /// Used for feedback during drag-and-drop.
        /// </summary>
        /// <param name="mime_type">mime type accepted by the target</param>
        public TargetHandler Target;
        /// <summary>
        /// Request for data from the client.  Send the data as the
        /// specified mime type over the passed file descriptor, then
        /// close it.
        /// </summary>
        /// <param name="mime_type">mime type for the data</param>
        /// <param name="fd">file descriptor for the data</param>
        public SendHandler Send;
        /// <summary>
        /// This data source is no longer valid. There are several reasons why
        /// this could happen:
        /// 
        /// - The data source has been replaced by another data source.
        /// - The drag-and-drop operation was performed, but the drop destination
        /// did not accept any of the mime types offered through
        /// wl_data_source.target.
        /// - The drag-and-drop operation was performed, but the drop destination
        /// did not select any of the actions present in the mask offered through
        /// wl_data_source.action.
        /// - The drag-and-drop operation was performed but didn't happen over a
        /// surface.
        /// - The compositor cancelled the drag-and-drop operation (e.g. compositor
        /// dependent timeouts to avoid stale drag-and-drop transfers).
        /// 
        /// The client should clean up and destroy this data source.
        /// 
        /// For objects of version 2 or older, wl_data_source.cancelled will
        /// only be emitted if the data source was replaced by another data
        /// source.
        /// </summary>
        public CancelledHandler Cancelled;
        /// <summary>
        /// The user performed the drop action. This event does not indicate
        /// acceptance, wl_data_source.cancelled may still be emitted afterwards
        /// if the drop destination does not accept any mime type.
        /// 
        /// However, this event might however not be received if the compositor
        /// cancelled the drag-and-drop operation before this event could happen.
        /// 
        /// Note that the data_source may still be used in the future and should
        /// not be destroyed here.
        /// </summary>
        public DndDropPerformedHandler DndDropPerformed;
        /// <summary>
        /// The drop destination finished interoperating with this data
        /// source, so the client is now free to destroy this data source and
        /// free all associated data.
        /// 
        /// If the action used to perform the operation was "move", the
        /// source can now delete the transferred data.
        /// </summary>
        public DndFinishedHandler DndFinished;
        /// <summary>
        /// This event indicates the action selected by the compositor after
        /// matching the source/destination side actions. Only one action (or
        /// none) will be offered here.
        /// 
        /// This event can be emitted multiple times during the drag-and-drop
        /// operation, mainly in response to destination side changes through
        /// wl_data_offer.set_actions, and as the data device enters/leaves
        /// surfaces.
        /// 
        /// It is only possible to receive this event after
        /// wl_data_source.dnd_drop_performed if the drag-and-drop operation
        /// ended in an "ask" action, in which case the final wl_data_source.action
        /// event will happen immediately before wl_data_source.dnd_finished.
        /// 
        /// Compositors may also change the selected action on the fly, mainly
        /// in response to keyboard modifier changes during the drag-and-drop
        /// operation.
        /// 
        /// The most recent action received is always the valid one. The chosen
        /// action may change alongside negotiation (e.g. an "ask" action can turn
        /// into a "move" operation), so the effects of the final action must
        /// always be applied in wl_data_offer.dnd_finished.
        /// 
        /// Clients can trigger cursor surface changes from this point, so
        /// they reflect the current action.
        /// </summary>
        /// <param name="dnd_action">action selected by the compositor</param>
        public ActionHandler Action;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Target));
            SMarshal.WriteIntPtr(_listener, 1 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Send));
            SMarshal.WriteIntPtr(_listener, 2 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Cancelled));
            SMarshal.WriteIntPtr(_listener, 3 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(DndDropPerformed));
            SMarshal.WriteIntPtr(_listener, 4 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(DndFinished));
            SMarshal.WriteIntPtr(_listener, 5 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Action));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        /// <summary>
        /// This request adds a mime type to the set of mime types
        /// advertised to targets.  Can be called several times to offer
        /// multiple types.
        /// </summary>
        /// <param name="mime_type">mime type offered by the data source</param>
        public void Offer(string mime_type)
        {
            Offer(Pointer, mime_type);
        }

        public static void Offer(IntPtr pointer, string mime_type)
        {
            Marshal(pointer, OfferOp);
        }

        /// <summary>
        /// Destroy the data source.
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
        /// Sets the actions that the source side client supports for this
        /// operation. This request may trigger wl_data_source.action and
        /// wl_data_offer.action events if the compositor needs to change the
        /// selected action.
        /// 
        /// The dnd_actions argument must contain only values expressed in the
        /// wl_data_device_manager.dnd_actions enum, otherwise it will result
        /// in a protocol error.
        /// 
        /// This request must be made once only, and can only be made on sources
        /// used in drag-and-drop, so it must be performed before
        /// wl_data_device.start_drag. Attempting to use the source other than
        /// for drag-and-drop will raise a protocol error.
        /// </summary>
        /// <param name="dnd_actions">actions supported by the data source</param>
        public void SetActions(uint dnd_actions)
        {
            SetActions(Pointer, dnd_actions);
        }

        public static void SetActions(IntPtr pointer, uint dnd_actions)
        {
            Marshal(pointer, SetActionsOp);
        }

        #endregion
    }

    /// <summary>
    /// There is one wl_data_device per seat which can be obtained
    /// from the global wl_data_device_manager singleton.
    /// 
    /// A wl_data_device provides access to inter-client data transfer
    /// mechanisms such as copy-and-paste and drag-and-drop.
    /// </summary>
    internal partial class WlDataDevice : WlProxy
    {
        #region Opcodes

        private const int StartDragOp = 0;
        private const int SetSelectionOp = 1;
        private const int ReleaseOp = 2;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_data_device", 3, 3, 6);
        public const string InterfaceName = "wl_data_device";

        private static readonly WlMessage StartDragMsg = new WlMessage("start_drag", "?oo?ou", new [] {WlDataSource.Interface, WlSurface.Interface, WlSurface.Interface});
        private static readonly WlMessage SetSelectionMsg = new WlMessage("set_selection", "?ou", new [] {WlDataSource.Interface});
        private static readonly WlMessage ReleaseMsg = new WlMessage("release", "", new WlInterface [0]);

        static WlDataDevice()
        {
            Interface.SetRequests(new [] {StartDragMsg, SetSelectionMsg, ReleaseMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            StartDragMsg.Dispose();
            SetSelectionMsg.Dispose();
            ReleaseMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlDataDevice(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void DataOfferHandler(IntPtr data, IntPtr iface, IntPtr @id);
        public delegate void EnterHandler(IntPtr data, IntPtr iface, uint @serial, IntPtr @surface, int @x, int @y, IntPtr @id);
        public delegate void LeaveHandler(IntPtr data, IntPtr iface);
        public delegate void MotionHandler(IntPtr data, IntPtr iface, uint @time, int @x, int @y);
        public delegate void DropHandler(IntPtr data, IntPtr iface);
        public delegate void SelectionHandler(IntPtr data, IntPtr iface, IntPtr @id);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 6);
        private bool _setListener;

        /// <summary>
        /// The data_offer event introduces a new wl_data_offer object,
        /// which will subsequently be used in either the
        /// data_device.enter event (for drag-and-drop) or the
        /// data_device.selection event (for selections).  Immediately
        /// following the data_device_data_offer event, the new data_offer
        /// object will send out data_offer.offer events to describe the
        /// mime types it offers.
        /// </summary>
        /// <param name="id">the new data_offer object</param>
        public DataOfferHandler DataOffer;
        /// <summary>
        /// This event is sent when an active drag-and-drop pointer enters
        /// a surface owned by the client.  The position of the pointer at
        /// enter time is provided by the x and y arguments, in surface-local
        /// coordinates.
        /// </summary>
        /// <param name="serial">serial number of the enter event</param>
        /// <param name="surface">client surface entered</param>
        /// <param name="x">surface-local x coordinate</param>
        /// <param name="y">surface-local y coordinate</param>
        /// <param name="id">source data_offer object</param>
        public EnterHandler Enter;
        /// <summary>
        /// This event is sent when the drag-and-drop pointer leaves the
        /// surface and the session ends.  The client must destroy the
        /// wl_data_offer introduced at enter time at this point.
        /// </summary>
        public LeaveHandler Leave;
        /// <summary>
        /// This event is sent when the drag-and-drop pointer moves within
        /// the currently focused surface. The new position of the pointer
        /// is provided by the x and y arguments, in surface-local
        /// coordinates.
        /// </summary>
        /// <param name="time">timestamp with millisecond granularity</param>
        /// <param name="x">surface-local x coordinate</param>
        /// <param name="y">surface-local y coordinate</param>
        public MotionHandler Motion;
        /// <summary>
        /// The event is sent when a drag-and-drop operation is ended
        /// because the implicit grab is removed.
        /// 
        /// The drag-and-drop destination is expected to honor the last action
        /// received through wl_data_offer.action, if the resulting action is
        /// "copy" or "move", the destination can still perform
        /// wl_data_offer.receive requests, and is expected to end all
        /// transfers with a wl_data_offer.finish request.
        /// 
        /// If the resulting action is "ask", the action will not be considered
        /// final. The drag-and-drop destination is expected to perform one last
        /// wl_data_offer.set_actions request, or wl_data_offer.destroy in order
        /// to cancel the operation.
        /// </summary>
        public DropHandler Drop;
        /// <summary>
        /// The selection event is sent out to notify the client of a new
        /// wl_data_offer for the selection for this device.  The
        /// data_device.data_offer and the data_offer.offer events are
        /// sent out immediately before this event to introduce the data
        /// offer object.  The selection event is sent to a client
        /// immediately before receiving keyboard focus and when a new
        /// selection is set while the client has keyboard focus.  The
        /// data_offer is valid until a new data_offer or NULL is received
        /// or until the client loses keyboard focus.  The client must
        /// destroy the previous selection data_offer, if any, upon receiving
        /// this event.
        /// </summary>
        /// <param name="id">selection data_offer object</param>
        public SelectionHandler Selection;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(DataOffer));
            SMarshal.WriteIntPtr(_listener, 1 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Enter));
            SMarshal.WriteIntPtr(_listener, 2 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Leave));
            SMarshal.WriteIntPtr(_listener, 3 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Motion));
            SMarshal.WriteIntPtr(_listener, 4 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Drop));
            SMarshal.WriteIntPtr(_listener, 5 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Selection));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        /// <summary>
        /// This request asks the compositor to start a drag-and-drop
        /// operation on behalf of the client.
        /// 
        /// The source argument is the data source that provides the data
        /// for the eventual data transfer. If source is NULL, enter, leave
        /// and motion events are sent only to the client that initiated the
        /// drag and the client is expected to handle the data passing
        /// internally.
        /// 
        /// The origin surface is the surface where the drag originates and
        /// the client must have an active implicit grab that matches the
        /// serial.
        /// 
        /// The icon surface is an optional (can be NULL) surface that
        /// provides an icon to be moved around with the cursor.  Initially,
        /// the top-left corner of the icon surface is placed at the cursor
        /// hotspot, but subsequent wl_surface.attach request can move the
        /// relative position. Attach requests must be confirmed with
        /// wl_surface.commit as usual. The icon surface is given the role of
        /// a drag-and-drop icon. If the icon surface already has another role,
        /// it raises a protocol error.
        /// 
        /// The current and pending input regions of the icon wl_surface are
        /// cleared, and wl_surface.set_input_region is ignored until the
        /// wl_surface is no longer used as the icon surface. When the use
        /// as an icon ends, the current and pending input regions become
        /// undefined, and the wl_surface is unmapped.
        /// </summary>
        /// <param name="source">data source for the eventual transfer</param>
        /// <param name="origin">surface where the drag originates</param>
        /// <param name="icon">drag-and-drop icon surface</param>
        /// <param name="serial">serial number of the implicit grab on the origin</param>
        public void StartDrag(WlDataSource source, WlSurface origin, WlSurface icon, uint serial)
        {
            StartDrag(Pointer, source, origin, icon, serial);
        }

        public static void StartDrag(IntPtr pointer, WlDataSource source, WlSurface origin, WlSurface icon, uint serial)
        {
            var args = new ArgumentList(source, origin, icon, serial);
            MarshalArray(pointer, StartDragOp, args.Pointer);
            args.Dispose();
        }

        /// <summary>
        /// This request asks the compositor to set the selection
        /// to the data from the source on behalf of the client.
        /// 
        /// To unset the selection, set the source to NULL.
        /// </summary>
        /// <param name="source">data source for the selection</param>
        /// <param name="serial">serial number of the event that triggered this request</param>
        public void SetSelection(WlDataSource source, uint serial)
        {
            SetSelection(Pointer, source, serial);
        }

        public static void SetSelection(IntPtr pointer, WlDataSource source, uint serial)
        {
            var args = new ArgumentList(source, serial);
            MarshalArray(pointer, SetSelectionOp, args.Pointer);
            args.Dispose();
        }

        /// <summary>
        /// This request destroys the data device.
        /// </summary>
        public void Release()
        {
            Release(Pointer);
        }

        public static void Release(IntPtr pointer)
        {
            Marshal(pointer, ReleaseOp);
        }

        #endregion
    }

    /// <summary>
    /// The wl_data_device_manager is a singleton global object that
    /// provides access to inter-client data transfer mechanisms such as
    /// copy-and-paste and drag-and-drop.  These mechanisms are tied to
    /// a wl_seat and this interface lets a client get a wl_data_device
    /// corresponding to a wl_seat.
    /// 
    /// Depending on the version bound, the objects created from the bound
    /// wl_data_device_manager object will have different requirements for
    /// functioning properly. See wl_data_source.set_actions,
    /// wl_data_offer.accept and wl_data_offer.finish for details.
    /// </summary>
    internal partial class WlDataDeviceManager : WlProxy
    {
        #region Opcodes

        private const int CreateDataSourceOp = 0;
        private const int GetDataDeviceOp = 1;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_data_device_manager", 3, 2, 0);
        public const string InterfaceName = "wl_data_device_manager";

        private static readonly WlMessage CreateDataSourceMsg = new WlMessage("create_data_source", "n", new [] {WlDataSource.Interface});
        private static readonly WlMessage GetDataDeviceMsg = new WlMessage("get_data_device", "no", new [] {WlDataDevice.Interface, WlSeat.Interface});

        static WlDataDeviceManager()
        {
            Interface.SetRequests(new [] {CreateDataSourceMsg, GetDataDeviceMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            CreateDataSourceMsg.Dispose();
            GetDataDeviceMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlDataDeviceManager(IntPtr pointer)
            : base(pointer) { }

        #region Events


        #endregion

        #region Requests

        /// <summary>
        /// Create a new data source.
        /// </summary>
        /// <param name="id">data source to create</param>
        public WlDataSource CreateDataSource()
        {
            return CreateDataSource(Pointer);
        }

        public static WlDataSource CreateDataSource(IntPtr pointer)
        {
            var ptr = MarshalConstructor(pointer, CreateDataSourceOp, WlDataSource.Interface.Pointer, IntPtr.Zero);
            return new WlDataSource(ptr);
        }

        /// <summary>
        /// Create a new data device for a given seat.
        /// </summary>
        /// <param name="id">data device to create</param>
        /// <param name="seat">seat associated with the data device</param>
        public WlDataDevice GetDataDevice(WlSeat seat)
        {
            return GetDataDevice(Pointer, seat);
        }

        public static WlDataDevice GetDataDevice(IntPtr pointer, WlSeat seat)
        {
            var args = new ArgumentList(seat);
            var ptr = MarshalArrayConstructor(pointer, GetDataDeviceOp, args.Pointer, WlDataDevice.Interface.Pointer);
            args.Dispose();
            return new WlDataDevice(ptr);
        }

        #endregion
    }

    /// <summary>
    /// This interface is implemented by servers that provide
    /// desktop-style user interfaces.
    /// 
    /// It allows clients to associate a wl_shell_surface with
    /// a basic surface.
    /// </summary>
    internal partial class WlShell : WlProxy
    {
        #region Opcodes

        private const int GetShellSurfaceOp = 0;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_shell", 1, 1, 0);
        public const string InterfaceName = "wl_shell";

        private static readonly WlMessage GetShellSurfaceMsg = new WlMessage("get_shell_surface", "no", new [] {WlShellSurface.Interface, WlSurface.Interface});

        static WlShell()
        {
            Interface.SetRequests(new [] {GetShellSurfaceMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            GetShellSurfaceMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlShell(IntPtr pointer)
            : base(pointer) { }

        #region Events


        #endregion

        #region Requests

        /// <summary>
        /// Create a shell surface for an existing surface. This gives
        /// the wl_surface the role of a shell surface. If the wl_surface
        /// already has another role, it raises a protocol error.
        /// 
        /// Only one shell surface can be associated with a given surface.
        /// </summary>
        /// <param name="id">shell surface to create</param>
        /// <param name="surface">surface to be given the shell surface role</param>
        public WlShellSurface GetShellSurface(WlSurface surface)
        {
            return GetShellSurface(Pointer, surface);
        }

        public static WlShellSurface GetShellSurface(IntPtr pointer, WlSurface surface)
        {
            var args = new ArgumentList(surface);
            var ptr = MarshalArrayConstructor(pointer, GetShellSurfaceOp, args.Pointer, WlShellSurface.Interface.Pointer);
            args.Dispose();
            return new WlShellSurface(ptr);
        }

        #endregion
    }

    /// <summary>
    /// An interface that may be implemented by a wl_surface, for
    /// implementations that provide a desktop-style user interface.
    /// 
    /// It provides requests to treat surfaces like toplevel, fullscreen
    /// or popup windows, move, resize or maximize them, associate
    /// metadata like title and class, etc.
    /// 
    /// On the server side the object is automatically destroyed when
    /// the related wl_surface is destroyed. On the client side,
    /// wl_shell_surface_destroy() must be called before destroying
    /// the wl_surface object.
    /// </summary>
    internal partial class WlShellSurface : WlProxy
    {
        #region Opcodes

        private const int PongOp = 0;
        private const int MoveOp = 1;
        private const int ResizeOp = 2;
        private const int SetToplevelOp = 3;
        private const int SetTransientOp = 4;
        private const int SetFullscreenOp = 5;
        private const int SetPopupOp = 6;
        private const int SetMaximizedOp = 7;
        private const int SetTitleOp = 8;
        private const int SetClassOp = 9;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_shell_surface", 1, 10, 3);
        public const string InterfaceName = "wl_shell_surface";

        private static readonly WlMessage PongMsg = new WlMessage("pong", "u", new WlInterface [0]);
        private static readonly WlMessage MoveMsg = new WlMessage("move", "ou", new [] {WlSeat.Interface});
        private static readonly WlMessage ResizeMsg = new WlMessage("resize", "ouu", new [] {WlSeat.Interface});
        private static readonly WlMessage SetToplevelMsg = new WlMessage("set_toplevel", "", new WlInterface [0]);
        private static readonly WlMessage SetTransientMsg = new WlMessage("set_transient", "oiiu", new [] {WlSurface.Interface});
        private static readonly WlMessage SetFullscreenMsg = new WlMessage("set_fullscreen", "uu?o", new [] {WlOutput.Interface});
        private static readonly WlMessage SetPopupMsg = new WlMessage("set_popup", "ouoiiu", new [] {WlSeat.Interface, WlSurface.Interface});
        private static readonly WlMessage SetMaximizedMsg = new WlMessage("set_maximized", "?o", new [] {WlOutput.Interface});
        private static readonly WlMessage SetTitleMsg = new WlMessage("set_title", "s", new WlInterface [0]);
        private static readonly WlMessage SetClassMsg = new WlMessage("set_class", "s", new WlInterface [0]);

        static WlShellSurface()
        {
            Interface.SetRequests(new [] {PongMsg, MoveMsg, ResizeMsg, SetToplevelMsg, SetTransientMsg, SetFullscreenMsg, SetPopupMsg, SetMaximizedMsg, SetTitleMsg, SetClassMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            PongMsg.Dispose();
            MoveMsg.Dispose();
            ResizeMsg.Dispose();
            SetToplevelMsg.Dispose();
            SetTransientMsg.Dispose();
            SetFullscreenMsg.Dispose();
            SetPopupMsg.Dispose();
            SetMaximizedMsg.Dispose();
            SetTitleMsg.Dispose();
            SetClassMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlShellSurface(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void PingHandler(IntPtr data, IntPtr iface, uint @serial);
        public delegate void ConfigureHandler(IntPtr data, IntPtr iface, uint @edges, int @width, int @height);
        public delegate void PopupDoneHandler(IntPtr data, IntPtr iface);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 3);
        private bool _setListener;

        /// <summary>
        /// Ping a client to check if it is receiving events and sending
        /// requests. A client is expected to reply with a pong request.
        /// </summary>
        /// <param name="serial">serial number of the ping</param>
        public PingHandler Ping;
        /// <summary>
        /// The configure event asks the client to resize its surface.
        /// 
        /// The size is a hint, in the sense that the client is free to
        /// ignore it if it doesn't resize, pick a smaller size (to
        /// satisfy aspect ratio or resize in steps of NxM pixels).
        /// 
        /// The edges parameter provides a hint about how the surface
        /// was resized. The client may use this information to decide
        /// how to adjust its content to the new size (e.g. a scrolling
        /// area might adjust its content position to leave the viewable
        /// content unmoved).
        /// 
        /// The client is free to dismiss all but the last configure
        /// event it received.
        /// 
        /// The width and height arguments specify the size of the window
        /// in surface-local coordinates.
        /// </summary>
        /// <param name="edges">how the surface was resized</param>
        /// <param name="width">new width of the surface</param>
        /// <param name="height">new height of the surface</param>
        public ConfigureHandler Configure;
        /// <summary>
        /// The popup_done event is sent out when a popup grab is broken,
        /// that is, when the user clicks a surface that doesn't belong
        /// to the client owning the popup surface.
        /// </summary>
        public PopupDoneHandler PopupDone;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Ping));
            SMarshal.WriteIntPtr(_listener, 1 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Configure));
            SMarshal.WriteIntPtr(_listener, 2 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(PopupDone));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        /// <summary>
        /// A client must respond to a ping event with a pong request or
        /// the client may be deemed unresponsive.
        /// </summary>
        /// <param name="serial">serial number of the ping event</param>
        public void Pong(uint serial)
        {
            Pong(Pointer, serial);
        }

        public static void Pong(IntPtr pointer, uint serial)
        {
            Marshal(pointer, PongOp);
        }

        /// <summary>
        /// Start a pointer-driven move of the surface.
        /// 
        /// This request must be used in response to a button press event.
        /// The server may ignore move requests depending on the state of
        /// the surface (e.g. fullscreen or maximized).
        /// </summary>
        /// <param name="seat">seat whose pointer is used</param>
        /// <param name="serial">serial number of the implicit grab on the pointer</param>
        public void Move(WlSeat seat, uint serial)
        {
            Move(Pointer, seat, serial);
        }

        public static void Move(IntPtr pointer, WlSeat seat, uint serial)
        {
            var args = new ArgumentList(seat, serial);
            MarshalArray(pointer, MoveOp, args.Pointer);
            args.Dispose();
        }

        /// <summary>
        /// Start a pointer-driven resizing of the surface.
        /// 
        /// This request must be used in response to a button press event.
        /// The server may ignore resize requests depending on the state of
        /// the surface (e.g. fullscreen or maximized).
        /// </summary>
        /// <param name="seat">seat whose pointer is used</param>
        /// <param name="serial">serial number of the implicit grab on the pointer</param>
        /// <param name="edges">which edge or corner is being dragged</param>
        public void Resize(WlSeat seat, uint serial, uint edges)
        {
            Resize(Pointer, seat, serial, edges);
        }

        public static void Resize(IntPtr pointer, WlSeat seat, uint serial, uint edges)
        {
            var args = new ArgumentList(seat, serial, edges);
            MarshalArray(pointer, ResizeOp, args.Pointer);
            args.Dispose();
        }

        /// <summary>
        /// Map the surface as a toplevel surface.
        /// 
        /// A toplevel surface is not fullscreen, maximized or transient.
        /// </summary>
        public void SetToplevel()
        {
            SetToplevel(Pointer);
        }

        public static void SetToplevel(IntPtr pointer)
        {
            Marshal(pointer, SetToplevelOp);
        }

        /// <summary>
        /// Map the surface relative to an existing surface.
        /// 
        /// The x and y arguments specify the location of the upper left
        /// corner of the surface relative to the upper left corner of the
        /// parent surface, in surface-local coordinates.
        /// 
        /// The flags argument controls details of the transient behaviour.
        /// </summary>
        /// <param name="parent">parent surface</param>
        /// <param name="x">surface-local x coordinate</param>
        /// <param name="y">surface-local y coordinate</param>
        /// <param name="flags">transient surface behavior</param>
        public void SetTransient(WlSurface parent, int x, int y, uint flags)
        {
            SetTransient(Pointer, parent, x, y, flags);
        }

        public static void SetTransient(IntPtr pointer, WlSurface parent, int x, int y, uint flags)
        {
            var args = new ArgumentList(parent, x, y, flags);
            MarshalArray(pointer, SetTransientOp, args.Pointer);
            args.Dispose();
        }

        /// <summary>
        /// Map the surface as a fullscreen surface.
        /// 
        /// If an output parameter is given then the surface will be made
        /// fullscreen on that output. If the client does not specify the
        /// output then the compositor will apply its policy - usually
        /// choosing the output on which the surface has the biggest surface
        /// area.
        /// 
        /// The client may specify a method to resolve a size conflict
        /// between the output size and the surface size - this is provided
        /// through the method parameter.
        /// 
        /// The framerate parameter is used only when the method is set
        /// to "driver", to indicate the preferred framerate. A value of 0
        /// indicates that the client does not care about framerate.  The
        /// framerate is specified in mHz, that is framerate of 60000 is 60Hz.
        /// 
        /// A method of "scale" or "driver" implies a scaling operation of
        /// the surface, either via a direct scaling operation or a change of
        /// the output mode. This will override any kind of output scaling, so
        /// that mapping a surface with a buffer size equal to the mode can
        /// fill the screen independent of buffer_scale.
        /// 
        /// A method of "fill" means we don't scale up the buffer, however
        /// any output scale is applied. This means that you may run into
        /// an edge case where the application maps a buffer with the same
        /// size of the output mode but buffer_scale 1 (thus making a
        /// surface larger than the output). In this case it is allowed to
        /// downscale the results to fit the screen.
        /// 
        /// The compositor must reply to this request with a configure event
        /// with the dimensions for the output on which the surface will
        /// be made fullscreen.
        /// </summary>
        /// <param name="method">method for resolving size conflict</param>
        /// <param name="framerate">framerate in mHz</param>
        /// <param name="output">output on which the surface is to be fullscreen</param>
        public void SetFullscreen(uint method, uint framerate, WlOutput output)
        {
            SetFullscreen(Pointer, method, framerate, output);
        }

        public static void SetFullscreen(IntPtr pointer, uint method, uint framerate, WlOutput output)
        {
            var args = new ArgumentList(method, framerate, output);
            MarshalArray(pointer, SetFullscreenOp, args.Pointer);
            args.Dispose();
        }

        /// <summary>
        /// Map the surface as a popup.
        /// 
        /// A popup surface is a transient surface with an added pointer
        /// grab.
        /// 
        /// An existing implicit grab will be changed to owner-events mode,
        /// and the popup grab will continue after the implicit grab ends
        /// (i.e. releasing the mouse button does not cause the popup to
        /// be unmapped).
        /// 
        /// The popup grab continues until the window is destroyed or a
        /// mouse button is pressed in any other client's window. A click
        /// in any of the client's surfaces is reported as normal, however,
        /// clicks in other clients' surfaces will be discarded and trigger
        /// the callback.
        /// 
        /// The x and y arguments specify the location of the upper left
        /// corner of the surface relative to the upper left corner of the
        /// parent surface, in surface-local coordinates.
        /// </summary>
        /// <param name="seat">seat whose pointer is used</param>
        /// <param name="serial">serial number of the implicit grab on the pointer</param>
        /// <param name="parent">parent surface</param>
        /// <param name="x">surface-local x coordinate</param>
        /// <param name="y">surface-local y coordinate</param>
        /// <param name="flags">transient surface behavior</param>
        public void SetPopup(WlSeat seat, uint serial, WlSurface parent, int x, int y, uint flags)
        {
            SetPopup(Pointer, seat, serial, parent, x, y, flags);
        }

        public static void SetPopup(IntPtr pointer, WlSeat seat, uint serial, WlSurface parent, int x, int y, uint flags)
        {
            var args = new ArgumentList(seat, serial, parent, x, y, flags);
            MarshalArray(pointer, SetPopupOp, args.Pointer);
            args.Dispose();
        }

        /// <summary>
        /// Map the surface as a maximized surface.
        /// 
        /// If an output parameter is given then the surface will be
        /// maximized on that output. If the client does not specify the
        /// output then the compositor will apply its policy - usually
        /// choosing the output on which the surface has the biggest surface
        /// area.
        /// 
        /// The compositor will reply with a configure event telling
        /// the expected new surface size. The operation is completed
        /// on the next buffer attach to this surface.
        /// 
        /// A maximized surface typically fills the entire output it is
        /// bound to, except for desktop elements such as panels. This is
        /// the main difference between a maximized shell surface and a
        /// fullscreen shell surface.
        /// 
        /// The details depend on the compositor implementation.
        /// </summary>
        /// <param name="output">output on which the surface is to be maximized</param>
        public void SetMaximized(WlOutput output)
        {
            SetMaximized(Pointer, output);
        }

        public static void SetMaximized(IntPtr pointer, WlOutput output)
        {
            Marshal(pointer, SetMaximizedOp);
        }

        /// <summary>
        /// Set a short title for the surface.
        /// 
        /// This string may be used to identify the surface in a task bar,
        /// window list, or other user interface elements provided by the
        /// compositor.
        /// 
        /// The string must be encoded in UTF-8.
        /// </summary>
        /// <param name="title">surface title</param>
        public void SetTitle(string title)
        {
            SetTitle(Pointer, title);
        }

        public static void SetTitle(IntPtr pointer, string title)
        {
            Marshal(pointer, SetTitleOp);
        }

        /// <summary>
        /// Set a class for the surface.
        /// 
        /// The surface class identifies the general class of applications
        /// to which the surface belongs. A common convention is to use the
        /// file name (or the full path if it is a non-standard location) of
        /// the application's .desktop file as the class.
        /// </summary>
        /// <param name="class_">surface class</param>
        public void SetClass(string class_)
        {
            SetClass(Pointer, class_);
        }

        public static void SetClass(IntPtr pointer, string class_)
        {
            Marshal(pointer, SetClassOp);
        }

        #endregion
    }

    /// <summary>
    /// A surface is a rectangular area that is displayed on the screen.
    /// It has a location, size and pixel contents.
    /// 
    /// The size of a surface (and relative positions on it) is described
    /// in surface-local coordinates, which may differ from the buffer
    /// coordinates of the pixel content, in case a buffer_transform
    /// or a buffer_scale is used.
    /// 
    /// A surface without a "role" is fairly useless: a compositor does
    /// not know where, when or how to present it. The role is the
    /// purpose of a wl_surface. Examples of roles are a cursor for a
    /// pointer (as set by wl_pointer.set_cursor), a drag icon
    /// (wl_data_device.start_drag), a sub-surface
    /// (wl_subcompositor.get_subsurface), and a window as defined by a
    /// shell protocol (e.g. wl_shell.get_shell_surface).
    /// 
    /// A surface can have only one role at a time. Initially a
    /// wl_surface does not have a role. Once a wl_surface is given a
    /// role, it is set permanently for the whole lifetime of the
    /// wl_surface object. Giving the current role again is allowed,
    /// unless explicitly forbidden by the relevant interface
    /// specification.
    /// 
    /// Surface roles are given by requests in other interfaces such as
    /// wl_pointer.set_cursor. The request should explicitly mention
    /// that this request gives a role to a wl_surface. Often, this
    /// request also creates a new protocol object that represents the
    /// role and adds additional functionality to wl_surface. When a
    /// client wants to destroy a wl_surface, they must destroy this 'role
    /// object' before the wl_surface.
    /// 
    /// Destroying the role object does not remove the role from the
    /// wl_surface, but it may stop the wl_surface from "playing the role".
    /// For instance, if a wl_subsurface object is destroyed, the wl_surface
    /// it was created for will be unmapped and forget its position and
    /// z-order. It is allowed to create a wl_subsurface for the same
    /// wl_surface again, but it is not allowed to use the wl_surface as
    /// a cursor (cursor is a different role than sub-surface, and role
    /// switching is not allowed).
    /// </summary>
    internal partial class WlSurface : WlProxy
    {
        #region Opcodes

        private const int DestroyOp = 0;
        private const int AttachOp = 1;
        private const int DamageOp = 2;
        private const int FrameOp = 3;
        private const int SetOpaqueRegionOp = 4;
        private const int SetInputRegionOp = 5;
        private const int CommitOp = 6;
        private const int SetBufferTransformOp = 7;
        private const int SetBufferScaleOp = 8;
        private const int DamageBufferOp = 9;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_surface", 4, 10, 2);
        public const string InterfaceName = "wl_surface";

        private static readonly WlMessage DestroyMsg = new WlMessage("destroy", "", new WlInterface [0]);
        private static readonly WlMessage AttachMsg = new WlMessage("attach", "?oii", new [] {WlBuffer.Interface});
        private static readonly WlMessage DamageMsg = new WlMessage("damage", "iiii", new WlInterface [0]);
        private static readonly WlMessage FrameMsg = new WlMessage("frame", "n", new [] {WlCallback.Interface});
        private static readonly WlMessage SetOpaqueRegionMsg = new WlMessage("set_opaque_region", "?o", new [] {WlRegion.Interface});
        private static readonly WlMessage SetInputRegionMsg = new WlMessage("set_input_region", "?o", new [] {WlRegion.Interface});
        private static readonly WlMessage CommitMsg = new WlMessage("commit", "", new WlInterface [0]);
        private static readonly WlMessage SetBufferTransformMsg = new WlMessage("set_buffer_transform", "i", new WlInterface [0]);
        private static readonly WlMessage SetBufferScaleMsg = new WlMessage("set_buffer_scale", "i", new WlInterface [0]);
        private static readonly WlMessage DamageBufferMsg = new WlMessage("damage_buffer", "iiii", new WlInterface [0]);

        static WlSurface()
        {
            Interface.SetRequests(new [] {DestroyMsg, AttachMsg, DamageMsg, FrameMsg, SetOpaqueRegionMsg, SetInputRegionMsg, CommitMsg, SetBufferTransformMsg, SetBufferScaleMsg, DamageBufferMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            DestroyMsg.Dispose();
            AttachMsg.Dispose();
            DamageMsg.Dispose();
            FrameMsg.Dispose();
            SetOpaqueRegionMsg.Dispose();
            SetInputRegionMsg.Dispose();
            CommitMsg.Dispose();
            SetBufferTransformMsg.Dispose();
            SetBufferScaleMsg.Dispose();
            DamageBufferMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlSurface(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void EnterHandler(IntPtr data, IntPtr iface, IntPtr @output);
        public delegate void LeaveHandler(IntPtr data, IntPtr iface, IntPtr @output);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 2);
        private bool _setListener;

        /// <summary>
        /// This is emitted whenever a surface's creation, movement, or resizing
        /// results in some part of it being within the scanout region of an
        /// output.
        /// 
        /// Note that a surface may be overlapping with zero or more outputs.
        /// </summary>
        /// <param name="output">output entered by the surface</param>
        public EnterHandler Enter;
        /// <summary>
        /// This is emitted whenever a surface's creation, movement, or resizing
        /// results in it no longer having any part of it within the scanout region
        /// of an output.
        /// </summary>
        /// <param name="output">output left by the surface</param>
        public LeaveHandler Leave;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Enter));
            SMarshal.WriteIntPtr(_listener, 1 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Leave));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        /// <summary>
        /// Deletes the surface and invalidates its object ID.
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
        /// Set a buffer as the content of this surface.
        /// 
        /// The new size of the surface is calculated based on the buffer
        /// size transformed by the inverse buffer_transform and the
        /// inverse buffer_scale. This means that the supplied buffer
        /// must be an integer multiple of the buffer_scale.
        /// 
        /// The x and y arguments specify the location of the new pending
        /// buffer's upper left corner, relative to the current buffer's upper
        /// left corner, in surface-local coordinates. In other words, the
        /// x and y, combined with the new surface size define in which
        /// directions the surface's size changes.
        /// 
        /// Surface contents are double-buffered state, see wl_surface.commit.
        /// 
        /// The initial surface contents are void; there is no content.
        /// wl_surface.attach assigns the given wl_buffer as the pending
        /// wl_buffer. wl_surface.commit makes the pending wl_buffer the new
        /// surface contents, and the size of the surface becomes the size
        /// calculated from the wl_buffer, as described above. After commit,
        /// there is no pending buffer until the next attach.
        /// 
        /// Committing a pending wl_buffer allows the compositor to read the
        /// pixels in the wl_buffer. The compositor may access the pixels at
        /// any time after the wl_surface.commit request. When the compositor
        /// will not access the pixels anymore, it will send the
        /// wl_buffer.release event. Only after receiving wl_buffer.release,
        /// the client may reuse the wl_buffer. A wl_buffer that has been
        /// attached and then replaced by another attach instead of committed
        /// will not receive a release event, and is not used by the
        /// compositor.
        /// 
        /// Destroying the wl_buffer after wl_buffer.release does not change
        /// the surface contents. However, if the client destroys the
        /// wl_buffer before receiving the wl_buffer.release event, the surface
        /// contents become undefined immediately.
        /// 
        /// If wl_surface.attach is sent with a NULL wl_buffer, the
        /// following wl_surface.commit will remove the surface content.
        /// </summary>
        /// <param name="buffer">buffer of surface contents</param>
        /// <param name="x">surface-local x coordinate</param>
        /// <param name="y">surface-local y coordinate</param>
        public void Attach(WlBuffer buffer, int x, int y)
        {
            Attach(Pointer, buffer, x, y);
        }

        public static void Attach(IntPtr pointer, WlBuffer buffer, int x, int y)
        {
            var args = new ArgumentList(buffer, x, y);
            MarshalArray(pointer, AttachOp, args.Pointer);
            args.Dispose();
        }

        /// <summary>
        /// This request is used to describe the regions where the pending
        /// buffer is different from the current surface contents, and where
        /// the surface therefore needs to be repainted. The compositor
        /// ignores the parts of the damage that fall outside of the surface.
        /// 
        /// Damage is double-buffered state, see wl_surface.commit.
        /// 
        /// The damage rectangle is specified in surface-local coordinates,
        /// where x and y specify the upper left corner of the damage rectangle.
        /// 
        /// The initial value for pending damage is empty: no damage.
        /// wl_surface.damage adds pending damage: the new pending damage
        /// is the union of old pending damage and the given rectangle.
        /// 
        /// wl_surface.commit assigns pending damage as the current damage,
        /// and clears pending damage. The server will clear the current
        /// damage as it repaints the surface.
        /// 
        /// Alternatively, damage can be posted with wl_surface.damage_buffer
        /// which uses buffer coordinates instead of surface coordinates,
        /// and is probably the preferred and intuitive way of doing this.
        /// </summary>
        /// <param name="x">surface-local x coordinate</param>
        /// <param name="y">surface-local y coordinate</param>
        /// <param name="width">width of damage rectangle</param>
        /// <param name="height">height of damage rectangle</param>
        public void Damage(int x, int y, int width, int height)
        {
            Damage(Pointer, x, y, width, height);
        }

        public static void Damage(IntPtr pointer, int x, int y, int width, int height)
        {
            var args = new ArgumentList(x, y, width, height);
            MarshalArray(pointer, DamageOp, args.Pointer);
            args.Dispose();
        }

        /// <summary>
        /// Request a notification when it is a good time to start drawing a new
        /// frame, by creating a frame callback. This is useful for throttling
        /// redrawing operations, and driving animations.
        /// 
        /// When a client is animating on a wl_surface, it can use the 'frame'
        /// request to get notified when it is a good time to draw and commit the
        /// next frame of animation. If the client commits an update earlier than
        /// that, it is likely that some updates will not make it to the display,
        /// and the client is wasting resources by drawing too often.
        /// 
        /// The frame request will take effect on the next wl_surface.commit.
        /// The notification will only be posted for one frame unless
        /// requested again. For a wl_surface, the notifications are posted in
        /// the order the frame requests were committed.
        /// 
        /// The server must send the notifications so that a client
        /// will not send excessive updates, while still allowing
        /// the highest possible update rate for clients that wait for the reply
        /// before drawing again. The server should give some time for the client
        /// to draw and commit after sending the frame callback events to let it
        /// hit the next output refresh.
        /// 
        /// A server should avoid signaling the frame callbacks if the
        /// surface is not visible in any way, e.g. the surface is off-screen,
        /// or completely obscured by other opaque surfaces.
        /// 
        /// The object returned by this request will be destroyed by the
        /// compositor after the callback is fired and as such the client must not
        /// attempt to use it after that point.
        /// 
        /// The callback_data passed in the callback is the current time, in
        /// milliseconds, with an undefined base.
        /// </summary>
        /// <param name="callback">callback object for the frame request</param>
        public WlCallback Frame()
        {
            return Frame(Pointer);
        }

        public static WlCallback Frame(IntPtr pointer)
        {
            var ptr = MarshalConstructor(pointer, FrameOp, WlCallback.Interface.Pointer, IntPtr.Zero);
            return new WlCallback(ptr);
        }

        /// <summary>
        /// This request sets the region of the surface that contains
        /// opaque content.
        /// 
        /// The opaque region is an optimization hint for the compositor
        /// that lets it optimize the redrawing of content behind opaque
        /// regions.  Setting an opaque region is not required for correct
        /// behaviour, but marking transparent content as opaque will result
        /// in repaint artifacts.
        /// 
        /// The opaque region is specified in surface-local coordinates.
        /// 
        /// The compositor ignores the parts of the opaque region that fall
        /// outside of the surface.
        /// 
        /// Opaque region is double-buffered state, see wl_surface.commit.
        /// 
        /// wl_surface.set_opaque_region changes the pending opaque region.
        /// wl_surface.commit copies the pending region to the current region.
        /// Otherwise, the pending and current regions are never changed.
        /// 
        /// The initial value for an opaque region is empty. Setting the pending
        /// opaque region has copy semantics, and the wl_region object can be
        /// destroyed immediately. A NULL wl_region causes the pending opaque
        /// region to be set to empty.
        /// </summary>
        /// <param name="region">opaque region of the surface</param>
        public void SetOpaqueRegion(WlRegion region)
        {
            SetOpaqueRegion(Pointer, region);
        }

        public static void SetOpaqueRegion(IntPtr pointer, WlRegion region)
        {
            Marshal(pointer, SetOpaqueRegionOp);
        }

        /// <summary>
        /// This request sets the region of the surface that can receive
        /// pointer and touch events.
        /// 
        /// Input events happening outside of this region will try the next
        /// surface in the server surface stack. The compositor ignores the
        /// parts of the input region that fall outside of the surface.
        /// 
        /// The input region is specified in surface-local coordinates.
        /// 
        /// Input region is double-buffered state, see wl_surface.commit.
        /// 
        /// wl_surface.set_input_region changes the pending input region.
        /// wl_surface.commit copies the pending region to the current region.
        /// Otherwise the pending and current regions are never changed,
        /// except cursor and icon surfaces are special cases, see
        /// wl_pointer.set_cursor and wl_data_device.start_drag.
        /// 
        /// The initial value for an input region is infinite. That means the
        /// whole surface will accept input. Setting the pending input region
        /// has copy semantics, and the wl_region object can be destroyed
        /// immediately. A NULL wl_region causes the input region to be set
        /// to infinite.
        /// </summary>
        /// <param name="region">input region of the surface</param>
        public void SetInputRegion(WlRegion region)
        {
            SetInputRegion(Pointer, region);
        }

        public static void SetInputRegion(IntPtr pointer, WlRegion region)
        {
            Marshal(pointer, SetInputRegionOp);
        }

        /// <summary>
        /// Surface state (input, opaque, and damage regions, attached buffers,
        /// etc.) is double-buffered. Protocol requests modify the pending state,
        /// as opposed to the current state in use by the compositor. A commit
        /// request atomically applies all pending state, replacing the current
        /// state. After commit, the new pending state is as documented for each
        /// related request.
        /// 
        /// On commit, a pending wl_buffer is applied first, and all other state
        /// second. This means that all coordinates in double-buffered state are
        /// relative to the new wl_buffer coming into use, except for
        /// wl_surface.attach itself. If there is no pending wl_buffer, the
        /// coordinates are relative to the current surface contents.
        /// 
        /// All requests that need a commit to become effective are documented
        /// to affect double-buffered state.
        /// 
        /// Other interfaces may add further double-buffered surface state.
        /// </summary>
        public void Commit()
        {
            Commit(Pointer);
        }

        public static void Commit(IntPtr pointer)
        {
            Marshal(pointer, CommitOp);
        }

        /// <summary>
        /// This request sets an optional transformation on how the compositor
        /// interprets the contents of the buffer attached to the surface. The
        /// accepted values for the transform parameter are the values for
        /// wl_output.transform.
        /// 
        /// Buffer transform is double-buffered state, see wl_surface.commit.
        /// 
        /// A newly created surface has its buffer transformation set to normal.
        /// 
        /// wl_surface.set_buffer_transform changes the pending buffer
        /// transformation. wl_surface.commit copies the pending buffer
        /// transformation to the current one. Otherwise, the pending and current
        /// values are never changed.
        /// 
        /// The purpose of this request is to allow clients to render content
        /// according to the output transform, thus permitting the compositor to
        /// use certain optimizations even if the display is rotated. Using
        /// hardware overlays and scanning out a client buffer for fullscreen
        /// surfaces are examples of such optimizations. Those optimizations are
        /// highly dependent on the compositor implementation, so the use of this
        /// request should be considered on a case-by-case basis.
        /// 
        /// Note that if the transform value includes 90 or 270 degree rotation,
        /// the width of the buffer will become the surface height and the height
        /// of the buffer will become the surface width.
        /// 
        /// If transform is not one of the values from the
        /// wl_output.transform enum the invalid_transform protocol error
        /// is raised.
        /// </summary>
        /// <param name="transform">transform for interpreting buffer contents</param>
        public void SetBufferTransform(int transform)
        {
            SetBufferTransform(Pointer, transform);
        }

        public static void SetBufferTransform(IntPtr pointer, int transform)
        {
            Marshal(pointer, SetBufferTransformOp);
        }

        /// <summary>
        /// This request sets an optional scaling factor on how the compositor
        /// interprets the contents of the buffer attached to the window.
        /// 
        /// Buffer scale is double-buffered state, see wl_surface.commit.
        /// 
        /// A newly created surface has its buffer scale set to 1.
        /// 
        /// wl_surface.set_buffer_scale changes the pending buffer scale.
        /// wl_surface.commit copies the pending buffer scale to the current one.
        /// Otherwise, the pending and current values are never changed.
        /// 
        /// The purpose of this request is to allow clients to supply higher
        /// resolution buffer data for use on high resolution outputs. It is
        /// intended that you pick the same buffer scale as the scale of the
        /// output that the surface is displayed on. This means the compositor
        /// can avoid scaling when rendering the surface on that output.
        /// 
        /// Note that if the scale is larger than 1, then you have to attach
        /// a buffer that is larger (by a factor of scale in each dimension)
        /// than the desired surface size.
        /// 
        /// If scale is not positive the invalid_scale protocol error is
        /// raised.
        /// </summary>
        /// <param name="scale">positive scale for interpreting buffer contents</param>
        public void SetBufferScale(int scale)
        {
            SetBufferScale(Pointer, scale);
        }

        public static void SetBufferScale(IntPtr pointer, int scale)
        {
            Marshal(pointer, SetBufferScaleOp);
        }

        /// <summary>
        /// This request is used to describe the regions where the pending
        /// buffer is different from the current surface contents, and where
        /// the surface therefore needs to be repainted. The compositor
        /// ignores the parts of the damage that fall outside of the surface.
        /// 
        /// Damage is double-buffered state, see wl_surface.commit.
        /// 
        /// The damage rectangle is specified in buffer coordinates,
        /// where x and y specify the upper left corner of the damage rectangle.
        /// 
        /// The initial value for pending damage is empty: no damage.
        /// wl_surface.damage_buffer adds pending damage: the new pending
        /// damage is the union of old pending damage and the given rectangle.
        /// 
        /// wl_surface.commit assigns pending damage as the current damage,
        /// and clears pending damage. The server will clear the current
        /// damage as it repaints the surface.
        /// 
        /// This request differs from wl_surface.damage in only one way - it
        /// takes damage in buffer coordinates instead of surface-local
        /// coordinates. While this generally is more intuitive than surface
        /// coordinates, it is especially desirable when using wp_viewport
        /// or when a drawing library (like EGL) is unaware of buffer scale
        /// and buffer transform.
        /// 
        /// Note: Because buffer transformation changes and damage requests may
        /// be interleaved in the protocol stream, it is impossible to determine
        /// the actual mapping between surface and buffer damage until
        /// wl_surface.commit time. Therefore, compositors wishing to take both
        /// kinds of damage into account will have to accumulate damage from the
        /// two requests separately and only transform from one to the other
        /// after receiving the wl_surface.commit.
        /// </summary>
        /// <param name="x">buffer-local x coordinate</param>
        /// <param name="y">buffer-local y coordinate</param>
        /// <param name="width">width of damage rectangle</param>
        /// <param name="height">height of damage rectangle</param>
        public void DamageBuffer(int x, int y, int width, int height)
        {
            DamageBuffer(Pointer, x, y, width, height);
        }

        public static void DamageBuffer(IntPtr pointer, int x, int y, int width, int height)
        {
            var args = new ArgumentList(x, y, width, height);
            MarshalArray(pointer, DamageBufferOp, args.Pointer);
            args.Dispose();
        }

        #endregion
    }

    /// <summary>
    /// A seat is a group of keyboards, pointer and touch devices. This
    /// object is published as a global during start up, or when such a
    /// device is hot plugged.  A seat typically has a pointer and
    /// maintains a keyboard focus and a pointer focus.
    /// </summary>
    internal partial class WlSeat : WlProxy
    {
        #region Opcodes

        private const int GetPointerOp = 0;
        private const int GetKeyboardOp = 1;
        private const int GetTouchOp = 2;
        private const int ReleaseOp = 3;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_seat", 6, 4, 2);
        public const string InterfaceName = "wl_seat";

        private static readonly WlMessage GetPointerMsg = new WlMessage("get_pointer", "n", new [] {WlPointer.Interface});
        private static readonly WlMessage GetKeyboardMsg = new WlMessage("get_keyboard", "n", new [] {WlKeyboard.Interface});
        private static readonly WlMessage GetTouchMsg = new WlMessage("get_touch", "n", new [] {WlTouch.Interface});
        private static readonly WlMessage ReleaseMsg = new WlMessage("release", "", new WlInterface [0]);

        static WlSeat()
        {
            Interface.SetRequests(new [] {GetPointerMsg, GetKeyboardMsg, GetTouchMsg, ReleaseMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            GetPointerMsg.Dispose();
            GetKeyboardMsg.Dispose();
            GetTouchMsg.Dispose();
            ReleaseMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlSeat(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void CapabilitiesHandler(IntPtr data, IntPtr iface, uint @capabilities);
        public delegate void NameHandler(IntPtr data, IntPtr iface, string @name);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 2);
        private bool _setListener;

        /// <summary>
        /// This is emitted whenever a seat gains or loses the pointer,
        /// keyboard or touch capabilities.  The argument is a capability
        /// enum containing the complete set of capabilities this seat has.
        /// 
        /// When the pointer capability is added, a client may create a
        /// wl_pointer object using the wl_seat.get_pointer request. This object
        /// will receive pointer events until the capability is removed in the
        /// future.
        /// 
        /// When the pointer capability is removed, a client should destroy the
        /// wl_pointer objects associated with the seat where the capability was
        /// removed, using the wl_pointer.release request. No further pointer
        /// events will be received on these objects.
        /// 
        /// In some compositors, if a seat regains the pointer capability and a
        /// client has a previously obtained wl_pointer object of version 4 or
        /// less, that object may start sending pointer events again. This
        /// behavior is considered a misinterpretation of the intended behavior
        /// and must not be relied upon by the client. wl_pointer objects of
        /// version 5 or later must not send events if created before the most
        /// recent event notifying the client of an added pointer capability.
        /// 
        /// The above behavior also applies to wl_keyboard and wl_touch with the
        /// keyboard and touch capabilities, respectively.
        /// </summary>
        /// <param name="capabilities">capabilities of the seat</param>
        public CapabilitiesHandler Capabilities;
        /// <summary>
        /// In a multiseat configuration this can be used by the client to help
        /// identify which physical devices the seat represents. Based on
        /// the seat configuration used by the compositor.
        /// </summary>
        /// <param name="name">seat identifier</param>
        public NameHandler Name;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Capabilities));
            SMarshal.WriteIntPtr(_listener, 1 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Name));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        /// <summary>
        /// The ID provided will be initialized to the wl_pointer interface
        /// for this seat.
        /// 
        /// This request only takes effect if the seat has the pointer
        /// capability, or has had the pointer capability in the past.
        /// It is a protocol violation to issue this request on a seat that has
        /// never had the pointer capability.
        /// </summary>
        /// <param name="id">seat pointer</param>
        public WlPointer GetPointer()
        {
            return GetPointer(Pointer);
        }

        public static WlPointer GetPointer(IntPtr pointer)
        {
            var ptr = MarshalConstructor(pointer, GetPointerOp, WlPointer.Interface.Pointer, IntPtr.Zero);
            return new WlPointer(ptr);
        }

        /// <summary>
        /// The ID provided will be initialized to the wl_keyboard interface
        /// for this seat.
        /// 
        /// This request only takes effect if the seat has the keyboard
        /// capability, or has had the keyboard capability in the past.
        /// It is a protocol violation to issue this request on a seat that has
        /// never had the keyboard capability.
        /// </summary>
        /// <param name="id">seat keyboard</param>
        public WlKeyboard GetKeyboard()
        {
            return GetKeyboard(Pointer);
        }

        public static WlKeyboard GetKeyboard(IntPtr pointer)
        {
            var ptr = MarshalConstructor(pointer, GetKeyboardOp, WlKeyboard.Interface.Pointer, IntPtr.Zero);
            return new WlKeyboard(ptr);
        }

        /// <summary>
        /// The ID provided will be initialized to the wl_touch interface
        /// for this seat.
        /// 
        /// This request only takes effect if the seat has the touch
        /// capability, or has had the touch capability in the past.
        /// It is a protocol violation to issue this request on a seat that has
        /// never had the touch capability.
        /// </summary>
        /// <param name="id">seat touch interface</param>
        public WlTouch GetTouch()
        {
            return GetTouch(Pointer);
        }

        public static WlTouch GetTouch(IntPtr pointer)
        {
            var ptr = MarshalConstructor(pointer, GetTouchOp, WlTouch.Interface.Pointer, IntPtr.Zero);
            return new WlTouch(ptr);
        }

        /// <summary>
        /// Using this request a client can tell the server that it is not going to
        /// use the seat object anymore.
        /// </summary>
        public void Release()
        {
            Release(Pointer);
        }

        public static void Release(IntPtr pointer)
        {
            Marshal(pointer, ReleaseOp);
        }

        #endregion
    }

    /// <summary>
    /// The wl_pointer interface represents one or more input devices,
    /// such as mice, which control the pointer location and pointer_focus
    /// of a seat.
    /// 
    /// The wl_pointer interface generates motion, enter and leave
    /// events for the surfaces that the pointer is located over,
    /// and button and axis events for button presses, button releases
    /// and scrolling.
    /// </summary>
    internal partial class WlPointer : WlProxy
    {
        #region Opcodes

        private const int SetCursorOp = 0;
        private const int ReleaseOp = 1;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_pointer", 6, 2, 9);
        public const string InterfaceName = "wl_pointer";

        private static readonly WlMessage SetCursorMsg = new WlMessage("set_cursor", "u?oii", new [] {WlSurface.Interface});
        private static readonly WlMessage ReleaseMsg = new WlMessage("release", "", new WlInterface [0]);

        static WlPointer()
        {
            Interface.SetRequests(new [] {SetCursorMsg, ReleaseMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            SetCursorMsg.Dispose();
            ReleaseMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlPointer(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void EnterHandler(IntPtr data, IntPtr iface, uint @serial, IntPtr @surface, int @surface_x, int @surface_y);
        public delegate void LeaveHandler(IntPtr data, IntPtr iface, uint @serial, IntPtr @surface);
        public delegate void MotionHandler(IntPtr data, IntPtr iface, uint @time, int @surface_x, int @surface_y);
        public delegate void ButtonHandler(IntPtr data, IntPtr iface, uint @serial, uint @time, uint @button, uint @state);
        public delegate void AxisHandler(IntPtr data, IntPtr iface, uint @time, uint @axis, int @value);
        public delegate void FrameHandler(IntPtr data, IntPtr iface);
        public delegate void AxisSourceHandler(IntPtr data, IntPtr iface, uint @axis_source);
        public delegate void AxisStopHandler(IntPtr data, IntPtr iface, uint @time, uint @axis);
        public delegate void AxisDiscreteHandler(IntPtr data, IntPtr iface, uint @axis, int @discrete);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 9);
        private bool _setListener;

        /// <summary>
        /// Notification that this seat's pointer is focused on a certain
        /// surface.
        /// 
        /// When a seat's focus enters a surface, the pointer image
        /// is undefined and a client should respond to this event by setting
        /// an appropriate pointer image with the set_cursor request.
        /// </summary>
        /// <param name="serial">serial number of the enter event</param>
        /// <param name="surface">surface entered by the pointer</param>
        /// <param name="surface_x">surface-local x coordinate</param>
        /// <param name="surface_y">surface-local y coordinate</param>
        public EnterHandler Enter;
        /// <summary>
        /// Notification that this seat's pointer is no longer focused on
        /// a certain surface.
        /// 
        /// The leave notification is sent before the enter notification
        /// for the new focus.
        /// </summary>
        /// <param name="serial">serial number of the leave event</param>
        /// <param name="surface">surface left by the pointer</param>
        public LeaveHandler Leave;
        /// <summary>
        /// Notification of pointer location change. The arguments
        /// surface_x and surface_y are the location relative to the
        /// focused surface.
        /// </summary>
        /// <param name="time">timestamp with millisecond granularity</param>
        /// <param name="surface_x">surface-local x coordinate</param>
        /// <param name="surface_y">surface-local y coordinate</param>
        public MotionHandler Motion;
        /// <summary>
        /// Mouse button click and release notifications.
        /// 
        /// The location of the click is given by the last motion or
        /// enter event.
        /// The time argument is a timestamp with millisecond
        /// granularity, with an undefined base.
        /// 
        /// The button is a button code as defined in the Linux kernel's
        /// linux/input-event-codes.h header file, e.g. BTN_LEFT.
        /// 
        /// Any 16-bit button code value is reserved for future additions to the
        /// kernel's event code list. All other button codes above 0xFFFF are
        /// currently undefined but may be used in future versions of this
        /// protocol.
        /// </summary>
        /// <param name="serial">serial number of the button event</param>
        /// <param name="time">timestamp with millisecond granularity</param>
        /// <param name="button">button that produced the event</param>
        /// <param name="state">physical state of the button</param>
        public ButtonHandler Button;
        /// <summary>
        /// Scroll and other axis notifications.
        /// 
        /// For scroll events (vertical and horizontal scroll axes), the
        /// value parameter is the length of a vector along the specified
        /// axis in a coordinate space identical to those of motion events,
        /// representing a relative movement along the specified axis.
        /// 
        /// For devices that support movements non-parallel to axes multiple
        /// axis events will be emitted.
        /// 
        /// When applicable, for example for touch pads, the server can
        /// choose to emit scroll events where the motion vector is
        /// equivalent to a motion event vector.
        /// 
        /// When applicable, a client can transform its content relative to the
        /// scroll distance.
        /// </summary>
        /// <param name="time">timestamp with millisecond granularity</param>
        /// <param name="axis">axis type</param>
        /// <param name="value">length of vector in surface-local coordinate space</param>
        public AxisHandler Axis;
        /// <summary>
        /// Indicates the end of a set of events that logically belong together.
        /// A client is expected to accumulate the data in all events within the
        /// frame before proceeding.
        /// 
        /// All wl_pointer events before a wl_pointer.frame event belong
        /// logically together. For example, in a diagonal scroll motion the
        /// compositor will send an optional wl_pointer.axis_source event, two
        /// wl_pointer.axis events (horizontal and vertical) and finally a
        /// wl_pointer.frame event. The client may use this information to
        /// calculate a diagonal vector for scrolling.
        /// 
        /// When multiple wl_pointer.axis events occur within the same frame,
        /// the motion vector is the combined motion of all events.
        /// When a wl_pointer.axis and a wl_pointer.axis_stop event occur within
        /// the same frame, this indicates that axis movement in one axis has
        /// stopped but continues in the other axis.
        /// When multiple wl_pointer.axis_stop events occur within the same
        /// frame, this indicates that these axes stopped in the same instance.
        /// 
        /// A wl_pointer.frame event is sent for every logical event group,
        /// even if the group only contains a single wl_pointer event.
        /// Specifically, a client may get a sequence: motion, frame, button,
        /// frame, axis, frame, axis_stop, frame.
        /// 
        /// The wl_pointer.enter and wl_pointer.leave events are logical events
        /// generated by the compositor and not the hardware. These events are
        /// also grouped by a wl_pointer.frame. When a pointer moves from one
        /// surface to another, a compositor should group the
        /// wl_pointer.leave event within the same wl_pointer.frame.
        /// However, a client must not rely on wl_pointer.leave and
        /// wl_pointer.enter being in the same wl_pointer.frame.
        /// Compositor-specific policies may require the wl_pointer.leave and
        /// wl_pointer.enter event being split across multiple wl_pointer.frame
        /// groups.
        /// </summary>
        public FrameHandler Frame;
        /// <summary>
        /// Source information for scroll and other axes.
        /// 
        /// This event does not occur on its own. It is sent before a
        /// wl_pointer.frame event and carries the source information for
        /// all events within that frame.
        /// 
        /// The source specifies how this event was generated. If the source is
        /// wl_pointer.axis_source.finger, a wl_pointer.axis_stop event will be
        /// sent when the user lifts the finger off the device.
        /// 
        /// If the source is wl_pointer.axis_source.wheel,
        /// wl_pointer.axis_source.wheel_tilt or
        /// wl_pointer.axis_source.continuous, a wl_pointer.axis_stop event may
        /// or may not be sent. Whether a compositor sends an axis_stop event
        /// for these sources is hardware-specific and implementation-dependent;
        /// clients must not rely on receiving an axis_stop event for these
        /// scroll sources and should treat scroll sequences from these scroll
        /// sources as unterminated by default.
        /// 
        /// This event is optional. If the source is unknown for a particular
        /// axis event sequence, no event is sent.
        /// Only one wl_pointer.axis_source event is permitted per frame.
        /// 
        /// The order of wl_pointer.axis_discrete and wl_pointer.axis_source is
        /// not guaranteed.
        /// </summary>
        /// <param name="axis_source">source of the axis event</param>
        public AxisSourceHandler AxisSource;
        /// <summary>
        /// Stop notification for scroll and other axes.
        /// 
        /// For some wl_pointer.axis_source types, a wl_pointer.axis_stop event
        /// is sent to notify a client that the axis sequence has terminated.
        /// This enables the client to implement kinetic scrolling.
        /// See the wl_pointer.axis_source documentation for information on when
        /// this event may be generated.
        /// 
        /// Any wl_pointer.axis events with the same axis_source after this
        /// event should be considered as the start of a new axis motion.
        /// 
        /// The timestamp is to be interpreted identical to the timestamp in the
        /// wl_pointer.axis event. The timestamp value may be the same as a
        /// preceding wl_pointer.axis event.
        /// </summary>
        /// <param name="time">timestamp with millisecond granularity</param>
        /// <param name="axis">the axis stopped with this event</param>
        public AxisStopHandler AxisStop;
        /// <summary>
        /// Discrete step information for scroll and other axes.
        /// 
        /// This event carries the axis value of the wl_pointer.axis event in
        /// discrete steps (e.g. mouse wheel clicks).
        /// 
        /// This event does not occur on its own, it is coupled with a
        /// wl_pointer.axis event that represents this axis value on a
        /// continuous scale. The protocol guarantees that each axis_discrete
        /// event is always followed by exactly one axis event with the same
        /// axis number within the same wl_pointer.frame. Note that the protocol
        /// allows for other events to occur between the axis_discrete and
        /// its coupled axis event, including other axis_discrete or axis
        /// events.
        /// 
        /// This event is optional; continuous scrolling devices
        /// like two-finger scrolling on touchpads do not have discrete
        /// steps and do not generate this event.
        /// 
        /// The discrete value carries the directional information. e.g. a value
        /// of -2 is two steps towards the negative direction of this axis.
        /// 
        /// The axis number is identical to the axis number in the associated
        /// axis event.
        /// 
        /// The order of wl_pointer.axis_discrete and wl_pointer.axis_source is
        /// not guaranteed.
        /// </summary>
        /// <param name="axis">axis type</param>
        /// <param name="discrete">number of steps</param>
        public AxisDiscreteHandler AxisDiscrete;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Enter));
            SMarshal.WriteIntPtr(_listener, 1 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Leave));
            SMarshal.WriteIntPtr(_listener, 2 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Motion));
            SMarshal.WriteIntPtr(_listener, 3 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Button));
            SMarshal.WriteIntPtr(_listener, 4 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Axis));
            SMarshal.WriteIntPtr(_listener, 5 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Frame));
            SMarshal.WriteIntPtr(_listener, 6 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(AxisSource));
            SMarshal.WriteIntPtr(_listener, 7 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(AxisStop));
            SMarshal.WriteIntPtr(_listener, 8 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(AxisDiscrete));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        /// <summary>
        /// Set the pointer surface, i.e., the surface that contains the
        /// pointer image (cursor). This request gives the surface the role
        /// of a cursor. If the surface already has another role, it raises
        /// a protocol error.
        /// 
        /// The cursor actually changes only if the pointer
        /// focus for this device is one of the requesting client's surfaces
        /// or the surface parameter is the current pointer surface. If
        /// there was a previous surface set with this request it is
        /// replaced. If surface is NULL, the pointer image is hidden.
        /// 
        /// The parameters hotspot_x and hotspot_y define the position of
        /// the pointer surface relative to the pointer location. Its
        /// top-left corner is always at (x, y) - (hotspot_x, hotspot_y),
        /// where (x, y) are the coordinates of the pointer location, in
        /// surface-local coordinates.
        /// 
        /// On surface.attach requests to the pointer surface, hotspot_x
        /// and hotspot_y are decremented by the x and y parameters
        /// passed to the request. Attach must be confirmed by
        /// wl_surface.commit as usual.
        /// 
        /// The hotspot can also be updated by passing the currently set
        /// pointer surface to this request with new values for hotspot_x
        /// and hotspot_y.
        /// 
        /// The current and pending input regions of the wl_surface are
        /// cleared, and wl_surface.set_input_region is ignored until the
        /// wl_surface is no longer used as the cursor. When the use as a
        /// cursor ends, the current and pending input regions become
        /// undefined, and the wl_surface is unmapped.
        /// </summary>
        /// <param name="serial">serial number of the enter event</param>
        /// <param name="surface">pointer surface</param>
        /// <param name="hotspot_x">surface-local x coordinate</param>
        /// <param name="hotspot_y">surface-local y coordinate</param>
        public void SetCursor(uint serial, WlSurface surface, int hotspot_x, int hotspot_y)
        {
            SetCursor(Pointer, serial, surface, hotspot_x, hotspot_y);
        }

        public static void SetCursor(IntPtr pointer, uint serial, WlSurface surface, int hotspot_x, int hotspot_y)
        {
            var args = new ArgumentList(serial, surface, hotspot_x, hotspot_y);
            MarshalArray(pointer, SetCursorOp, args.Pointer);
            args.Dispose();
        }

        /// <summary>
        /// Using this request a client can tell the server that it is not going to
        /// use the pointer object anymore.
        /// 
        /// This request destroys the pointer proxy object, so clients must not call
        /// wl_pointer_destroy() after using this request.
        /// </summary>
        public void Release()
        {
            Release(Pointer);
        }

        public static void Release(IntPtr pointer)
        {
            Marshal(pointer, ReleaseOp);
        }

        #endregion
    }

    /// <summary>
    /// The wl_keyboard interface represents one or more keyboards
    /// associated with a seat.
    /// </summary>
    internal partial class WlKeyboard : WlProxy
    {
        #region Opcodes

        private const int ReleaseOp = 0;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_keyboard", 6, 1, 6);
        public const string InterfaceName = "wl_keyboard";

        private static readonly WlMessage ReleaseMsg = new WlMessage("release", "", new WlInterface [0]);

        static WlKeyboard()
        {
            Interface.SetRequests(new [] {ReleaseMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            ReleaseMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlKeyboard(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void KeymapHandler(IntPtr data, IntPtr iface, uint @format, int @fd, uint @size);
        public delegate void EnterHandler(IntPtr data, IntPtr iface, uint @serial, IntPtr @surface, WlArray @keys);
        public delegate void LeaveHandler(IntPtr data, IntPtr iface, uint @serial, IntPtr @surface);
        public delegate void KeyHandler(IntPtr data, IntPtr iface, uint @serial, uint @time, uint @key, uint @state);
        public delegate void ModifiersHandler(IntPtr data, IntPtr iface, uint @serial, uint @mods_depressed, uint @mods_latched, uint @mods_locked, uint @group);
        public delegate void RepeatInfoHandler(IntPtr data, IntPtr iface, int @rate, int @delay);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 6);
        private bool _setListener;

        /// <summary>
        /// This event provides a file descriptor to the client which can be
        /// memory-mapped to provide a keyboard mapping description.
        /// </summary>
        /// <param name="format">keymap format</param>
        /// <param name="fd">keymap file descriptor</param>
        /// <param name="size">keymap size, in bytes</param>
        public KeymapHandler Keymap;
        /// <summary>
        /// Notification that this seat's keyboard focus is on a certain
        /// surface.
        /// </summary>
        /// <param name="serial">serial number of the enter event</param>
        /// <param name="surface">surface gaining keyboard focus</param>
        /// <param name="keys">the currently pressed keys</param>
        public EnterHandler Enter;
        /// <summary>
        /// Notification that this seat's keyboard focus is no longer on
        /// a certain surface.
        /// 
        /// The leave notification is sent before the enter notification
        /// for the new focus.
        /// </summary>
        /// <param name="serial">serial number of the leave event</param>
        /// <param name="surface">surface that lost keyboard focus</param>
        public LeaveHandler Leave;
        /// <summary>
        /// A key was pressed or released.
        /// The time argument is a timestamp with millisecond
        /// granularity, with an undefined base.
        /// </summary>
        /// <param name="serial">serial number of the key event</param>
        /// <param name="time">timestamp with millisecond granularity</param>
        /// <param name="key">key that produced the event</param>
        /// <param name="state">physical state of the key</param>
        public KeyHandler Key;
        /// <summary>
        /// Notifies clients that the modifier and/or group state has
        /// changed, and it should update its local state.
        /// </summary>
        /// <param name="serial">serial number of the modifiers event</param>
        /// <param name="mods_depressed">depressed modifiers</param>
        /// <param name="mods_latched">latched modifiers</param>
        /// <param name="mods_locked">locked modifiers</param>
        /// <param name="group">keyboard layout</param>
        public ModifiersHandler Modifiers;
        /// <summary>
        /// Informs the client about the keyboard's repeat rate and delay.
        /// 
        /// This event is sent as soon as the wl_keyboard object has been created,
        /// and is guaranteed to be received by the client before any key press
        /// event.
        /// 
        /// Negative values for either rate or delay are illegal. A rate of zero
        /// will disable any repeating (regardless of the value of delay).
        /// 
        /// This event can be sent later on as well with a new value if necessary,
        /// so clients should continue listening for the event past the creation
        /// of wl_keyboard.
        /// </summary>
        /// <param name="rate">the rate of repeating keys in characters per second</param>
        /// <param name="delay">delay in milliseconds since key down until repeating starts</param>
        public RepeatInfoHandler RepeatInfo;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Keymap));
            SMarshal.WriteIntPtr(_listener, 1 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Enter));
            SMarshal.WriteIntPtr(_listener, 2 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Leave));
            SMarshal.WriteIntPtr(_listener, 3 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Key));
            SMarshal.WriteIntPtr(_listener, 4 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Modifiers));
            SMarshal.WriteIntPtr(_listener, 5 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(RepeatInfo));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        /// <summary>
        /// 
        /// </summary>
        public void Release()
        {
            Release(Pointer);
        }

        public static void Release(IntPtr pointer)
        {
            Marshal(pointer, ReleaseOp);
        }

        #endregion
    }

    /// <summary>
    /// The wl_touch interface represents a touchscreen
    /// associated with a seat.
    /// 
    /// Touch interactions can consist of one or more contacts.
    /// For each contact, a series of events is generated, starting
    /// with a down event, followed by zero or more motion events,
    /// and ending with an up event. Events relating to the same
    /// contact point can be identified by the ID of the sequence.
    /// </summary>
    internal partial class WlTouch : WlProxy
    {
        #region Opcodes

        private const int ReleaseOp = 0;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_touch", 6, 1, 7);
        public const string InterfaceName = "wl_touch";

        private static readonly WlMessage ReleaseMsg = new WlMessage("release", "", new WlInterface [0]);

        static WlTouch()
        {
            Interface.SetRequests(new [] {ReleaseMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            ReleaseMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlTouch(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void DownHandler(IntPtr data, IntPtr iface, uint @serial, uint @time, IntPtr @surface, int @id, int @x, int @y);
        public delegate void UpHandler(IntPtr data, IntPtr iface, uint @serial, uint @time, int @id);
        public delegate void MotionHandler(IntPtr data, IntPtr iface, uint @time, int @id, int @x, int @y);
        public delegate void FrameHandler(IntPtr data, IntPtr iface);
        public delegate void CancelHandler(IntPtr data, IntPtr iface);
        public delegate void ShapeHandler(IntPtr data, IntPtr iface, int @id, int @major, int @minor);
        public delegate void OrientationHandler(IntPtr data, IntPtr iface, int @id, int @orientation);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 7);
        private bool _setListener;

        /// <summary>
        /// A new touch point has appeared on the surface. This touch point is
        /// assigned a unique ID. Future events from this touch point reference
        /// this ID. The ID ceases to be valid after a touch up event and may be
        /// reused in the future.
        /// </summary>
        /// <param name="serial">serial number of the touch down event</param>
        /// <param name="time">timestamp with millisecond granularity</param>
        /// <param name="surface">surface touched</param>
        /// <param name="id">the unique ID of this touch point</param>
        /// <param name="x">surface-local x coordinate</param>
        /// <param name="y">surface-local y coordinate</param>
        public DownHandler Down;
        /// <summary>
        /// The touch point has disappeared. No further events will be sent for
        /// this touch point and the touch point's ID is released and may be
        /// reused in a future touch down event.
        /// </summary>
        /// <param name="serial">serial number of the touch up event</param>
        /// <param name="time">timestamp with millisecond granularity</param>
        /// <param name="id">the unique ID of this touch point</param>
        public UpHandler Up;
        /// <summary>
        /// A touch point has changed coordinates.
        /// </summary>
        /// <param name="time">timestamp with millisecond granularity</param>
        /// <param name="id">the unique ID of this touch point</param>
        /// <param name="x">surface-local x coordinate</param>
        /// <param name="y">surface-local y coordinate</param>
        public MotionHandler Motion;
        /// <summary>
        /// Indicates the end of a set of events that logically belong together.
        /// A client is expected to accumulate the data in all events within the
        /// frame before proceeding.
        /// 
        /// A wl_touch.frame terminates at least one event but otherwise no
        /// guarantee is provided about the set of events within a frame. A client
        /// must assume that any state not updated in a frame is unchanged from the
        /// previously known state.
        /// </summary>
        public FrameHandler Frame;
        /// <summary>
        /// Sent if the compositor decides the touch stream is a global
        /// gesture. No further events are sent to the clients from that
        /// particular gesture. Touch cancellation applies to all touch points
        /// currently active on this client's surface. The client is
        /// responsible for finalizing the touch points, future touch points on
        /// this surface may reuse the touch point ID.
        /// </summary>
        public CancelHandler Cancel;
        /// <summary>
        /// Sent when a touchpoint has changed its shape.
        /// 
        /// This event does not occur on its own. It is sent before a
        /// wl_touch.frame event and carries the new shape information for
        /// any previously reported, or new touch points of that frame.
        /// 
        /// Other events describing the touch point such as wl_touch.down,
        /// wl_touch.motion or wl_touch.orientation may be sent within the
        /// same wl_touch.frame. A client should treat these events as a single
        /// logical touch point update. The order of wl_touch.shape,
        /// wl_touch.orientation and wl_touch.motion is not guaranteed.
        /// A wl_touch.down event is guaranteed to occur before the first
        /// wl_touch.shape event for this touch ID but both events may occur within
        /// the same wl_touch.frame.
        /// 
        /// A touchpoint shape is approximated by an ellipse through the major and
        /// minor axis length. The major axis length describes the longer diameter
        /// of the ellipse, while the minor axis length describes the shorter
        /// diameter. Major and minor are orthogonal and both are specified in
        /// surface-local coordinates. The center of the ellipse is always at the
        /// touchpoint location as reported by wl_touch.down or wl_touch.move.
        /// 
        /// This event is only sent by the compositor if the touch device supports
        /// shape reports. The client has to make reasonable assumptions about the
        /// shape if it did not receive this event.
        /// </summary>
        /// <param name="id">the unique ID of this touch point</param>
        /// <param name="major">length of the major axis in surface-local coordinates</param>
        /// <param name="minor">length of the minor axis in surface-local coordinates</param>
        public ShapeHandler Shape;
        /// <summary>
        /// Sent when a touchpoint has changed its orientation.
        /// 
        /// This event does not occur on its own. It is sent before a
        /// wl_touch.frame event and carries the new shape information for
        /// any previously reported, or new touch points of that frame.
        /// 
        /// Other events describing the touch point such as wl_touch.down,
        /// wl_touch.motion or wl_touch.shape may be sent within the
        /// same wl_touch.frame. A client should treat these events as a single
        /// logical touch point update. The order of wl_touch.shape,
        /// wl_touch.orientation and wl_touch.motion is not guaranteed.
        /// A wl_touch.down event is guaranteed to occur before the first
        /// wl_touch.orientation event for this touch ID but both events may occur
        /// within the same wl_touch.frame.
        /// 
        /// The orientation describes the clockwise angle of a touchpoint's major
        /// axis to the positive surface y-axis and is normalized to the -180 to
        /// +180 degree range. The granularity of orientation depends on the touch
        /// device, some devices only support binary rotation values between 0 and
        /// 90 degrees.
        /// 
        /// This event is only sent by the compositor if the touch device supports
        /// orientation reports.
        /// </summary>
        /// <param name="id">the unique ID of this touch point</param>
        /// <param name="orientation">angle between major axis and positive surface y-axis in degrees</param>
        public OrientationHandler Orientation;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Down));
            SMarshal.WriteIntPtr(_listener, 1 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Up));
            SMarshal.WriteIntPtr(_listener, 2 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Motion));
            SMarshal.WriteIntPtr(_listener, 3 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Frame));
            SMarshal.WriteIntPtr(_listener, 4 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Cancel));
            SMarshal.WriteIntPtr(_listener, 5 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Shape));
            SMarshal.WriteIntPtr(_listener, 6 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Orientation));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        /// <summary>
        /// 
        /// </summary>
        public void Release()
        {
            Release(Pointer);
        }

        public static void Release(IntPtr pointer)
        {
            Marshal(pointer, ReleaseOp);
        }

        #endregion
    }

    /// <summary>
    /// An output describes part of the compositor geometry.  The
    /// compositor works in the 'compositor coordinate system' and an
    /// output corresponds to a rectangular area in that space that is
    /// actually visible.  This typically corresponds to a monitor that
    /// displays part of the compositor space.  This object is published
    /// as global during start up, or when a monitor is hotplugged.
    /// </summary>
    internal partial class WlOutput : WlProxy
    {
        #region Opcodes

        private const int ReleaseOp = 0;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_output", 3, 1, 4);
        public const string InterfaceName = "wl_output";

        private static readonly WlMessage ReleaseMsg = new WlMessage("release", "", new WlInterface [0]);

        static WlOutput()
        {
            Interface.SetRequests(new [] {ReleaseMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            ReleaseMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlOutput(IntPtr pointer)
            : base(pointer) { }

        #region Events

        public delegate void GeometryHandler(IntPtr data, IntPtr iface, int @x, int @y, int @physical_width, int @physical_height, int @subpixel, string @make, string @model, int @transform);
        public delegate void ModeHandler(IntPtr data, IntPtr iface, uint @flags, int @width, int @height, int @refresh);
        public delegate void DoneHandler(IntPtr data, IntPtr iface);
        public delegate void ScaleHandler(IntPtr data, IntPtr iface, int @factor);

        private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * 4);
        private bool _setListener;

        /// <summary>
        /// The geometry event describes geometric properties of the output.
        /// The event is sent when binding to the output object and whenever
        /// any of the properties change.
        /// </summary>
        /// <param name="x">x position within the global compositor space</param>
        /// <param name="y">y position within the global compositor space</param>
        /// <param name="physical_width">width in millimeters of the output</param>
        /// <param name="physical_height">height in millimeters of the output</param>
        /// <param name="subpixel">subpixel orientation of the output</param>
        /// <param name="make">textual description of the manufacturer</param>
        /// <param name="model">textual description of the model</param>
        /// <param name="transform">transform that maps framebuffer to output</param>
        public GeometryHandler Geometry;
        /// <summary>
        /// The mode event describes an available mode for the output.
        /// 
        /// The event is sent when binding to the output object and there
        /// will always be one mode, the current mode.  The event is sent
        /// again if an output changes mode, for the mode that is now
        /// current.  In other words, the current mode is always the last
        /// mode that was received with the current flag set.
        /// 
        /// The size of a mode is given in physical hardware units of
        /// the output device. This is not necessarily the same as
        /// the output size in the global compositor space. For instance,
        /// the output may be scaled, as described in wl_output.scale,
        /// or transformed, as described in wl_output.transform.
        /// </summary>
        /// <param name="flags">bitfield of mode flags</param>
        /// <param name="width">width of the mode in hardware units</param>
        /// <param name="height">height of the mode in hardware units</param>
        /// <param name="refresh">vertical refresh rate in mHz</param>
        public ModeHandler Mode;
        /// <summary>
        /// This event is sent after all other properties have been
        /// sent after binding to the output object and after any
        /// other property changes done after that. This allows
        /// changes to the output properties to be seen as
        /// atomic, even if they happen via multiple events.
        /// </summary>
        public DoneHandler Done;
        /// <summary>
        /// This event contains scaling geometry information
        /// that is not in the geometry event. It may be sent after
        /// binding the output object or if the output scale changes
        /// later. If it is not sent, the client should assume a
        /// scale of 1.
        /// 
        /// A scale larger than 1 means that the compositor will
        /// automatically scale surface buffers by this amount
        /// when rendering. This is used for very high resolution
        /// displays where applications rendering at the native
        /// resolution would be too small to be legible.
        /// 
        /// It is intended that scaling aware clients track the
        /// current output of a surface, and if it is on a scaled
        /// output it should use wl_surface.set_buffer_scale with
        /// the scale of the output. That way the compositor can
        /// avoid scaling the surface, and the client can supply
        /// a higher detail image.
        /// </summary>
        /// <param name="factor">scaling factor of output</param>
        public ScaleHandler Scale;

        public void SetListener()
        {
            if (_setListener)
                throw new Exception("Listener already set.");
            SMarshal.WriteIntPtr(_listener, 0 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Geometry));
            SMarshal.WriteIntPtr(_listener, 1 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Mode));
            SMarshal.WriteIntPtr(_listener, 2 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Done));
            SMarshal.WriteIntPtr(_listener, 3 * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate(Scale));
            AddListener(Pointer, _listener, IntPtr.Zero);
            _setListener = true;
        }

        [DllImport("libwayland-client.so", EntryPoint = "wl_proxy_add_listener")]
        private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);

        #endregion

        #region Requests

        /// <summary>
        /// Using this request a client can tell the server that it is not going to
        /// use the output object anymore.
        /// </summary>
        public void Release()
        {
            Release(Pointer);
        }

        public static void Release(IntPtr pointer)
        {
            Marshal(pointer, ReleaseOp);
        }

        #endregion
    }

    /// <summary>
    /// A region object describes an area.
    /// 
    /// Region objects are used to describe the opaque and input
    /// regions of a surface.
    /// </summary>
    internal partial class WlRegion : WlProxy
    {
        #region Opcodes

        private const int DestroyOp = 0;
        private const int AddOp = 1;
        private const int SubtractOp = 2;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_region", 1, 3, 0);
        public const string InterfaceName = "wl_region";

        private static readonly WlMessage DestroyMsg = new WlMessage("destroy", "", new WlInterface [0]);
        private static readonly WlMessage AddMsg = new WlMessage("add", "iiii", new WlInterface [0]);
        private static readonly WlMessage SubtractMsg = new WlMessage("subtract", "iiii", new WlInterface [0]);

        static WlRegion()
        {
            Interface.SetRequests(new [] {DestroyMsg, AddMsg, SubtractMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            DestroyMsg.Dispose();
            AddMsg.Dispose();
            SubtractMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlRegion(IntPtr pointer)
            : base(pointer) { }

        #region Events


        #endregion

        #region Requests

        /// <summary>
        /// Destroy the region.  This will invalidate the object ID.
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
        /// Add the specified rectangle to the region.
        /// </summary>
        /// <param name="x">region-local x coordinate</param>
        /// <param name="y">region-local y coordinate</param>
        /// <param name="width">rectangle width</param>
        /// <param name="height">rectangle height</param>
        public void Add(int x, int y, int width, int height)
        {
            Add(Pointer, x, y, width, height);
        }

        public static void Add(IntPtr pointer, int x, int y, int width, int height)
        {
            var args = new ArgumentList(x, y, width, height);
            MarshalArray(pointer, AddOp, args.Pointer);
            args.Dispose();
        }

        /// <summary>
        /// Subtract the specified rectangle from the region.
        /// </summary>
        /// <param name="x">region-local x coordinate</param>
        /// <param name="y">region-local y coordinate</param>
        /// <param name="width">rectangle width</param>
        /// <param name="height">rectangle height</param>
        public void Subtract(int x, int y, int width, int height)
        {
            Subtract(Pointer, x, y, width, height);
        }

        public static void Subtract(IntPtr pointer, int x, int y, int width, int height)
        {
            var args = new ArgumentList(x, y, width, height);
            MarshalArray(pointer, SubtractOp, args.Pointer);
            args.Dispose();
        }

        #endregion
    }

    /// <summary>
    /// The global interface exposing sub-surface compositing capabilities.
    /// A wl_surface, that has sub-surfaces associated, is called the
    /// parent surface. Sub-surfaces can be arbitrarily nested and create
    /// a tree of sub-surfaces.
    /// 
    /// The root surface in a tree of sub-surfaces is the main
    /// surface. The main surface cannot be a sub-surface, because
    /// sub-surfaces must always have a parent.
    /// 
    /// A main surface with its sub-surfaces forms a (compound) window.
    /// For window management purposes, this set of wl_surface objects is
    /// to be considered as a single window, and it should also behave as
    /// such.
    /// 
    /// The aim of sub-surfaces is to offload some of the compositing work
    /// within a window from clients to the compositor. A prime example is
    /// a video player with decorations and video in separate wl_surface
    /// objects. This should allow the compositor to pass YUV video buffer
    /// processing to dedicated overlay hardware when possible.
    /// </summary>
    internal partial class WlSubcompositor : WlProxy
    {
        #region Opcodes

        private const int DestroyOp = 0;
        private const int GetSubsurfaceOp = 1;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_subcompositor", 1, 2, 0);
        public const string InterfaceName = "wl_subcompositor";

        private static readonly WlMessage DestroyMsg = new WlMessage("destroy", "", new WlInterface [0]);
        private static readonly WlMessage GetSubsurfaceMsg = new WlMessage("get_subsurface", "noo", new [] {WlSubsurface.Interface, WlSurface.Interface, WlSurface.Interface});

        static WlSubcompositor()
        {
            Interface.SetRequests(new [] {DestroyMsg, GetSubsurfaceMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            DestroyMsg.Dispose();
            GetSubsurfaceMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlSubcompositor(IntPtr pointer)
            : base(pointer) { }

        #region Events


        #endregion

        #region Requests

        /// <summary>
        /// Informs the server that the client will not be using this
        /// protocol object anymore. This does not affect any other
        /// objects, wl_subsurface objects included.
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
        /// Create a sub-surface interface for the given surface, and
        /// associate it with the given parent surface. This turns a
        /// plain wl_surface into a sub-surface.
        /// 
        /// The to-be sub-surface must not already have another role, and it
        /// must not have an existing wl_subsurface object. Otherwise a protocol
        /// error is raised.
        /// </summary>
        /// <param name="id">the new sub-surface object ID</param>
        /// <param name="surface">the surface to be turned into a sub-surface</param>
        /// <param name="parent">the parent surface</param>
        public WlSubsurface GetSubsurface(WlSurface surface, WlSurface parent)
        {
            return GetSubsurface(Pointer, surface, parent);
        }

        public static WlSubsurface GetSubsurface(IntPtr pointer, WlSurface surface, WlSurface parent)
        {
            var args = new ArgumentList(surface, parent);
            var ptr = MarshalArrayConstructor(pointer, GetSubsurfaceOp, args.Pointer, WlSubsurface.Interface.Pointer);
            args.Dispose();
            return new WlSubsurface(ptr);
        }

        #endregion
    }

    /// <summary>
    /// An additional interface to a wl_surface object, which has been
    /// made a sub-surface. A sub-surface has one parent surface. A
    /// sub-surface's size and position are not limited to that of the parent.
    /// Particularly, a sub-surface is not automatically clipped to its
    /// parent's area.
    /// 
    /// A sub-surface becomes mapped, when a non-NULL wl_buffer is applied
    /// and the parent surface is mapped. The order of which one happens
    /// first is irrelevant. A sub-surface is hidden if the parent becomes
    /// hidden, or if a NULL wl_buffer is applied. These rules apply
    /// recursively through the tree of surfaces.
    /// 
    /// The behaviour of a wl_surface.commit request on a sub-surface
    /// depends on the sub-surface's mode. The possible modes are
    /// synchronized and desynchronized, see methods
    /// wl_subsurface.set_sync and wl_subsurface.set_desync. Synchronized
    /// mode caches the wl_surface state to be applied when the parent's
    /// state gets applied, and desynchronized mode applies the pending
    /// wl_surface state directly. A sub-surface is initially in the
    /// synchronized mode.
    /// 
    /// Sub-surfaces have also other kind of state, which is managed by
    /// wl_subsurface requests, as opposed to wl_surface requests. This
    /// state includes the sub-surface position relative to the parent
    /// surface (wl_subsurface.set_position), and the stacking order of
    /// the parent and its sub-surfaces (wl_subsurface.place_above and
    /// .place_below). This state is applied when the parent surface's
    /// wl_surface state is applied, regardless of the sub-surface's mode.
    /// As the exception, set_sync and set_desync are effective immediately.
    /// 
    /// The main surface can be thought to be always in desynchronized mode,
    /// since it does not have a parent in the sub-surfaces sense.
    /// 
    /// Even if a sub-surface is in desynchronized mode, it will behave as
    /// in synchronized mode, if its parent surface behaves as in
    /// synchronized mode. This rule is applied recursively throughout the
    /// tree of surfaces. This means, that one can set a sub-surface into
    /// synchronized mode, and then assume that all its child and grand-child
    /// sub-surfaces are synchronized, too, without explicitly setting them.
    /// 
    /// If the wl_surface associated with the wl_subsurface is destroyed, the
    /// wl_subsurface object becomes inert. Note, that destroying either object
    /// takes effect immediately. If you need to synchronize the removal
    /// of a sub-surface to the parent surface update, unmap the sub-surface
    /// first by attaching a NULL wl_buffer, update parent, and then destroy
    /// the sub-surface.
    /// 
    /// If the parent wl_surface object is destroyed, the sub-surface is
    /// unmapped.
    /// </summary>
    internal partial class WlSubsurface : WlProxy
    {
        #region Opcodes

        private const int DestroyOp = 0;
        private const int SetPositionOp = 1;
        private const int PlaceAboveOp = 2;
        private const int PlaceBelowOp = 3;
        private const int SetSyncOp = 4;
        private const int SetDesyncOp = 5;

        #endregion

        #region Interface

        public static WlInterface Interface = new WlInterface("wl_subsurface", 1, 6, 0);
        public const string InterfaceName = "wl_subsurface";

        private static readonly WlMessage DestroyMsg = new WlMessage("destroy", "", new WlInterface [0]);
        private static readonly WlMessage SetPositionMsg = new WlMessage("set_position", "ii", new WlInterface [0]);
        private static readonly WlMessage PlaceAboveMsg = new WlMessage("place_above", "o", new [] {WlSurface.Interface});
        private static readonly WlMessage PlaceBelowMsg = new WlMessage("place_below", "o", new [] {WlSurface.Interface});
        private static readonly WlMessage SetSyncMsg = new WlMessage("set_sync", "", new WlInterface [0]);
        private static readonly WlMessage SetDesyncMsg = new WlMessage("set_desync", "", new WlInterface [0]);

        static WlSubsurface()
        {
            Interface.SetRequests(new [] {DestroyMsg, SetPositionMsg, PlaceAboveMsg, PlaceBelowMsg, SetSyncMsg, SetDesyncMsg});
            Interface.Finish();
        }

        public static void CleanUp()
        {
            DestroyMsg.Dispose();
            SetPositionMsg.Dispose();
            PlaceAboveMsg.Dispose();
            PlaceBelowMsg.Dispose();
            SetSyncMsg.Dispose();
            SetDesyncMsg.Dispose();
            Interface.Dispose();
        }

        #endregion

        public WlSubsurface(IntPtr pointer)
            : base(pointer) { }

        #region Events


        #endregion

        #region Requests

        /// <summary>
        /// The sub-surface interface is removed from the wl_surface object
        /// that was turned into a sub-surface with a
        /// wl_subcompositor.get_subsurface request. The wl_surface's association
        /// to the parent is deleted, and the wl_surface loses its role as
        /// a sub-surface. The wl_surface is unmapped.
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
        /// This schedules a sub-surface position change.
        /// The sub-surface will be moved so that its origin (top left
        /// corner pixel) will be at the location x, y of the parent surface
        /// coordinate system. The coordinates are not restricted to the parent
        /// surface area. Negative values are allowed.
        /// 
        /// The scheduled coordinates will take effect whenever the state of the
        /// parent surface is applied. When this happens depends on whether the
        /// parent surface is in synchronized mode or not. See
        /// wl_subsurface.set_sync and wl_subsurface.set_desync for details.
        /// 
        /// If more than one set_position request is invoked by the client before
        /// the commit of the parent surface, the position of a new request always
        /// replaces the scheduled position from any previous request.
        /// 
        /// The initial position is 0, 0.
        /// </summary>
        /// <param name="x">x coordinate in the parent surface</param>
        /// <param name="y">y coordinate in the parent surface</param>
        public void SetPosition(int x, int y)
        {
            SetPosition(Pointer, x, y);
        }

        public static void SetPosition(IntPtr pointer, int x, int y)
        {
            var args = new ArgumentList(x, y);
            MarshalArray(pointer, SetPositionOp, args.Pointer);
            args.Dispose();
        }

        /// <summary>
        /// This sub-surface is taken from the stack, and put back just
        /// above the reference surface, changing the z-order of the sub-surfaces.
        /// The reference surface must be one of the sibling surfaces, or the
        /// parent surface. Using any other surface, including this sub-surface,
        /// will cause a protocol error.
        /// 
        /// The z-order is double-buffered. Requests are handled in order and
        /// applied immediately to a pending state. The final pending state is
        /// copied to the active state the next time the state of the parent
        /// surface is applied. When this happens depends on whether the parent
        /// surface is in synchronized mode or not. See wl_subsurface.set_sync and
        /// wl_subsurface.set_desync for details.
        /// 
        /// A new sub-surface is initially added as the top-most in the stack
        /// of its siblings and parent.
        /// </summary>
        /// <param name="sibling">the reference surface</param>
        public void PlaceAbove(WlSurface sibling)
        {
            PlaceAbove(Pointer, sibling);
        }

        public static void PlaceAbove(IntPtr pointer, WlSurface sibling)
        {
            Marshal(pointer, PlaceAboveOp);
        }

        /// <summary>
        /// The sub-surface is placed just below the reference surface.
        /// See wl_subsurface.place_above.
        /// </summary>
        /// <param name="sibling">the reference surface</param>
        public void PlaceBelow(WlSurface sibling)
        {
            PlaceBelow(Pointer, sibling);
        }

        public static void PlaceBelow(IntPtr pointer, WlSurface sibling)
        {
            Marshal(pointer, PlaceBelowOp);
        }

        /// <summary>
        /// Change the commit behaviour of the sub-surface to synchronized
        /// mode, also described as the parent dependent mode.
        /// 
        /// In synchronized mode, wl_surface.commit on a sub-surface will
        /// accumulate the committed state in a cache, but the state will
        /// not be applied and hence will not change the compositor output.
        /// The cached state is applied to the sub-surface immediately after
        /// the parent surface's state is applied. This ensures atomic
        /// updates of the parent and all its synchronized sub-surfaces.
        /// Applying the cached state will invalidate the cache, so further
        /// parent surface commits do not (re-)apply old state.
        /// 
        /// See wl_subsurface for the recursive effect of this mode.
        /// </summary>
        public void SetSync()
        {
            SetSync(Pointer);
        }

        public static void SetSync(IntPtr pointer)
        {
            Marshal(pointer, SetSyncOp);
        }

        /// <summary>
        /// Change the commit behaviour of the sub-surface to desynchronized
        /// mode, also described as independent or freely running mode.
        /// 
        /// In desynchronized mode, wl_surface.commit on a sub-surface will
        /// apply the pending state directly, without caching, as happens
        /// normally with a wl_surface. Calling wl_surface.commit on the
        /// parent surface has no effect on the sub-surface's wl_surface
        /// state. This mode allows a sub-surface to be updated on its own.
        /// 
        /// If cached state exists when wl_surface.commit is called in
        /// desynchronized mode, the pending state is added to the cached
        /// state, and applied as a whole. This invalidates the cache.
        /// 
        /// Note: even if a sub-surface is set to desynchronized, a parent
        /// sub-surface may override it to behave as synchronized. For details,
        /// see wl_subsurface.
        /// 
        /// If a surface's parent surface behaves as desynchronized, then
        /// the cached state is applied on set_desync.
        /// </summary>
        public void SetDesync()
        {
            SetDesync(Pointer);
        }

        public static void SetDesync(IntPtr pointer)
        {
            Marshal(pointer, SetDesyncOp);
        }

        #endregion
    }
}
