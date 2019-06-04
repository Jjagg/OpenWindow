namespace OpenWindow
{
    /// <summary>
    /// A global object as advertised by the Wayland compositor.
    /// </summary>
    public class WaylandGlobal
    {
        public string Interface { get; }
        public uint Name { get; }
        public uint Version { get; }

        public WaylandGlobal(string iface, uint name, uint version)
        {
            Interface = iface;
            Name = name;
            Version = version;
        }
    }
}
