using System;
using System.Collections.Generic;
using System.Xml.Linq;
using ProtocolGeneratorHelper;

namespace WaylandSharpGen
{
    public class Argument
    {
        private const string NameAttrib = "name";
        private const string TypeAttrib = "type";
        private const string InterfaceAttrib = "interface";
        private const string EnumAttrib = "enum";
        private const string AllowNullAttrib = "allow-null";

        public readonly string Name;
        public readonly ArgType Type;
        public readonly string EnumType;
        public readonly string Signature;
        public readonly string ParamType;
        public readonly string Interface;
        public readonly bool AllowNull;

        public bool IsEnumType => EnumType != null;

        public Argument(XElement element)
        {
            Name = element.Attribute(NameAttrib).Value;
            Enum.TryParse(element.Attribute(TypeAttrib).Value, true, out Type);
            EnumType = element.Attribute(EnumAttrib)?.Value;
            Interface = element.Attribute(InterfaceAttrib)?.Value;
            var allowNull = element.Attribute(AllowNullAttrib);
            AllowNull = allowNull == null ? false : bool.Parse(allowNull.Value);
            Signature = (AllowNull ? "?" : string.Empty) + ArgToSig(Type, Interface != null);

            ParamType = GetParamType();
        }

        internal IEnumerable<string> GetSignatureTypes()
        {
            if (Type == ArgType.New_id && Interface == null)
            {
                // n becomes sun so we must return the types for s and u here
                yield return string.Empty;
                yield return string.Empty;
            }
            yield return Interface != null ? Interface : string.Empty;
        }

        private static string ArgToSig(ArgType type, bool hasInterface)
        {
            switch (type)
            {
                case ArgType.Int:
                    return "i";
                case ArgType.Uint:
                    return "u";
                case ArgType.Fixed:
                    return "f";
                case ArgType.String:
                    return "s";
                case ArgType.Object:
                    return "o";
                case ArgType.New_id:
                    // From https://lists.freedesktop.org/archives/wayland-devel/2014-September/017074.html
                    // new_id's without interface specified in the protocol (such as the one in wl_registry::bind) 
                    // must come with 3 arguments (s: interface name, u: version, n: the actual new_id).
                    // 
                    // Bug report to add to spec: https://gitlab.freedesktop.org/wayland/wayland/issues/27
                    return hasInterface ? "n" : "sun";
                case ArgType.Array:
                    return "a";
                case ArgType.Fd:
                    return "h";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private string GetParamType()
        {
            if (IsEnumType)
                return null;

            switch (Type)
            {
                case ArgType.Int:
                    return "int";
                case ArgType.Uint:
                    return "uint";
                case ArgType.Fixed:
                    return "int";
                case ArgType.String:
                    return "string";
                case ArgType.Object:
                    return Interface == null ? "uint" : Interface + '*';
                case ArgType.New_id:
                    return "uint";
                case ArgType.Array:
                    return "wl_array*";
                case ArgType.Fd:
                    return "int";
                default:
                    throw new ArgumentOutOfRangeException(nameof(Type), Type, null);
            }
        }
    }
}
