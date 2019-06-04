# OpenWindow [![Build Status](https://dev.azure.com/jessegielen/jessegielen/_apis/build/status/Jjagg.OpenWindow?branchName=master)](https://dev.azure.com/jessegielen/jessegielen/_build/latest?definitionId=1&branchName=master)

OpenWindow is a project that aims to offer a simple C# API that calls into the running 
native windowing backend without any layer in between. It targets .NET Standard 1.3 and 2.0.

## Goals

- Intuitive API
- Only C#, no C/C++ or any other language for interop
- Support for keyboard, mouse and touch input
- No dependencies other than the native libs that come installed with the windowing backend
- Multiple backends
  - Win32 (Windows)
  - X11 using XCB (Linux)
  - Wayland (Linux)
  - Quartz (macOS)

### Non-goals

- Rendering. No graphical backend will be implemented whatsoever. OpenWindow does allow passing parameters to window creation for OpenGL support (e.g. surface format, depth buffer).
- Mobile platform support. This might change in the future, but currently the API is targeted for desktop, so a large chunk wouldn't be usable on mobile platforms.

## State

The project is very much a work in progress. The Win32 backend is pretty far along, but the others cannot be used yet.
The API is not final, but I don't expect major changes.

### Win32 (Windows)

The Win32 backend has most of what you'd expect from a library like this. Check out the HelloOpenWindow sample or the Window API to see what's supported.

To do:
- Touch
- Drag and drop
- Clipboard
- Cursor appearance

### X (Linux)

The X backend requires a generator to generate the client-side library from xml files that define the protocol. I'm working on parsing the files and generating the bindings.

### Wayland (Linux)

Wayland bindings are generated and the implementation is in a usable state. Needs a lot more testing though.

### Quartz (OSX)

Not started. I don't have a Mac device to test with. Any help would be most welcome :)
If you have a Mac and want to help implement the macOS backend, please open an issue for discussion.


## Building

Install .NET Core 2.0+ and dotnet CLI, and run `dotnet build` on any of the projects.

## License

OpenWindow is licensed under the MIT license. See the LICENSE.txt file.

