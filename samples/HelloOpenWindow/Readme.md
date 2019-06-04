# Hello OpenWindow

This sample creates a window, supports some key input to manipulate it and prints events to console.

If you see some weird artifacts in the window (including an old school border),
that's because nothing is drawn to it.

On Wayland no window will show, because no buffer is bound to the window surface.

Inputs:

- **b**: Toggle border
- **r**: Toggle resizable
- **h**: Hide the window for 2 seconds
- **m**: Move the window to a random position with a random size
- **p**: Print some information about the window to console
- **j**: Minimize the window
- **k**: Restore the window
- **l**: Maximize the window
- **c**: Toggle cursor visibility
- **Esc**: Close the window

