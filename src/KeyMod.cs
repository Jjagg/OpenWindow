using System;

namespace OpenWindow
{
    /// <summary>
    /// Input modifiers.
    /// </summary>
    [Flags]
    public enum KeyMod
    {
        /// <summary>
        /// No modifier.
        /// </summary>
        None    = 0,
        /// <summary>
        /// Control modifier.
        /// </summary>
        Control = 1,
        /// <summary>
        /// Shift modifier.
        /// </summary>
        Shift   = 1 << 1,
        /// <summary>
        /// Alt modifier.
        /// </summary>
        Alt     = 1 << 2
    }
}