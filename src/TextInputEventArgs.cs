namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.TextInput"/> event.
    /// </summary>
    public struct TextInputEventArgs
    {
        /// <summary>
        /// The character input in UTF-32.
        /// </summary>
        public readonly int Character;

        internal TextInputEventArgs(int character)
        {
            Character = character;
        }
    }
}
