// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

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

        private static bool _closing;

        static void Main(string[] args)
        {
            var owService = WindowingService.Get();
            _window = owService.CreateWindow();
            _window.Closing += HandleClosing;
            _window.ClientBounds = new OwRectangle(100, 100, 600, 600);
            _window.Title = "I'm rendering with DirectX11!";

            Initialize();
            BuildTriangle();
            BuildAndBindShaders();

            while (true)
            {
                owService.Update();
                if (_closing)
                    break;
                
                Draw();

                Thread.Sleep(10);
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

        private static void HandleClosing(object sender, EventArgs args)
        {
            _closing = true;
        }

        public static void Initialize()
        {
            ModeDescription backBufferDesc = new ModeDescription(600, 600, new Rational(60, 1), Format.R8G8B8A8_UNorm);
            SwapChainDescription swapChainDesc = new SwapChainDescription()
            {
                ModeDescription = backBufferDesc,
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = 1,
                OutputHandle = _window.Handle,
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
