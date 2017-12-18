// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Xml.Linq;
using ProtocolGeneratorHelper;

namespace WaylandSharpGen
{
    public class Argument
    {
        private const string NameAttrib = "name";
        private const string TypeAttrib = "type";
        private const string InterfaceAttrib = "interface";
        private const string AllowNullAttrib = "allow-null";

        public readonly string Name;
        public readonly ArgType Type;
        public readonly string Signature;
        public readonly string ParamType;
        public readonly string Interface;
        public readonly string InterfaceCls;
        public readonly bool ObjectType;
        public readonly bool AllowNull;

        public Argument(XElement element)
        {
            Name = element.Attribute(NameAttrib).Value;
            Enum.TryParse(element.Attribute(TypeAttrib).Value, true, out Type);
            ParamType = ArgToParamType(Type);
            Interface = element.Attribute(InterfaceAttrib)?.Value;
            InterfaceCls = Util.ToPascalCase(Interface);
            ObjectType = Type == ArgType.New_id || Type == ArgType.Object;
            var allowNull = element.Attribute(AllowNullAttrib);
            AllowNull = allowNull == null ? false : bool.Parse(allowNull.Value);
            Signature = (AllowNull ? "?" : string.Empty) + ArgToSig(Type);
        }
        
        private static char ArgToSig(ArgType type)
        {
            switch (type)
            {
                case ArgType.Int:
                    return 'i';
                case ArgType.Uint:
                    return 'u';
                case ArgType.Fixed:
                    return 'f';
                case ArgType.String:
                    return 's';
                case ArgType.Object:
                    return 'o';
                case ArgType.New_id:
                    return 'n';
                case ArgType.Array:
                    return 'a';
                case ArgType.Fd:
                    return 'h';
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static string ArgToParamType(ArgType type)
        {
            switch (type)
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
                    return "WlObject";
                case ArgType.New_id:
                    return "WlObject";
                case ArgType.Array:
                    return "WlArray";
                case ArgType.Fd:
                    return "int";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
