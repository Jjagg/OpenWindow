// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

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
        private static readonly GraphicsBackend Backend = GraphicsBackend.Direct3D11;

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

            if (Backend == GraphicsBackend.OpenGL)
                ws.GlSettings.EnableOpenGl = true;

            var w = ws.CreateWindow(false);
            w.Bounds = new Rectangle(100, 100, 960, 540);
            w.Title = "Veldrid Tutorial";
            w.Show();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var wd = (Win32WindowData) w.GetPlatformData();
                switch (Backend)
                {
                    case GraphicsBackend.Direct3D11:
                        _graphicsDevice = GraphicsDevice.CreateD3D11(gdo, w.Handle, (uint) w.Bounds.Width, (uint) w.Bounds.Height);
                        break;
                    case GraphicsBackend.Vulkan:
                        _graphicsDevice = GraphicsDevice.CreateVulkan(gdo,
                            VkSurfaceSource.CreateWin32(wd.HInstance, w.Handle), (uint) w.Bounds.Width,
                            (uint) w.Bounds.Height);
                        break;
                    case GraphicsBackend.OpenGL:
                        throw new NotImplementedException();
                    case GraphicsBackend.Metal:
                        throw new NotSupportedException();
                    case GraphicsBackend.OpenGLES:
                        var scs = SwapchainSource.CreateWin32(w.Handle, wd.HInstance);
                        var scd = new SwapchainDescription(scs, (uint) w.Bounds.Width, (uint) w.Bounds.Height, null,
                            true);
                        GraphicsDevice.CreateOpenGLES(gdo, scd);
                        throw new NotSupportedException();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            CreateResources();

            while (!w.ShouldClose)
            {
                ws.WaitEvent();
                if (w.ShouldClose)
                    break;
                Draw();
            }

            DisposeResources();

            w.Dispose();
            ws.Dispose();
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
