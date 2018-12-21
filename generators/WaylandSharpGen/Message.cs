using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ProtocolGeneratorHelper;

namespace WaylandSharpGen
{
    public class Message
    {
        private const string ArgElement = "arg";
        private const string NameAttrib = "name";

        public readonly XElement Element;

        public readonly string RawName;
        public readonly string NiceName;
        public readonly Argument[] Arguments;
        public string Signature => Arguments.Any()
                ? Arguments.Select(a => a.Signature).Aggregate(string.Concat)
                : string.Empty;
        public readonly string[] Types;

        public Message(XElement element)
        {
            Element = element;

            RawName = element.Attribute(NameAttrib).Value;
            NiceName = Util.ToPascalCase(RawName);
            Arguments = element.Elements(ArgElement).Select(e => new Argument(e)).ToArray();
            var types = new List<string>();
            Types = Arguments.SelectMany(a => a.GetSignatureTypes()).ToArray();
        }

        public string EventDelegate()
        {
            var sb = new StringBuilder($"public delegate void {NiceName}Handler(IntPtr data, IntPtr iface");
            foreach (var a in Arguments)
            {
                sb.Append(", ");
                sb.Append(a.Type == ArgType.Object ? "IntPtr" : a.ParamType);
                sb.Append(" ");
                if (Util.IsCSharpKeyword(a.Name))
                    sb.Append("@");
                sb.Append(a.Name);
            }
            sb.Append(");");
            return sb.ToString();
        }

        public string Initializer()
        {
            var sb = new StringBuilder();
            sb.Append("new WlMessage(\"");
            sb.Append(RawName);
            sb.Append("\", \"");
            sb.Append(Signature);
            sb.Append("\", ");
            if (Types.Any())
            {
                sb.Append("new [] {");
                sb.Append(Types.Select(t => t == "" ? "IntPtr.Zero" : $"{t}.Interface.Pointer").Aggregate((s1, s2) => s1 + ", " + s2));
                sb.Append("})");
            }
            else
            {
                sb.Append("new IntPtr[0])");
            }
            return sb.ToString();

        }
    }
}
