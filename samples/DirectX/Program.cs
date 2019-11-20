using System;
using System.Threading;
using OpenWindow;
using SharpDX.DXGI;
using D3D11 = SharpDX.Direct3D11;
using SharpDX.Direct3D;
using SharpDX.Mathematics.Interop;
using System.Runtime.InteropServices;
using SharpDX.D3DCompiler;

namespace SharpDX
{
    class Program
    {
        private static Window _window;
        private static D3D11.Device _d3dDevice;
        private static D3D11.DeviceContext _d3dDeviceContext;
        private static SwapChain _swapChain;
        private static D3D11.RenderTargetView _renderTargetView;
        private static D3D11.Buffer _vertexBuffer;

        private static D3D11.VertexShader _vertexShader;
        private static D3D11.PixelShader _pixelShader;

        private static D3D11.InputLayout _inputLayout;
        private static ShaderSignature _inputSignature;

        private static RawViewportF _viewport;

        static void Main(string[] args)
        {
            var owService = WindowingService.Create();

            var wci = new WindowCreateInfo(100, 100, 600, 600, "I'm rendering with DirectX11!");
            _window = owService.CreateWindow(ref wci);

            Initialize();
            BuildTriangle();
            BuildAndBindShaders();

            _window.KeyDown += (s, e) => Console.WriteLine($"Key down: {e.Key} ({e.ScanCode})");
            _window.KeyUp += (s, e) => Console.WriteLine($"Key up: {e.Key} ({e.ScanCode})");
            _window.TextInput += (s, e) => Console.WriteLine($"Text input: {char.ConvertFromUtf32(e.Character)}");

            while (!_window.ShouldClose)
            {
                Draw();
                Thread.Sleep(10);
                owService.PumpEvents();
            }

            _inputLayout.Dispose();
            _inputSignature.Dispose();
            _vertexBuffer.Dispose();
            _vertexShader.Dispose();
            _pixelShader.Dispose();
            _renderTargetView.Dispose();
            _swapChain.Dispose();
            _d3dDevice.Dispose();
            _d3dDeviceContext.Dispose();
        }

        public static void Initialize()
        {
            var wpd = _window.GetPlatformData();
            if (wpd.Backend != WindowingBackend.Win32)
                throw new PlatformNotSupportedException($"The DirectX sample only runs on Win32, but the current backend is {wpd.Backend}.");

            var wd = (Win32WindowData) wpd;

            ModeDescription backBufferDesc = new ModeDescription(600, 600, new Rational(60, 1), Format.R8G8B8A8_UNorm);
            SwapChainDescription swapChainDesc = new SwapChainDescription()
            {
                ModeDescription = backBufferDesc,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = 1,
                OutputHandle = wd.Hwnd,
                IsWindowed = true
            };

            D3D11.Device.CreateWithSwapChain(DriverType.Hardware, D3D11.DeviceCreationFlags.None, swapChainDesc,
                out _d3dDevice, out _swapChain);
            _d3dDeviceContext = _d3dDevice.ImmediateContext;

            using (D3D11.Texture2D backBuffer = _swapChain.GetBackBuffer<D3D11.Texture2D>(0))
            {
                _renderTargetView = new D3D11.RenderTargetView(_d3dDevice, backBuffer);
            }

            _viewport = new RawViewportF
            {
                X = 0,
                Y = 0,
                Width = 600,
                Height = 600
            };
            _d3dDeviceContext.Rasterizer.SetViewport(_viewport);
        }

        public static void BuildTriangle()
        {
            Vertex[] vertices = 
            {
                new Vertex(new RawVector3(-1f, -1f, 0f), new RawColor3(0f, 1f, 0f)),
                new Vertex(new RawVector3(0f, 1f, 0f), new RawColor3(1f, 0f, 0f)),
                new Vertex(new RawVector3(1f, -1f, 0f), new RawColor3(0f, 0f, 1f)),
            };
            _vertexBuffer = D3D11.Buffer.Create(_d3dDevice, D3D11.BindFlags.VertexBuffer, vertices);
        }

        public static void BuildAndBindShaders()
        {
            D3D11.InputElement[] inputElements =
            {
                new D3D11.InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, D3D11.InputClassification.PerVertexData, 0),
                new D3D11.InputElement("COLOR", 0, Format.R32G32B32_Float, 12, 0, D3D11.InputClassification.PerVertexData, 0)
            };

            using (var byteCode = ShaderBytecode.CompileFromFile("vertex.hlsl", "main", "vs_4_0", ShaderFlags.Debug))
            {
                _inputSignature = ShaderSignature.GetInputOutputSignature(byteCode);
                _inputLayout = new D3D11.InputLayout(_d3dDevice, _inputSignature, inputElements);
                _d3dDeviceContext.InputAssembler.InputLayout = _inputLayout;
                _vertexShader = new D3D11.VertexShader(_d3dDevice, byteCode);
            }

            using (var pixelShaderByteCode = ShaderBytecode.CompileFromFile("pixel.hlsl", "main", "ps_4_0", ShaderFlags.Debug))
            {
                _pixelShader = new D3D11.PixelShader(_d3dDevice, pixelShaderByteCode);
            }

            _d3dDeviceContext.VertexShader.Set(_vertexShader);
            _d3dDeviceContext.PixelShader.Set(_pixelShader);
            _d3dDeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        }

        public static void Draw()
        {

            _d3dDeviceContext.OutputMerger.SetRenderTargets(_renderTargetView);
            _d3dDeviceContext.ClearRenderTargetView(_renderTargetView, new RawColor4(0f, 0f, 0f, 1f));

            _d3dDeviceContext.InputAssembler.SetVertexBuffers(0, new D3D11.VertexBufferBinding(_vertexBuffer, Marshal.SizeOf<Vertex>(), 0));
            _d3dDeviceContext.Draw(3, 0);

            _swapChain.Present(1, PresentFlags.None);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Vertex
        {
            public RawVector3 Position;
            public RawColor3 Color;

            public Vertex(RawVector3 position, RawColor3 color)
            {
                Position = position;
                Color = color;
            }
        }
    }
}
