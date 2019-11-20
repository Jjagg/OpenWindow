﻿// This sample was adapted from the Veldrid Getting Started sample:
// https://github.com/mellinoe/veldrid-samples/blob/master/src/GettingStarted/Program.cs

using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using OpenWindow;
using OpenWindow.GL;
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
            var ws = WindowingService.Create();

            if (GraphicsBackend == GraphicsBackend.OpenGL)
                ws.GlSettings.EnableOpenGl = true;

            var wci = new WindowCreateInfo(100, 100, 960, 540, "Veldrid with OpenWindow");
            var w = ws.CreateWindow(ref wci);

            var serviceData = ws.GetPlatformData();
            var windowData = w.GetPlatformData();
            switch (windowData.Backend)
            {
                case WindowingBackend.Win32:
                    InitWindows(ws, w, gdo);
                    break;
                case WindowingBackend.Wayland:
                    InitWayland(ws, w, gdo);
                    break;
                default:
                    throw new NotImplementedException();
            }

            Console.WriteLine("Windowing backend: " + windowData.Backend);
            Console.WriteLine("Graphics backend: " + GraphicsBackend);

            CreateResources();

            w.CloseRequested += (s, e) => Console.WriteLine("Received request to close the window!");
            w.Closing += (s, e) => Console.WriteLine("Closing the window! Bye :)");
            w.MouseFocusChanged += (ww, e) => Console.WriteLine($"Mouse focus: {e.HasFocus}");
            w.MouseScroll += (s, e) => Console.WriteLine($"Mouse scrolled [{e.X}, {e.Y}]");
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

            ws.DestroyWindow(w);
            ws.Dispose();

            DisposeResources();
        }

        private static void InitWindows(WindowingService ws, Window w, in GraphicsDeviceOptions gdo)
        {
            var wsd = (Win32WindowingServiceData) ws.GetPlatformData();
            var wd = (Win32WindowData) w.GetPlatformData();

            switch (GraphicsBackend)
            {
                case GraphicsBackend.Direct3D11:
                    _graphicsDevice = GraphicsDevice.CreateD3D11(gdo, wd.Hwnd, (uint) w.ClientBounds.Width, (uint) w.ClientBounds.Height);
                    break;
                case GraphicsBackend.Vulkan:
                    _graphicsDevice = GraphicsDevice.CreateVulkan(gdo,
                        VkSurfaceSource.CreateWin32(wsd.HInstance, wd.Hwnd), (uint) w.ClientBounds.Width, (uint) w.ClientBounds.Height);
                    break;
                case GraphicsBackend.OpenGL:
                    throw new NotSupportedException();
                case GraphicsBackend.Metal:
                    throw new NotSupportedException();
                case GraphicsBackend.OpenGLES:
                    var scs = SwapchainSource.CreateWin32(wd.Hwnd, wsd.HInstance);
                    var scd = new SwapchainDescription(scs, (uint) w.ClientBounds.Width, (uint) w.ClientBounds.Height, null, true);
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

        private static void InitWayland(WindowingService ws, Window w, GraphicsDeviceOptions gdo)
        {
            var wsdata = (WaylandWindowingServiceData) ws.GetPlatformData();
            var wdata = (WaylandWindowData) w.GetPlatformData();

            const uint width = 960;
            const uint height = 540;
            switch (GraphicsBackend)
            {
                case GraphicsBackend.Direct3D11:
                    throw new PlatformNotSupportedException("Direct3D11 is not supported on Wayland.");
                case GraphicsBackend.Vulkan:
                    var sws = SwapchainSource.CreateWayland(wsdata.WlDisplay, wdata.WlSurface);
                    var scd = new SwapchainDescription(sws, width, height, null, true);
                    _graphicsDevice = GraphicsDevice.CreateVulkan(gdo, scd);
                    break;
                case GraphicsBackend.OpenGL:
                    OpenWindowGl.Initialize(ws);
                    var glctx = OpenWindowGl.CreateContext(w, 3, 1);
                    if (glctx == null)
                        throw new Exception("EGL context creation failed.");

                    if (!OpenWindowGl.MakeCurrent(w, glctx))
                        throw new Exception("EGL make current failed.");

                    var glpi = new global::Veldrid.OpenGL.OpenGLPlatformInfo(
                        glctx,
                        OpenWindowGl.GetProcAddress,
                        (ctxptr) => OpenWindowGl.MakeCurrent(w, ctxptr),
                        OpenWindowGl.GetCurrentContext,
                        () => OpenWindowGl.MakeCurrent(null, IntPtr.Zero),
                        (ctxptr) => OpenWindowGl.DestroyContext(ctxptr),
                        () => OpenWindowGl.SwapBuffers(w),
                        vsync => OpenWindowGl.SetVSync(vsync ? VSyncState.On : VSyncState.Off));
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
