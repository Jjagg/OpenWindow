// This file was generated from an xml Wayland protocol specification

using System;

namespace OpenWindow.Backends.Wayland
{
    internal static partial class WlInterfaces
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

    internal partial class WlDisplay : WlProxy
    {
        private const int SyncOp = 0;
        private const int GetRegistryOp = 1;

        public static WlInterface Interface = new WlInterface("wl_display", 1, 2, 2);

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

        public WlDisplay(IntPtr pointer)
            : base(pointer) { }

        public WlCallback Sync()
        {
            var ptr = MarshalConstructor(Pointer, SyncOp, WlCallback.Interface.Pointer, IntPtr.Zero);
            return new WlCallback(ptr);
        }

        public WlRegistry GetRegistry()
        {
            var ptr = MarshalConstructor(Pointer, GetRegistryOp, WlRegistry.Interface.Pointer, IntPtr.Zero);
            return new WlRegistry(ptr);
        }
    }

    internal partial class WlRegistry : WlProxy
    {
        private const int BindOp = 0;

        public static WlInterface Interface = new WlInterface("wl_registry", 1, 1, 2);

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

        public WlRegistry(IntPtr pointer)
            : base(pointer) { }

        public T Bind<T>(uint name, WlInterface iface)
            where T : WlObject
        {
            var args = new ArgumentList(name);
            var ptr = MarshalArrayConstructor(Pointer, BindOp, args.Pointer, iface.Pointer);
            args.Dispose();
            return (T) Activator.CreateInstance(typeof(T), new [] { ptr });
        }
    }

    internal partial class WlCallback : WlObject
    {

        public static WlInterface Interface = new WlInterface("wl_callback", 1, 0, 1);


        static WlCallback()
        {
            Interface.SetRequests(new WlMessage[0]);
            Interface.Finish();
        }

        public static void CleanUp()
        {
            Interface.Dispose();
        }

        public WlCallback(IntPtr pointer)
            : base(pointer) { }
    }

    internal partial class WlCompositor : WlProxy
    {
        private const int CreateSurfaceOp = 0;
        private const int CreateRegionOp = 1;

        public static WlInterface Interface = new WlInterface("wl_compositor", 4, 2, 0);

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

        public WlCompositor(IntPtr pointer)
            : base(pointer) { }

        public WlSurface CreateSurface()
        {
            var ptr = MarshalConstructor(Pointer, CreateSurfaceOp, WlSurface.Interface.Pointer, IntPtr.Zero);
            return new WlSurface(ptr);
        }

        public WlRegion CreateRegion()
        {
            var ptr = MarshalConstructor(Pointer, CreateRegionOp, WlRegion.Interface.Pointer, IntPtr.Zero);
            return new WlRegion(ptr);
        }
    }

    internal partial class WlShmPool : WlProxy
    {
        private const int CreateBufferOp = 0;
        private const int DestroyOp = 1;
        private const int ResizeOp = 2;

        public static WlInterface Interface = new WlInterface("wl_shm_pool", 1, 3, 0);

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

        public WlShmPool(IntPtr pointer)
            : base(pointer) { }

        public WlBuffer CreateBuffer(int offset, int width, int height, int stride, uint format)
        {
            var args = new ArgumentList(offset, width, height, stride, format);
            var ptr = MarshalArrayConstructor(Pointer, CreateBufferOp, args.Pointer, WlBuffer.Interface.Pointer);
            args.Dispose();
            return new WlBuffer(ptr);
        }

        public void Destroy()
        {
            Marshal(Pointer, DestroyOp);
        }

        public void Resize(int size)
        {
            Marshal(Pointer, ResizeOp);
        }
    }

    internal partial class WlShm : WlProxy
    {
        private const int CreatePoolOp = 0;

        public static WlInterface Interface = new WlInterface("wl_shm", 1, 1, 1);

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

        public WlShm(IntPtr pointer)
            : base(pointer) { }

        public WlShmPool CreatePool(int fd, int size)
        {
            var args = new ArgumentList(fd, size);
            var ptr = MarshalArrayConstructor(Pointer, CreatePoolOp, args.Pointer, WlShmPool.Interface.Pointer);
            args.Dispose();
            return new WlShmPool(ptr);
        }
    }

    internal partial class WlBuffer : WlProxy
    {
        private const int DestroyOp = 0;

        public static WlInterface Interface = new WlInterface("wl_buffer", 1, 1, 1);

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

        public WlBuffer(IntPtr pointer)
            : base(pointer) { }

        public void Destroy()
        {
            Marshal(Pointer, DestroyOp);
        }
    }

    internal partial class WlDataOffer : WlProxy
    {
        private const int AcceptOp = 0;
        private const int ReceiveOp = 1;
        private const int DestroyOp = 2;
        private const int FinishOp = 3;
        private const int SetActionsOp = 4;

        public static WlInterface Interface = new WlInterface("wl_data_offer", 3, 5, 3);

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

        public WlDataOffer(IntPtr pointer)
            : base(pointer) { }

        public void Accept(uint serial, string mime_type)
        {
            var args = new ArgumentList(serial, mime_type);
            MarshalArray(Pointer, AcceptOp, args.Pointer);
            args.Dispose();
        }

        public void Receive(string mime_type, int fd)
        {
            var args = new ArgumentList(mime_type, fd);
            MarshalArray(Pointer, ReceiveOp, args.Pointer);
            args.Dispose();
        }

        public void Destroy()
        {
            Marshal(Pointer, DestroyOp);
        }

        public void Finish()
        {
            Marshal(Pointer, FinishOp);
        }

        public void SetActions(uint dnd_actions, uint preferred_action)
        {
            var args = new ArgumentList(dnd_actions, preferred_action);
            MarshalArray(Pointer, SetActionsOp, args.Pointer);
            args.Dispose();
        }
    }

    internal partial class WlDataSource : WlProxy
    {
        private const int OfferOp = 0;
        private const int DestroyOp = 1;
        private const int SetActionsOp = 2;

        public static WlInterface Interface = new WlInterface("wl_data_source", 3, 3, 6);

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

        public WlDataSource(IntPtr pointer)
            : base(pointer) { }

        public void Offer(string mime_type)
        {
            Marshal(Pointer, OfferOp);
        }

        public void Destroy()
        {
            Marshal(Pointer, DestroyOp);
        }

        public void SetActions(uint dnd_actions)
        {
            Marshal(Pointer, SetActionsOp);
        }
    }

    internal partial class WlDataDevice : WlProxy
    {
        private const int StartDragOp = 0;
        private const int SetSelectionOp = 1;
        private const int ReleaseOp = 2;

        public static WlInterface Interface = new WlInterface("wl_data_device", 3, 3, 6);

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

        public WlDataDevice(IntPtr pointer)
            : base(pointer) { }

        public void StartDrag(WlObject source, WlObject origin, WlObject icon, uint serial)
        {
            var args = new ArgumentList(WlDataSource.Interface.Pointer, WlSurface.Interface.Pointer, WlSurface.Interface.Pointer, serial);
            MarshalArray(Pointer, StartDragOp, args.Pointer);
            args.Dispose();
        }

        public void SetSelection(WlObject source, uint serial)
        {
            var args = new ArgumentList(WlDataSource.Interface.Pointer, serial);
            MarshalArray(Pointer, SetSelectionOp, args.Pointer);
            args.Dispose();
        }

        public void Release()
        {
            Marshal(Pointer, ReleaseOp);
        }
    }

    internal partial class WlDataDeviceManager : WlProxy
    {
        private const int CreateDataSourceOp = 0;
        private const int GetDataDeviceOp = 1;

        public static WlInterface Interface = new WlInterface("wl_data_device_manager", 3, 2, 0);

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

        public WlDataDeviceManager(IntPtr pointer)
            : base(pointer) { }

        public WlDataSource CreateDataSource()
        {
            var ptr = MarshalConstructor(Pointer, CreateDataSourceOp, WlDataSource.Interface.Pointer, IntPtr.Zero);
            return new WlDataSource(ptr);
        }

        public WlDataDevice GetDataDevice(WlObject seat)
        {
            var args = new ArgumentList(WlSeat.Interface.Pointer);
            var ptr = MarshalArrayConstructor(Pointer, GetDataDeviceOp, args.Pointer, WlDataDevice.Interface.Pointer);
            args.Dispose();
            return new WlDataDevice(ptr);
        }
    }

    internal partial class WlShell : WlProxy
    {
        private const int GetShellSurfaceOp = 0;

        public static WlInterface Interface = new WlInterface("wl_shell", 1, 1, 0);

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

        public WlShell(IntPtr pointer)
            : base(pointer) { }

        public WlShellSurface GetShellSurface(WlObject surface)
        {
            var args = new ArgumentList(WlSurface.Interface.Pointer);
            var ptr = MarshalArrayConstructor(Pointer, GetShellSurfaceOp, args.Pointer, WlShellSurface.Interface.Pointer);
            args.Dispose();
            return new WlShellSurface(ptr);
        }
    }

    internal partial class WlShellSurface : WlProxy
    {
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

        public static WlInterface Interface = new WlInterface("wl_shell_surface", 1, 10, 3);

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

        public WlShellSurface(IntPtr pointer)
            : base(pointer) { }

        public void Pong(uint serial)
        {
            Marshal(Pointer, PongOp);
        }

        public void Move(WlObject seat, uint serial)
        {
            var args = new ArgumentList(WlSeat.Interface.Pointer, serial);
            MarshalArray(Pointer, MoveOp, args.Pointer);
            args.Dispose();
        }

        public void Resize(WlObject seat, uint serial, uint edges)
        {
            var args = new ArgumentList(WlSeat.Interface.Pointer, serial, edges);
            MarshalArray(Pointer, ResizeOp, args.Pointer);
            args.Dispose();
        }

        public void SetToplevel()
        {
            Marshal(Pointer, SetToplevelOp);
        }

        public void SetTransient(WlObject parent, int x, int y, uint flags)
        {
            var args = new ArgumentList(WlSurface.Interface.Pointer, x, y, flags);
            MarshalArray(Pointer, SetTransientOp, args.Pointer);
            args.Dispose();
        }

        public void SetFullscreen(uint method, uint framerate, WlObject output)
        {
            var args = new ArgumentList(method, framerate, WlOutput.Interface.Pointer);
            MarshalArray(Pointer, SetFullscreenOp, args.Pointer);
            args.Dispose();
        }

        public void SetPopup(WlObject seat, uint serial, WlObject parent, int x, int y, uint flags)
        {
            var args = new ArgumentList(WlSeat.Interface.Pointer, serial, WlSurface.Interface.Pointer, x, y, flags);
            MarshalArray(Pointer, SetPopupOp, args.Pointer);
            args.Dispose();
        }

        public void SetMaximized(WlObject output)
        {
            Marshal(Pointer, SetMaximizedOp);
        }

        public void SetTitle(string title)
        {
            Marshal(Pointer, SetTitleOp);
        }

        public void SetClass(string class_)
        {
            Marshal(Pointer, SetClassOp);
        }
    }

    internal partial class WlSurface : WlProxy
    {
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

        public static WlInterface Interface = new WlInterface("wl_surface", 4, 10, 2);

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

        public WlSurface(IntPtr pointer)
            : base(pointer) { }

        public void Destroy()
        {
            Marshal(Pointer, DestroyOp);
        }

        public void Attach(WlObject buffer, int x, int y)
        {
            var args = new ArgumentList(WlBuffer.Interface.Pointer, x, y);
            MarshalArray(Pointer, AttachOp, args.Pointer);
            args.Dispose();
        }

        public void Damage(int x, int y, int width, int height)
        {
            var args = new ArgumentList(x, y, width, height);
            MarshalArray(Pointer, DamageOp, args.Pointer);
            args.Dispose();
        }

        public WlCallback Frame()
        {
            var ptr = MarshalConstructor(Pointer, FrameOp, WlCallback.Interface.Pointer, IntPtr.Zero);
            return new WlCallback(ptr);
        }

        public void SetOpaqueRegion(WlObject region)
        {
            Marshal(Pointer, SetOpaqueRegionOp);
        }

        public void SetInputRegion(WlObject region)
        {
            Marshal(Pointer, SetInputRegionOp);
        }

        public void Commit()
        {
            Marshal(Pointer, CommitOp);
        }

        public void SetBufferTransform(int transform)
        {
            Marshal(Pointer, SetBufferTransformOp);
        }

        public void SetBufferScale(int scale)
        {
            Marshal(Pointer, SetBufferScaleOp);
        }

        public void DamageBuffer(int x, int y, int width, int height)
        {
            var args = new ArgumentList(x, y, width, height);
            MarshalArray(Pointer, DamageBufferOp, args.Pointer);
            args.Dispose();
        }
    }

    internal partial class WlSeat : WlProxy
    {
        private const int GetPointerOp = 0;
        private const int GetKeyboardOp = 1;
        private const int GetTouchOp = 2;
        private const int ReleaseOp = 3;

        public static WlInterface Interface = new WlInterface("wl_seat", 6, 4, 2);

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

        public WlSeat(IntPtr pointer)
            : base(pointer) { }

        public WlPointer GetPointer()
        {
            var ptr = MarshalConstructor(Pointer, GetPointerOp, WlPointer.Interface.Pointer, IntPtr.Zero);
            return new WlPointer(ptr);
        }

        public WlKeyboard GetKeyboard()
        {
            var ptr = MarshalConstructor(Pointer, GetKeyboardOp, WlKeyboard.Interface.Pointer, IntPtr.Zero);
            return new WlKeyboard(ptr);
        }

        public WlTouch GetTouch()
        {
            var ptr = MarshalConstructor(Pointer, GetTouchOp, WlTouch.Interface.Pointer, IntPtr.Zero);
            return new WlTouch(ptr);
        }

        public void Release()
        {
            Marshal(Pointer, ReleaseOp);
        }
    }

    internal partial class WlPointer : WlProxy
    {
        private const int SetCursorOp = 0;
        private const int ReleaseOp = 1;

        public static WlInterface Interface = new WlInterface("wl_pointer", 6, 2, 9);

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

        public WlPointer(IntPtr pointer)
            : base(pointer) { }

        public void SetCursor(uint serial, WlObject surface, int hotspot_x, int hotspot_y)
        {
            var args = new ArgumentList(serial, WlSurface.Interface.Pointer, hotspot_x, hotspot_y);
            MarshalArray(Pointer, SetCursorOp, args.Pointer);
            args.Dispose();
        }

        public void Release()
        {
            Marshal(Pointer, ReleaseOp);
        }
    }

    internal partial class WlKeyboard : WlProxy
    {
        private const int ReleaseOp = 0;

        public static WlInterface Interface = new WlInterface("wl_keyboard", 6, 1, 6);

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

        public WlKeyboard(IntPtr pointer)
            : base(pointer) { }

        public void Release()
        {
            Marshal(Pointer, ReleaseOp);
        }
    }

    internal partial class WlTouch : WlProxy
    {
        private const int ReleaseOp = 0;

        public static WlInterface Interface = new WlInterface("wl_touch", 6, 1, 7);

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

        public WlTouch(IntPtr pointer)
            : base(pointer) { }

        public void Release()
        {
            Marshal(Pointer, ReleaseOp);
        }
    }

    internal partial class WlOutput : WlProxy
    {
        private const int ReleaseOp = 0;

        public static WlInterface Interface = new WlInterface("wl_output", 3, 1, 4);

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

        public WlOutput(IntPtr pointer)
            : base(pointer) { }

        public void Release()
        {
            Marshal(Pointer, ReleaseOp);
        }
    }

    internal partial class WlRegion : WlProxy
    {
        private const int DestroyOp = 0;
        private const int AddOp = 1;
        private const int SubtractOp = 2;

        public static WlInterface Interface = new WlInterface("wl_region", 1, 3, 0);

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

        public WlRegion(IntPtr pointer)
            : base(pointer) { }

        public void Destroy()
        {
            Marshal(Pointer, DestroyOp);
        }

        public void Add(int x, int y, int width, int height)
        {
            var args = new ArgumentList(x, y, width, height);
            MarshalArray(Pointer, AddOp, args.Pointer);
            args.Dispose();
        }

        public void Subtract(int x, int y, int width, int height)
        {
            var args = new ArgumentList(x, y, width, height);
            MarshalArray(Pointer, SubtractOp, args.Pointer);
            args.Dispose();
        }
    }

    internal partial class WlSubcompositor : WlProxy
    {
        private const int DestroyOp = 0;
        private const int GetSubsurfaceOp = 1;

        public static WlInterface Interface = new WlInterface("wl_subcompositor", 1, 2, 0);

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

        public WlSubcompositor(IntPtr pointer)
            : base(pointer) { }

        public void Destroy()
        {
            Marshal(Pointer, DestroyOp);
        }

        public WlSubsurface GetSubsurface(WlObject surface, WlObject parent)
        {
            var args = new ArgumentList(WlSurface.Interface.Pointer, WlSurface.Interface.Pointer);
            var ptr = MarshalArrayConstructor(Pointer, GetSubsurfaceOp, args.Pointer, WlSubsurface.Interface.Pointer);
            args.Dispose();
            return new WlSubsurface(ptr);
        }
    }

    internal partial class WlSubsurface : WlProxy
    {
        private const int DestroyOp = 0;
        private const int SetPositionOp = 1;
        private const int PlaceAboveOp = 2;
        private const int PlaceBelowOp = 3;
        private const int SetSyncOp = 4;
        private const int SetDesyncOp = 5;

        public static WlInterface Interface = new WlInterface("wl_subsurface", 1, 6, 0);

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

        public WlSubsurface(IntPtr pointer)
            : base(pointer) { }

        public void Destroy()
        {
            Marshal(Pointer, DestroyOp);
        }

        public void SetPosition(int x, int y)
        {
            var args = new ArgumentList(x, y);
            MarshalArray(Pointer, SetPositionOp, args.Pointer);
            args.Dispose();
        }

        public void PlaceAbove(WlObject sibling)
        {
            Marshal(Pointer, PlaceAboveOp);
        }

        public void PlaceBelow(WlObject sibling)
        {
            Marshal(Pointer, PlaceBelowOp);
        }

        public void SetSync()
        {
            Marshal(Pointer, SetSyncOp);
        }

        public void SetDesync()
        {
            Marshal(Pointer, SetDesyncOp);
        }
    }
}