namespace OpenWindow
{
    /// <summary>
    /// Contains data for the <see cref="Window.TextInput"/> event.
    /// </summary>
    public class TextInputEventArgs : System.EventArgs
    {
        /// <summary>
        /// The character input.
        /// </summary>
        public readonly char Character;

        internal TextInputEventArgs(char character)
        {
            Character = character;
        }
    }
}
