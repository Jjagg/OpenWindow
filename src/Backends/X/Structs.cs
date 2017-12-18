// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;

namespace OpenWindow.Backends.X
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Screen
    {
        IntPtr root;
        IntPtr default_colormap;
        uint white_pixel;
        uint black_pixel;
        uint current_input_masks;
        ushort width_in_pixels;
        ushort height_in_pixels;
        ushort width_in_millimeters;
        ushort height_in_millimeters;
        ushort min_installed_maps;
        ushort max_installed_maps;
        IntPtr root_visual;
        byte backing_stores;
        byte save_unders;
        byte root_depth;
        byte allowed_depths_len;
    }
}