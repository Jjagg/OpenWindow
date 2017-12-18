# Hello OpenWindow

This sample creates a window and supports some key input to manipulate it.

Note that the keystate API is currently tailored to Windows because that's 
the only implemented backend and I don't have a clue yet how I'll create 
a unified API for it.

If you see some weird artifacts in the window, that's because nothing is drawn to it.

Inputs:

- **b**: Toggle Borderless
- **r**: Move the window to a random position with a random size
- **Esc**: Close the window

