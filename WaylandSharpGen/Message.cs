using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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
        public readonly string Signature;
        public readonly string[] Types;

        public Message(XElement element)
        {
            Element = element;

            RawName = element.Attribute(NameAttrib).Value;
            NiceName = Util.ToPascalCase(RawName);
            Arguments = element.Elements(ArgElement).Select(e => new Argument(e)).ToArray();
            Signature = Arguments.Any()
                ? Arguments.Select(a => a.Signature).Aggregate(string.Concat)
                : string.Empty;
            var types = new List<string>();
            foreach (var a in Arguments)
            {
                if (a.Interface != null)
                    types.Add(a.InterfaceCls);
            }
            Types = types.ToArray();
        }

        public string EventDelegate()
        {
            var sb = new StringBuilder($"public delegate void {NiceName}Handler(IntPtr data, IntPtr iface");
            foreach (var a in Arguments)
            {
                sb.Append(", ");
                sb.Append(a.ParamType == "WlObject" ? "IntPtr" : a.ParamType);
                // @ in case argument names are C# keywords
                sb.Append(" @");
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
            if (Types.Any())
            {
                sb.Append("\", new [] {");
                sb.Append(Types.Select(t => $"{t}.Interface").Aggregate((s1, s2) => s1 + ", " + s2));
                sb.Append("})");
            }
            else
            {
                sb.Append("\", new WlInterface [0])");
            }
            return sb.ToString();

        }
    }
}