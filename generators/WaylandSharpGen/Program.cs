using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;
using ProtocolGeneratorHelper;

namespace WaylandSharpGen
{
    /// <summary>
    /// This program generates a file with a C# implementation
    /// of a Wayland protocol client.
    /// </summary>
    internal class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length < 2 || (args.Length > 0 && (args[0] == "-h" || args[0] == "--help")))
            {
                Console.WriteLine("usage:  dotnet WaylandSharpGen.dll [protocol] [output]");
                return 0;
            }

            if (args.Length == 0)
                Console.WriteLine("No protocol path specified, using default path.");

            var path = args[0];
            Console.WriteLine($"Using protocol at '{path}'.");
            if (!File.Exists(path))
            {
                Console.WriteLine("Protocol file not found. Exiting...");
                return 1;
            }

            var fp = args[1];

            var doc = XDocument.Load(path);
            var w = new CSharpWriter();

            w.Line("// This file was generated from an xml Wayland protocol specification");
            w.Line("// by WaylandSharpGen. https://github.com/Jjagg/OpenWindow/tree/master/generators/WaylandSharpGen");
            w.Line();
            w.Line("#pragma warning disable CS0649");
            w.Line();
            WriteProtocol(doc, w);

            var dirName = Path.GetDirectoryName(fp);
            if (!string.IsNullOrWhiteSpace(dirName))
                Directory.CreateDirectory(dirName);
            File.WriteAllText(fp, w.ToString());
            Console.WriteLine($"Wrote output to '{fp}'.");
            return 0;
        }

        private static readonly bool GenerateComments = true;

        private const string ProtocolElement = "protocol";
        private const string DescriptionElement = "description";
        private const string InterfaceElement = "interface";
        private const string ArgElement = "arg";

        private const string NameAttrib = "name";
        private const string SummaryAttrib = "summary";

        private const string MessageCodeSuffix = "Msg";
        private const string OpCodeSuffix = "Op";

        private static string _protocolClassName;

        private static void WriteProtocol(XContainer doc, CSharpWriter w)
        {
            // for writing the high level API
            var hlw = new CSharpWriter();

            var protocolElement = doc.Element(ProtocolElement);
            if (protocolElement == null)
                throw new Exception("No protocol element found!");

            var protocolName = protocolElement.Attribute(NameAttrib).Value;
            w.Line($"// Protocol: {protocolName}");
            w.Line();

            w.Line("using System;");
            w.Line("using System.Runtime.InteropServices;");
            w.Line();

            hlw.Line("namespace OpenWindow.Backends.Wayland.Managed");
            hlw.OpenBlock();

            w.Line("namespace OpenWindow.Backends.Wayland");
            w.OpenBlock();

            var interfaceElements = protocolElement.Elements(InterfaceElement);
            var ifaces = interfaceElements.Select(e => new Interface(e)).ToArray();
            var messages = ifaces.SelectMany(iface => iface.Requests.Concat(iface.Events)).ToArray();
            var ifaceCount = ifaces.Length;
            var msgCount = messages.Length;

            _protocolClassName = Util.ToPascalCase(protocolName) + "Bindings";

            w.Line($"internal static unsafe partial class {_protocolClassName}");
            w.OpenBlock();

            w.Line("private static bool _loaded;");
            w.Line();
            w.Line("public static wl_interface* Interfaces;");
            w.Line("private static wl_message* _messages;");
            w.Line("private static wl_interface** _signatureTypes;");
            w.Line();
            w.Line($"private static readonly int InterfaceCount = {ifaceCount};");
            w.Line($"private static readonly int MessageCount = {msgCount};");

            w.Line();

            foreach (var iface in ifaces)
                w.Line($"public const string {iface.RawName}_name = \"{iface.RawName}\";");

            w.Line();

            w.Line("public static void Load()");
            w.OpenBlock();
            w.Line("if (_loaded)");
            w.LineIndented("return;");
            w.Line("_loaded = true;");
            w.Line();

            w.Line("Interfaces = (wl_interface*) Marshal.AllocHGlobal(sizeof(wl_interface) * InterfaceCount);");
            w.Line("_messages = (wl_message*) Marshal.AllocHGlobal(sizeof(wl_message) * MessageCount);");

            w.Line();

            w.Line();

            for (var i = 0; i < ifaces.Length; i++)
            {
                var iface = ifaces[i];
                w.Line($"Util.CreateInterface(&Interfaces[{i}], \"{iface.RawName}\", {iface.Version}, {iface.Requests.Length}, {iface.Events.Length});");
            }

            w.Line();

            var signatureTypes = new List<string>();
            var typeMap = new List<(string TypeStr, int Index)>();

            var typeMapIndex = 0;

            // handle messages with the most types first so we don't create duplicates
            foreach (var message in messages.OrderByDescending(m => m.Types.Length))
            {
                var typesIndex = 0;
                var typeStr = ';' + string.Join(";", message.Types) + ';';
                var match = typeMap.Where(t => t.TypeStr.Contains(typeStr));
                if (match.Any())
                {
                    var m = match.First();
                    var offset = m.TypeStr.Take(m.TypeStr.IndexOf(typeStr)).Count(c => c == ';');
                    typeMap.Add((typeStr, m.Index + offset));
                    typesIndex = m.Index + offset;
                }
                else
                {
                    typeMap.Add((typeStr, typeMapIndex));
                    typesIndex = typeMapIndex;

                    foreach (var type in message.Types)
                    {
                        var sigTypeStr = type == string.Empty ? null : type;
                        signatureTypes.Add(sigTypeStr);
                        typeMapIndex++;
                    }
                }

                message.TypesIndex = typesIndex;
            }

            w.Line($"_signatureTypes = (wl_interface**) Marshal.AllocHGlobal(sizeof(void*) * {signatureTypes.Count});");
            for (int i = 0; i < signatureTypes.Count; i++)
            {
                var stype = signatureTypes[i];
                var stypeStr = stype == null ? "null" : stype + ".Interface";
                w.Line($"_signatureTypes[{i}] = {stypeStr};");
            }

            w.Line();

            for (var i = 0; i < messages.Length; i++)
            {
                var message = messages[i];
                w.Line($"Util.CreateMessage(&_messages[{i}], \"{message.RawName}\", \"{message.Signature}\", &_signatureTypes[{message.TypesIndex}]);");
            }

            w.Line();

            var msgIndex = 0;
            for (var i = 0; i < ifaces.Length; i++)
            {
                var iface = ifaces[i];
                var reqInit = iface.Requests.Length == 0 ? "null" : $"&_messages[{msgIndex}]";
                w.Line($"Interfaces[{i}].Requests = {reqInit};");
                msgIndex += iface.Requests.Length;

                var evInit = iface.Events.Length == 0 ? "null" : $"&_messages[{msgIndex}]";
                w.Line($"Interfaces[{i}].Events = {evInit};");
                msgIndex += iface.Events.Length;
            }

            w.CloseBlock();
            w.Line();

            w.Line("public static void Unload()");
            w.OpenBlock();
            w.Line("if (!_loaded)");
            w.LineIndented("return;");
            w.Line("_loaded = false;");
            w.Line();

            w.Line("for (var i = 0; i < InterfaceCount; i++)");
            w.LineIndented($"Marshal.FreeHGlobal((IntPtr) Interfaces[i].Name);");

            w.Line();

            w.Line("for (var i = 0; i < MessageCount; i++)");
            w.OpenBlock();
            w.Line("Marshal.FreeHGlobal((IntPtr) _messages[i].Name);");
            w.Line("Marshal.FreeHGlobal((IntPtr) _messages[i].Signature);");
            w.CloseBlock();

            w.Line();
            w.Line("Marshal.FreeHGlobal((IntPtr) _messages);");
            w.Line("Marshal.FreeHGlobal((IntPtr) _signatureTypes);");
            w.Line("Marshal.FreeHGlobal((IntPtr) Interfaces);");
            w.CloseBlock();

            w.Line();

            foreach (var iface in ifaces)
            {
                hlw.Line($"internal unsafe partial struct {iface.ClsName}");
                hlw.OpenBlock();
                hlw.Line($"public static IntPtr Interface => (IntPtr) {iface.RawName}.Interface;");
                hlw.Line($"public readonly {iface.RawName}* Pointer;");
                hlw.Line("public bool IsNull => Pointer == null;");
                if (iface.Events.Length != 0)
                {
                    // cached delegates to prevent GC
                    foreach (var ev in iface.Events)
                        hlw.Line($"private {_protocolClassName}.{GetDelegateName(iface, ev)} _{ev.RawName};");
                    // unmanaged listener, needs to be freed by users
                    hlw.Line($"private {_protocolClassName}.{iface.RawName}_listener* _listener;");
                }
                // we need to initialize all fields in the ctor
                var eventInitializer = iface.Events.Length == 0 ? "" : 
                    " _listener = null; " + string.Join(' ', iface.Events.Select(ev => "_" + ev.RawName + " = null;"));
                hlw.Line($"public {iface.ClsName}({iface.RawName}* ptr) {{ Pointer = ptr;{eventInitializer} }}");
                hlw.Line($"public static implicit operator {iface.ClsName}({iface.RawName}* ptr) => new {iface.ClsName}(ptr);");
                hlw.Line($"public static explicit operator {iface.ClsName}(wl_proxy* ptr) => new {iface.ClsName}(({iface.RawName}*) ptr);");

                WriteRequests(iface, w, hlw);
                WriteEvents(iface, w, hlw);

                if (!iface.Requests.Any(m => m.RawName.Equals("destroy")))
                    hlw.Line("public void Destroy() { if (!IsNull) WaylandClient.wl_proxy_destroy((wl_proxy*) Pointer); }");

                hlw.CloseBlock();
            }

            w.CloseBlock(); // class {ProtocolName}Bindings*/

            w.Line();

            // dummy structs for typed pointers
            for (var i = 0; i < ifaces.Length; i++)
            {
                var iface = ifaces[i];
                WriteDescription(iface.Element, w);
                w.Line($"internal struct {iface.RawName} {{ public static unsafe wl_interface* Interface => &{_protocolClassName}.Interfaces[{i}]; }}");
            }

            w.Line();

            foreach (var iface in ifaces)
            {
                foreach (var e in iface.Enums)
                {
                    w.Line();
                    WriteEnum(iface, e, w);
                }
            }

            w.CloseBlock(); // namespace
            hlw.CloseBlock(); // namespace

            w.Line();
            w.Write(hlw);
        }

        private static void WriteRequests(Interface iface, CSharpWriter w, CSharpWriter hlw)
        {
            for (var i = 0; i < iface.Requests.Length; i++)
            {
                var req = iface.Requests[i];
                WriteRequest(iface, req, i, w, hlw);
                w.Line();
            }
        }

        private static void WriteEvents(Interface iface, CSharpWriter w, CSharpWriter hlw)
        {
            var events = iface.Events;
            foreach (var ev in events)
            {
                if (GenerateComments)
                {
                    WriteDescription(ev.Element, w);
                }
                w.Line(GetEventDelegate(iface, ev));
                w.Line();
            }

            if (events.Length > 0)
            {
                w.Line($"internal struct {iface.RawName}_listener");
                w.OpenBlock();
                foreach (var ev in events)
                    w.Line($"public IntPtr {ev.RawName};");

                w.Line();

                w.Line($"public static {iface.RawName}_listener* Alloc(");
                for (int i = 0; i < events.Length; i++)
                {
                    var ev = events[i];
                    if (i != 0) w.Line(",");
                    w.AppendIndented($"{GetDelegateName(iface, ev)} {ev.RawName}");
                }
                w.Line(")");
                w.OpenBlock();
                w.Line($"var ret = ({iface.RawName}_listener*) Marshal.AllocHGlobal(sizeof({iface.RawName}_listener));");
                w.Line($"Set(ret, {string.Join(",", events.Select(ev => ev.RawName))});");
                w.Line("return ret;");
                w.CloseBlock();
 
                w.Line();

                w.Line($"public static void Set({iface.RawName}_listener* listener");

                foreach (var ev in events)
                {
                    w.Line(",");
                    w.AppendIndented($"{GetDelegateName(iface, ev)} {ev.RawName}");
                }
                w.Line(")");
                w.OpenBlock();

                foreach (var ev in events)
                {
                    w.Append($"if ({ev.RawName} != null) ");
                    w.Line($"listener->{ev.RawName} = Marshal.GetFunctionPointerForDelegate<{GetDelegateName(iface, ev)}>({ev.RawName});");
                }
 
                w.CloseBlock();
                w.CloseBlock(); // struct listener

                w.Line();

                if (GenerateComments)
                {
                    w.DocSummary($"Set the callbacks for the given <see cref=\"{iface.RawName}\"/>.");
                    foreach (var ev in events)
                        WriteArgsDescription(ev.Element, w);
                }
                w.Line($"public static int {iface.RawName}_add_listener({iface.RawName}* iface, {iface.RawName}_listener* listener)");
                w.OpenBlock();
                w.Line("return WaylandClient.wl_proxy_add_listener((wl_proxy*) iface, listener, null);");
                w.CloseBlock();

                hlw.Append("public void SetListener(");
                for (var i = 0; i < events.Length; i++)
                {
                    var ev = events[i];
                    hlw.Line(i == 0 ? "" : ",");
                    hlw.AppendIndented($"{_protocolClassName}.{GetDelegateName(iface, ev)} {ev.RawName}");
                }
                hlw.Line(")");
                hlw.OpenBlock();

                foreach (var ev in events)
                    hlw.Line($"_{ev.RawName} = {ev.RawName};");

                hlw.Line($"_listener = {_protocolClassName}.{iface.RawName}_listener.Alloc({string.Join(", ", events.Select(e => e.RawName))});");
                hlw.Line($"{_protocolClassName}.{iface.RawName}_add_listener(Pointer, _listener);");

                hlw.CloseBlock();
                hlw.Line("public void FreeListener() { if (_listener != null) Marshal.FreeHGlobal((IntPtr) _listener); }");
            }
        }

        private static string GetEventDelegate(Interface iface, Message ev)
        {
            var sb = new StringBuilder($"public delegate void {GetDelegateName(iface, ev)}(void* data, {iface.RawName}* proxy");
            foreach (var a in ev.Arguments)
            {
                sb.Append(", ");
                if (a.IsEnumType)
                    sb.Append($"{iface.RawName}_{a.EnumType}");
                else if (a.Type == ArgType.String)
                    sb.Append("byte*"); // don't let .NET marshal to string
                else
                    sb.Append(a.ParamType);
                sb.Append(" ");
                if (Util.IsCSharpKeyword(a.Name))
                    sb.Append("@");
                sb.Append(a.Name);
            }
            sb.Append(");");
            return sb.ToString();
        }

        private static string GetDelegateName(Interface iface, Message ev)
        {
            return iface.RawName + '_' + ev.RawName + "_delegate";
        }

        private static void WriteRequest(Interface iface, Message r, int opcode, CSharpWriter w, CSharpWriter hlw)
        {
            WriteDescription(r.Element, w);
            WriteArgsDescription(r.Element, w);

            var parameters = new List<string>();
            var argsString = opcode.ToString();
            var args = new List<string>();

            var before = new List<string>();
            var after = new List<string>();

            var ifaceArg = string.Empty;

            var ret = "void";
            var hlwRet = "void";
            var newId = false;
            var generic = false;

            foreach (var arg in r.Arguments)
            {
                if (arg.Type == ArgType.New_id)
                {
                    newId = true;
                    if (arg.Interface == null)
                    {
                        parameters.Add("wl_interface* iface");
                        parameters.Add("uint version");
                        ifaceArg = ", iface";

                        args.Add("iface->Name");
                        args.Add("version");
                        ret = "wl_proxy*";
                        hlwRet = "T*";
                        generic = true;
                    }
                    else
                    {
                        ifaceArg = $", {arg.Interface}.Interface";
                        ret = arg.Interface + '*';
                        hlwRet = Util.ToPascalCase(arg.Interface); // use implicit cast e.g. wl_display* => WlDisplay
                    }

                    // we just pass 0 because we don't care about the id
                    // I didn't find this in the documentation, but the example
                    // in the wayland client lib passes 0 so that's fine I guess?
                    args.Add("0");
                }
                else if (arg.Type == ArgType.String)
                {
                    // TODO should this be in the low-level API?
                    before.Add($"var {arg.Name}ByteCount = System.Text.Encoding.UTF8.GetByteCount({arg.Name});");
                    before.Add($"var {arg.Name}Bytes = stackalloc byte[{arg.Name}ByteCount];");
                    before.Add($"Util.StringToUtf8({arg.Name}, {arg.Name}Bytes, {arg.Name}ByteCount);");
                    parameters.Add(arg.ParamType + " " + arg.Name);
                    args.Add(arg.Name + "Bytes");
                }
                else
                {
                    if (arg.IsEnumType)
                    {
                        // enum type can be with interface qualified or not; if not qualified it's an enum of this requests interface
                        var type = arg.EnumType.Contains('.') ? arg.EnumType.Replace('.', '_') : $"{iface.RawName}_{arg.EnumType}";
                        parameters.Add($"{type} {arg.Name}");
                        args.Add("(int) " + arg.Name);
                    }
                    else
                    {
                        parameters.Add(arg.ParamType + " " + arg.Name);
                        args.Add(arg.Name);
                    }
                }
            }

            var paramsString = parameters.Any() ? parameters.Aggregate((s1, s2) => s1 + ", " + s2) : string.Empty;
            var paramsString2 = paramsString == string.Empty ? string.Empty : ", " + paramsString;

            // replace unsafe pointers with managed variants
            var hlParams = parameters.Select(p => MakeManagedParam(p));
            var hlParamsString = hlParams.Any() ? hlParams.Aggregate((s1, s2) => s1 + ", " + s2) : string.Empty;
            // extract names from parameters
            var hlParamNames = hlParams.Select(p => p.Split(' '))
                .Select(ptv => (ptv[0].Equals("IntPtr") ? "(wl_interface*) " : "") + ptv[ptv.Length - 1] + (ptv.Length == 3 ? ".Pointer" : ""));
            var hlArgs = hlParamNames.Any() ? ", " + hlParamNames.Aggregate((s1, s2) => s1 + ", " + s2) : string.Empty;
            var genericStr = generic ? "<T>" : "";
            var genericConstrStr = generic ? " where T : unmanaged" : "";
            var castStr = generic ? "(T*) " : "";
            hlw.Append($"public {hlwRet} {r.NiceName}{genericStr}({hlParamsString}){genericConstrStr} => ");
            hlw.Line($"{castStr}{_protocolClassName}.{iface.RawName}_{r.RawName}(Pointer{hlArgs});");

            w.Line($"public static {ret} {iface.RawName}_{r.RawName}({iface.RawName}* pointer{paramsString2})");
            w.OpenBlock();
            var createReturn = $"return ({ret}) ptr;";

            foreach (var line in before)
                w.Line(line);

            var methodName = "WaylandClient.wl_proxy_marshal";

            var array = args.Count > 0 && (args.Count != 1 || newId);
            if (array)
            {
                methodName += "_array";
                w.Line($"var args = stackalloc wl_argument[{args.Count()}];");
                for (int i = 0; i < args.Count; i++)
                {
                    var arg = (string) args[i];
                    w.Line($"args[{i}] = {arg};");
                }
                argsString += ", args";
                argsString +=  ifaceArg;
            }
            else
            {
                argsString += ifaceArg;
                if (newId)
                    argsString += ", IntPtr.Zero";
            }

            if (newId)
                methodName += "_constructor";

            var newPtr = ret != "void" ? "var ptr = " : string.Empty;
            w.Line($"{newPtr}{methodName}((wl_proxy*) pointer, {argsString});");

            foreach (var line in after)
                w.Line(line);

            if (ret != "void")
                w.Line(createReturn);

            w.CloseBlock();
        }

        private static string MakeManagedParam(string p)
        {
            if (!p.Contains('*'))
                return p;
            var spl = p.Split(' ');
            var type = spl[0];
            var name = spl[1];
            if (type[type.Length - 1] != '*')
                throw new Exception("Unexpected parameter type.");
            if (type == "wl_interface*")
                return "IntPtr " + name;

            type = Util.ToPascalCase(type.Substring(0, type.Length - 1));
            return "in " + type + ' ' + name;
        }

        private static void WriteEnum(Interface iface, WlEnum e, CSharpWriter w)
        {
            if (GenerateComments)
                WriteDescription(e.Element, w);
            if (e.Bitfield)
                w.Line("[Flags]");
            w.Line($"internal enum {iface.RawName}_{e.Name}");
            w.OpenBlock();

            foreach (var entry in e.Entries)
            {
                if (GenerateComments)
                    w.DocSummary(entry.Summary);
                var name = Util.ToPascalCase(entry.Name);
                // number only names get a 'V' prepended
                if (name.All(c => char.IsDigit(c)))
                    name = "V" + name;
                w.Line($"{name} = {entry.Value},");
                if (GenerateComments)
                    w.Line();
            }

            w.CloseBlock();
        }

        private static void WriteDescription(XElement e, CSharpWriter w)
        {
            if (GenerateComments)
            {
                var summary = e.Attribute(SummaryAttrib);
                var descr = e.Element(DescriptionElement);
                if (descr != null)
                {
                    var descrStr = descr.Value;
                    var summaryStr = summary == null ? string.Empty : summary.Value + "\n";
                    descrStr = $"{summaryStr}<p>" + descrStr + "</p>";
                    var lines = descrStr
                        .Trim()
                        .Split('\n')
                        .SelectMany(l => string.IsNullOrEmpty(l) ? new [] {"</p>", "<p>"} : new [] {l})
                        .Select(s => s.Trim());
                    w.DocSummary(lines);
                }
            }
        }

        private static void WriteArgsDescription(XElement request, CSharpWriter w)
        {
            if (GenerateComments)
            {
                var args = request.Elements(ArgElement);
                foreach (var a in args)
                {
                    var name = a.Attribute(NameAttrib)?.Value;
                    var descr = a.Attribute(SummaryAttrib)?.Value;
                    if (descr != null)
                        w.Line($"/// <param name=\"{name}\">{descr}</param>");
                }
            }
        }
    }
}
