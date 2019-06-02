// This sample was adapted from the Veldrid Getting Started sample:
// https://github.com/mellinoe/veldrid-samples/blob/master/src/GettingStarted/Program.cs

using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using OpenWindow;
using OpenWindow.Backends.Windows;
using Veldrid;
using Veldrid.Vk;

namespace VeldridApp
{
    class Program
    {
        private static readonly GraphicsBackend GraphicsBackend = GraphicsBackend.OpenGL;

        private static GraphicsDevice _graphicsDevice;
        private static CommandList _commandList;
        private static DeviceBuffer _vertexBuffer;
        private static DeviceBuffer _indexBuffer;
        private static Shader _vertexShader;
        private static Shader _fragmentShader;
        private static Pipeline _pipeline;

        static void Main(string[] args)
        {
            var gdo = new GraphicsDeviceOptions();
            var ws = WindowingService.Get();

            if (GraphicsBackend == GraphicsBackend.OpenGL)
                ws.GlSettings.EnableOpenGl = true;

            var w = ws.CreateWindow();
            w.ClientBounds = new Rectangle(100, 100, 960, 540);
            w.Title = "Veldrid Tutorial";

            var windowData = w.GetPlatformData();
            switch (windowData.Backend)
            {
                case WindowingBackend.Win32:
                    InitWindows(w, (Win32WindowData) windowData, gdo);
                    break;
                case WindowingBackend.Wayland:
                    InitWayland(w, (WaylandWindowData) windowData, gdo);
                    break;
                default:
                    throw new NotImplementedException();
            }

            Console.WriteLine("Windowing backend: " + windowData.Backend);
            Console.WriteLine("Graphics backend:  " + GraphicsBackend);

            CreateResources();

            w.CloseRequested += (s, e) => Console.WriteLine("Received request to close the window!");
            w.Closing += (s, e) => Console.WriteLine("Closing the window! Bye :)");
            w.MouseFocusChanged += (ww, e) => Console.WriteLine($"Mouse focus: {e.HasFocus}");
            w.MouseDown += (ww, e) => Console.WriteLine($"Button {e.Button} down");
            w.MouseUp += (ww, e) => Console.WriteLine($"Button {e.Button} up");
            w.KeyDown += (s, e) => Console.WriteLine($"Key Down: {e.Key} ({e.ScanCode})");
            //w.KeyUp += (s, e) => Console.WriteLine($"Key Up: {e.Key} ({e.ScanCode})");
            w.TextInput += (s, e) => Console.WriteLine($"Got text input: {char.ConvertFromUtf32(e.Character)}");

            Console.WriteLine("Running draw loop...");
            while (!w.ShouldClose)
            {
                ws.PumpEvents();
                if (w.ShouldClose)
                    break;
                Draw();
            }

            Console.WriteLine("Shutting down.");

            w.Dispose();
            ws.Dispose();

            DisposeResources();
        }

        private static void InitWindows(Window w, Win32WindowData wd, in GraphicsDeviceOptions gdo)
        {
            switch (GraphicsBackend)
            {
                case GraphicsBackend.Direct3D11:
                    _graphicsDevice = GraphicsDevice.CreateD3D11(gdo, wd.Hwnd, (uint) w.Bounds.Width, (uint) w.Bounds.Height);
                    break;
                case GraphicsBackend.Vulkan:
                    _graphicsDevice = GraphicsDevice.CreateVulkan(gdo,
                        VkSurfaceSource.CreateWin32(wd.HInstance, wd.Hwnd), (uint) w.Bounds.Width,
                        (uint) w.Bounds.Height);
                    break;
                case GraphicsBackend.OpenGL:
                    throw new NotSupportedException();
                case GraphicsBackend.Metal:
                    throw new NotSupportedException();
                case GraphicsBackend.OpenGLES:
                    var scs = SwapchainSource.CreateWin32(wd.Hwnd, wd.HInstance);
                    var scd = new SwapchainDescription(scs, (uint) w.Bounds.Width, (uint) w.Bounds.Height, null, true);
                    _graphicsDevice = GraphicsDevice.CreateOpenGLES(gdo, scd);
                    throw new NotSupportedException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static int EGL_NONE = 0x3038;
        // >= 1.3
        public static int EGL_CONTEXT_MAJOR_VERSION = 0x3098;
        // >= 1.4 + EGL_KHR_create_context
        // >= 1.5
        public static int EGL_CONTEXT_MINOR_VERSION = 0x30FB;

        private static void InitWayland(Window w, WaylandWindowData wdata, GraphicsDeviceOptions gdo)
        {
            const uint width = 960;
            const uint height = 540;
            switch (GraphicsBackend)
            {
                case GraphicsBackend.Direct3D11:
                    throw new PlatformNotSupportedException("Direct3D11 is not supported on Wayland.");
                case GraphicsBackend.Vulkan:
                    var sws = SwapchainSource.CreateWayland(wdata.WlDisplay, wdata.WlSurface);
                    var scd = new SwapchainDescription(sws, width, height, null, true);
                    _graphicsDevice = GraphicsDevice.CreateVulkan(gdo, scd);
                    break;
                case GraphicsBackend.OpenGL:
                    LoadEGL();

                    int[] attribs =
                    {
                        EGL_CONTEXT_MAJOR_VERSION, 3,
                        EGL_CONTEXT_MINOR_VERSION, 3,
                        EGL_NONE
                    };

                    var glctx = EGLCreateContext(wdata.EGLDisplay, wdata.EGLConfig, IntPtr.Zero, attribs);
                    if (glctx == null)
                        throw new Exception("EGL context creation failed.");

                    if (!EGLMakeCurrent(wdata.EGLDisplay, wdata.EGLSurface, wdata.EGLSurface, glctx))
                        throw new Exception("EGL make current failed.");

                    var glpi = new global::Veldrid.OpenGL.OpenGLPlatformInfo(
                        glctx,
                        str => LoadFuncRaw(str, EGLGetProcAddress),
                        (ctxptr) => EGLMakeCurrent(wdata.EGLDisplay, wdata.EGLSurface, wdata.EGLSurface, ctxptr),
                        () => EGLGetCurrentContext(),
                        () => EGLMakeCurrent(wdata.EGLDisplay, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero),
                        (ctxptr) => EGLDestroyContext(wdata.EGLDisplay, ctxptr),
                        () => EGLSwapBuffers(wdata.EGLDisplay, wdata.EGLSurface),
                        vsync => EGLSwapInterval(wdata.EGLDisplay, vsync ? 1 : 0));
                    _graphicsDevice = GraphicsDevice.CreateOpenGL(gdo, glpi, width, height);
                    break;
                case GraphicsBackend.OpenGLES:
                    throw new NotImplementedException();
                case GraphicsBackend.Metal:
                    throw new PlatformNotSupportedException("Metal is not supported on Wayland.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void CreateResources()
        {
            ResourceFactory factory = _graphicsDevice.ResourceFactory;

            VertexPositionColor[] quadVertices =
            {
                new VertexPositionColor(new Vector2(-.75f, .75f), RgbaFloat.Red),
                new VertexPositionColor(new Vector2(.75f, .75f), RgbaFloat.Green),
                new VertexPositionColor(new Vector2(-.75f, -.75f), RgbaFloat.Blue),
                new VertexPositionColor(new Vector2(.75f, -.75f), RgbaFloat.Yellow)
            };
            BufferDescription vbDescription = new BufferDescription(
                4 * VertexPositionColor.SizeInBytes,
                BufferUsage.VertexBuffer);
            _vertexBuffer = factory.CreateBuffer(vbDescription);
            _graphicsDevice.UpdateBuffer(_vertexBuffer, 0, quadVertices);

            ushort[] quadIndices = {0, 1, 2, 3};
            BufferDescription ibDescription = new BufferDescription(
                4 * sizeof(ushort),
                BufferUsage.IndexBuffer);
            _indexBuffer = factory.CreateBuffer(ibDescription);
            _graphicsDevice.UpdateBuffer(_indexBuffer, 0, quadIndices);

            VertexLayoutDescription vertexLayout = new VertexLayoutDescription(
                new VertexElementDescription("Position", VertexElementSemantic.Position, VertexElementFormat.Float2),
                new VertexElementDescription("Color", VertexElementSemantic.Color, VertexElementFormat.Float4));

            _vertexShader = LoadShader(ShaderStages.Vertex);
            _fragmentShader = LoadShader(ShaderStages.Fragment);

            // Create pipeline
            GraphicsPipelineDescription pipelineDescription = new GraphicsPipelineDescription();
            pipelineDescription.BlendState = BlendStateDescription.SingleOverrideBlend;
            pipelineDescription.DepthStencilState = new DepthStencilStateDescription(
                depthTestEnabled: true,
                depthWriteEnabled: true,
                comparisonKind: ComparisonKind.LessEqual);
            pipelineDescription.RasterizerState = new RasterizerStateDescription(
                cullMode: FaceCullMode.Back,
                fillMode: PolygonFillMode.Solid,
                frontFace: FrontFace.Clockwise,
                depthClipEnabled: true,
                scissorTestEnabled: false);
            pipelineDescription.PrimitiveTopology = PrimitiveTopology.TriangleStrip;
            pipelineDescription.ResourceLayouts = System.Array.Empty<ResourceLayout>();
            pipelineDescription.ShaderSet = new ShaderSetDescription(
                vertexLayouts: new VertexLayoutDescription[] {vertexLayout},
                shaders: new Shader[] {_vertexShader, _fragmentShader});
            pipelineDescription.Outputs = _graphicsDevice.SwapchainFramebuffer.OutputDescription;

            _pipeline = factory.CreateGraphicsPipeline(pipelineDescription);

            _commandList = factory.CreateCommandList();
        }

        private static Shader LoadShader(ShaderStages stage)
        {
            string extension = null;
            switch (_graphicsDevice.BackendType)
            {
                case GraphicsBackend.Direct3D11:
                    extension = "hlsl.bytes";
                    break;
                case GraphicsBackend.Vulkan:
                    extension = "spv";
                    break;
                case GraphicsBackend.OpenGL:
                    extension = "glsl";
                    break;
                case GraphicsBackend.Metal:
                    extension = "metallib";
                    break;
                default: throw new System.InvalidOperationException();
            }

            string entryPoint = stage == ShaderStages.Vertex ? "VS" : "FS";
            string path = Path.Combine(System.AppContext.BaseDirectory, "Shaders", $"{stage.ToString()}.{extension}");
            byte[] shaderBytes = File.ReadAllBytes(path);
            return _graphicsDevice.ResourceFactory.CreateShader(new ShaderDescription(stage, shaderBytes, entryPoint));
        }

        private static void Draw()
        {
            // Begin() must be called before commands can be issued.
            _commandList.Begin();

            // We want to render directly to the output window.
            _commandList.SetFramebuffer(_graphicsDevice.SwapchainFramebuffer);
            _commandList.ClearColorTarget(0, RgbaFloat.Black);

            // Set all relevant state to draw our quad.
            _commandList.SetVertexBuffer(0, _vertexBuffer);
            _commandList.SetIndexBuffer(_indexBuffer, IndexFormat.UInt16);
            _commandList.SetPipeline(_pipeline);
            // Issue a Draw command for a single instance with 4 indices.
            _commandList.DrawIndexed(
                indexCount: 4,
                instanceCount: 1,
                indexStart: 0,
                vertexOffset: 0,
                instanceStart: 0);

            // End() must be called before commands can be submitted for execution.
            _commandList.End();
            _graphicsDevice.SubmitCommands(_commandList);

            // Once commands have been submitted, the rendered image can be presented to the application window.
            _graphicsDevice.SwapBuffers();
        }

        private static void DisposeResources()
        {
            _pipeline.Dispose();
            _vertexShader.Dispose();
            _fragmentShader.Dispose();
            _commandList.Dispose();
            _vertexBuffer.Dispose();
            _indexBuffer.Dispose();
            _graphicsDevice.Dispose();
        }

        #region EGL

        public static void LoadEGL()
        {
            EGLCreateContext = LoadFunc<EGLCreateContextDelegate>("eglCreateContext", EGLGetProcAddress);
            EGLMakeCurrent = LoadFunc<EGLMakeCurrentDelegate>("eglMakeCurrent", EGLGetProcAddress);
            EGLGetCurrentContext = LoadFunc<EGLGetCurrentContextDelegate>("eglGetCurrentContext", EGLGetProcAddress);
            EGLDestroyContext = LoadFunc<EGLDestroyContextDelegate>("eglDestroyContext", EGLGetProcAddress);
            EGLSwapBuffers = LoadFunc<EGLSwapBuffersDelegate>("eglSwapBuffers", EGLGetProcAddress);
            EGLSwapInterval = LoadFunc<EGLSwapIntervalDelegate>("eglSwapInterval", EGLGetProcAddress);
        }

        public static T LoadFunc<T>(string func, Func<string, IntPtr> getProcAddress) where T : Delegate
        {
            var ptr = LoadFuncRaw(func, getProcAddress);
            if (ptr == IntPtr.Zero)
                return null;
            return Marshal.GetDelegateForFunctionPointer<T>(ptr);
        }

        public static IntPtr LoadFuncRaw(string func, Func<string, IntPtr> getProcAddress)
        {
            return getProcAddress(func);
        }

        [DllImport("libEGL.so", EntryPoint="eglGetProcAddress")]
        public static extern IntPtr EGLGetProcAddress(string procName);

        public delegate IntPtr EGLCreateContextDelegate(IntPtr display, IntPtr config, IntPtr shareContext, int[] attribList);
        public static EGLCreateContextDelegate EGLCreateContext;

        public delegate bool EGLMakeCurrentDelegate(IntPtr display, IntPtr draw, IntPtr read, IntPtr context);
        public static EGLMakeCurrentDelegate EGLMakeCurrent;

        public delegate IntPtr EGLGetCurrentContextDelegate();
        public static EGLGetCurrentContextDelegate EGLGetCurrentContext;

        public delegate bool EGLDestroyContextDelegate(IntPtr display, IntPtr context);
        public static EGLDestroyContextDelegate EGLDestroyContext;

        public delegate bool EGLSwapBuffersDelegate(IntPtr display, IntPtr surface);
        public static EGLSwapBuffersDelegate EGLSwapBuffers;

        public delegate bool EGLSwapIntervalDelegate(IntPtr display, int interval);
        public static EGLSwapIntervalDelegate EGLSwapInterval;

        #endregion
    }

    struct VertexPositionColor
    {
        public const uint SizeInBytes = 24;
        public Vector2 Position;
        public RgbaFloat Color;

        public VertexPositionColor(Vector2 position, RgbaFloat color)
        {
            Position = position;
            Color = color;
        }
    }
}
