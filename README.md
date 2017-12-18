Note that this project is very early in development. I have only worked on the Win32 back end and generators for a C# implementation of the XCB and Wayland protocols.

Because I am not familiar with the other windowing API's the current OpenWindow API reflects the Win32 API in some places. It will without a doubt change drastically in the future.

# OpenWindow

OpenWindow is a project that aims to offer a simple C# API that calls into the running 
native windowing backend without any layer in between. It currently targets .NET Standard 1.1.

## Goals

- Intuitive API
- Only C#, no C/C++ or any other language for interop
- Support for keyboard and mouse input
- No dependencies other than the native libs that come installed with the windowing backend
- Multiple backends
  - Win32
  - X11 using XCB
  - Wayland

### Non-goals

- Rendering. No graphical backend will be implemented whatsoever. OpenWindow does allow passing parameters to window creation for OpenGL support (e.g. surface format, depth buffer).
- Mobile platform support

## Building

Install .NET Core 2.0 and dotnet CLI, and run `dotnet build` on any of the projects.

## License

OpenWindow is licensed under the MIT license. See the LICENSE.txt file.

