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

## State

The project is very much a work in progress. The Win32 backend is pretty far along, but the others cannot be used yet.
The API is not final, but I don't expect major changes.

### Win32 (Windows)

The Win32 backend has most of what you'd expect from a library like this. Check out the HelloOpenWindow sample or the Window API to see what's supported.

To do:
- Drag and drop

### X (Linux)

The X backend requires a generator to generate the client-side library from xml files that define the protocol. I'm working on parsing the files and generating the bindings.

### Wayland (Linux)

Similar to X, it's recommended to generate Wayland bindings from the files that define the protocol. I'm working on generating the bindings here too.

### Quartz (Apple)

Not started.


## Building

Install .NET Core 2.0 and dotnet CLI, and run `dotnet build` on any of the projects.

## License

OpenWindow is licensed under the MIT license. See the LICENSE.txt file.

