using System.Linq;
using System.Xml.Linq;
using ProtocolGeneratorHelper;

namespace WaylandSharpGen
{
    public class Interface
    {
        private const string EventElement = "event";
        private const string RequestElement = "request";
        private const string NameAttrib = "name";
        private const string VersionAttrib = "version";

        public readonly XElement Element;

        public readonly string RawName;
        public readonly string ClsName;
        public readonly string Version;

        public readonly Message[] Requests;
        public readonly Message[] Events;

        public Interface(XElement element)
        {
            Element = element;
            RawName = element.Attribute(NameAttrib).Value;
            ClsName = Util.ToPascalCase(RawName);
            Version = element.Attribute(VersionAttrib)?.Value ?? "1";
            Requests = element.Elements(RequestElement).Select(e => new Message(e)).ToArray();
            Events = element.Elements(EventElement).Select(e => new Message(e)).ToArray();
        }
    }
}