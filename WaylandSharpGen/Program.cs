using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace WaylandSharpGen
{
    /// <summary>
    /// This program generates a file with a C# implementation
    /// of a Wayland protocol client.
    /// </summary>
    internal class Program
    {
        private static readonly string DefaultProtocolPath = "/usr/share/wayland/wayland.xml";
        private static readonly string DefaultOutputPath = Path.Combine("generated", "WaylandBindings.cs");

        private static int Main(string[] args)
        {
            if (args.Length > 0 && (args[0] == "-h" || args[0] == "--help"))
            {
                Console.WriteLine("usage:  dotnet WaylandSharpGen.dll [protocol] [output]");
                return 0;
            }

            if (args.Length == 0)
                Console.WriteLine("No protocol path specified, using default path.");

            var path = args.Length > 0 ? args[0] : DefaultProtocolPath;
            Console.WriteLine($"Using protocol at '{path}'.");
            if (!File.Exists(path)) {
                Console.WriteLine("Protocol file not found. Exiting...");
                return 1;
            }

            var doc = XDocument.Load(path);
            var w = new CSharpWriter
            {
                DefaultAm = AccessModifier.Public,
                DefaultStatic = false
            };

            w.LineComment("This file was generated from an xml Wayland protocol specification");
            w.LineComment("by WaylandSharpGen. https://github.com/Jjagg/WaylandSharpGen");
            w.NewLine();
            ParseProtocol(doc, w);

            var fp = args.Length > 1 ? args[1] : DefaultOutputPath;
            Directory.CreateDirectory(Path.GetDirectoryName(fp));
            File.WriteAllText(fp, w.ToString());
            Console.WriteLine($"Wrote output to '{fp}'.");
            return 0;
        }

        private static readonly bool GenerateComments = true;
        private static readonly bool GenerateRegion = true;

        private const string ProtocolElement = "protocol";
        private const string DescriptionElement = "description";
        private const string InterfaceElement = "interface";
        private const string ArgElement = "arg";

        private const string NameAttrib = "name";
        private const string SummaryAttrib = "summary";

        private const string MessageCodeSuffix = "Msg";
        private const string OpCodeSuffix = "Op";

        private static void ParseProtocol(XContainer doc, CSharpWriter w)
        {
            var protocolElement = doc.Element(ProtocolElement);
            if (protocolElement == null)
                throw new Exception("No protocol element found!");

            w.Using("System");
            w.Using("System.Runtime.InteropServices");
            w.Using("SMarshal = System.Runtime.InteropServices.Marshal");
            w.NewLine();
            w.BeginNs("OpenWindow.Backends.Wayland");

            var interfaceElements = protocolElement.Elements(InterfaceElement);
            var ifaces = interfaceElements.Select(e => new Interface(e)).ToArray();

            w.BeginClass("WlInterfaces", AccessModifier.Internal, true, true);

            w.BeginMethod("CleanUp", null, null, true, AccessModifier.Public);

            foreach (var iface in ifaces)
                w.Line($"{iface.ClsName}.CleanUp();");

            w.CloseBlock();

            w.CloseBlock(); // WlInterfaces*/

            foreach (var iface in ifaces)
            {
                w.NewLine();
                WriteInterface(iface, w);
            }
            w.CloseBlock();
        }

        private static void WriteInterface(Interface iface, CSharpWriter w)
        {
            ParseDescription(iface.Element, w);

            // TODO when is something a proxy?
            var baseClass = iface.Requests.Any() ? "WlProxy" : "WlObject";
            w.BeginClass(iface.ClsName + " : " + baseClass, AccessModifier.Internal, false, true);

            if (GenerateRegion)
            {
                w.Line("#region Opcodes");
                w.NewLine();
            }
            // add the opcodes
            for (var i = 0; i < iface.Requests.Length; i++)
            {
                var r = iface.Requests[i];
                var rname = r.NiceName;
                w.Constant("int", rname + OpCodeSuffix, i.ToString(), AccessModifier.Private);
            }

            if (GenerateRegion)
            {
                w.NewLine();
                w.Line("#endregion");
            }

            w.NewLine();

            if (GenerateRegion)
            {
                w.Line("#region Interface");
                w.NewLine();
            }

            var requestCount = iface.Requests.Length;
            var evCount = iface.Events.Length;
            w.Field("WlInterface", "Interface", true, AccessModifier.Public,
                $"new WlInterface(\"{iface.RawName}\", {iface.Version}, {requestCount}, {evCount})");
            w.Line($"public const string InterfaceName = \"{iface.RawName}\";");

            w.NewLine();

            foreach (var r in iface.Requests)
                w.Field("readonly WlMessage", r.NiceName + MessageCodeSuffix, true, AccessModifier.Private,
                    r.Initializer());

            w.NewLine();

            w.Line($"static {iface.ClsName}()");
            w.OpenBlock();

            var requestsStr = iface.Requests.Any()
                ? "new [] {" + iface.Requests.Select(r => r.NiceName + MessageCodeSuffix)
                      .Aggregate((s1, s2) => s1 + ", " + s2) + "}"
                : "new WlMessage[0]";
            w.Line($"Interface.SetRequests({requestsStr});");
            w.Line("Interface.Finish();");

            w.CloseBlock();

            w.NewLine();
            w.BeginMethod("CleanUp", null, null, true, AccessModifier.Public);

            foreach (var request in iface.Requests)
                w.Line($"{request.NiceName + MessageCodeSuffix}.Dispose();");
            //foreach (var ev in iface.Events)
            //w.Line($"{ev.NiceName}.Dispose();");

            w.Line("Interface.Dispose();");

            w.CloseBlock();

            if (GenerateRegion)
            {
                w.NewLine();
                w.Line("#endregion");
            }

            w.NewLine();

            w.Line($"public {iface.ClsName}(IntPtr pointer)");
            w.Line("    : base(pointer) { }");

            if (GenerateRegion)
            {
                w.NewLine();
                w.Line("#region Events");
            }

            w.NewLine();
            if (iface.Events.Any())
                WriteEvents(iface.Events, w);

            if (GenerateRegion)
            {
                w.NewLine();
                w.Line("#endregion");
                w.NewLine();
                w.Line("#region Requests");
            }

            foreach (var r in iface.Requests)
            {
                w.NewLine();
                WriteRequest(r, w);
            }

            if (GenerateRegion)
            {
                w.NewLine();
                w.Line("#endregion");
            }

            w.CloseBlock();
        }

        private static void WriteEvents(Message[] events, CSharpWriter w)
        {
            foreach (var e in events)
                w.Line(e.EventDelegate());
            w.NewLine();
            
            w.Line($"private IntPtr _listener = SMarshal.AllocHGlobal(IntPtr.Size * {events.Length});");
            w.Line("private bool _setListener;");

            w.NewLine();
            foreach (var e in events)
            {
                ParseDescription(e.Element, w);
                ParseArgsDescription(e.Element, w);
                w.Line($"public {e.NiceName}Handler {e.NiceName};");
            }
            
            w.NewLine();
            w.Line("public void SetListener()");
            w.OpenBlock();
            w.Line("if (_setListener)");
            w.Line("    throw new Exception(\"Listener already set.\");");
            for (var i = 0; i < events.Length; i++)
            {
                var e = events[i];
                w.Line($"SMarshal.WriteIntPtr(_listener, {i} * IntPtr.Size, SMarshal.GetFunctionPointerForDelegate({e.NiceName}));");
            }
            w.Line("AddListener(Pointer, _listener, IntPtr.Zero);");
            w.Line("_setListener = true;");
            w.CloseBlock();
            
            w.NewLine();
            w.Line("[DllImport(\"libwayland-client.so\", EntryPoint = \"wl_proxy_add_listener\")]");
            w.Line("private static extern int AddListener(IntPtr proxy, IntPtr listener, IntPtr data);");
        }

        private static void WriteRequest(Message r, CSharpWriter w)
        {
            ParseDescription(r.Element, w);
            ParseArgsDescription(r.Element, w);

            var ps = new List<string>();
            var argsString = $"Pointer, {r.NiceName + OpCodeSuffix}";
            var args = new List<string>();

            var ifaceArg = string.Empty;

            var ret = "void";
            var newId = false;
            var generic = false;
            foreach (var arg in r.Arguments)
            {
                if (arg.Type == ArgType.New_id)
                {
                    newId = true;
                    if (arg.Interface == null)
                    {
                        ps.Add("WlInterface iface");
                        ifaceArg = ", iface.Pointer";
                        ret = "T";
                        generic = true;
                    }
                    else
                    {
                        ifaceArg = $", {arg.InterfaceCls}.Interface.Pointer";
                        ret = arg.InterfaceCls;
                    }
                }
                else
                {
                    if (arg.Type == ArgType.Object)
                        args.Add(arg.InterfaceCls + ".Interface.Pointer");
                    else
                        args.Add(arg.Name);

                    ps.Add(arg.ParamType + " " + arg.Name);
                }
            }

            var paramsString = ps.Any() ? ps.Aggregate((s1, s2) => s1 + ", " + s2) : string.Empty;
            string createReturn;
            if (generic)
            {
                w.Line($"public T {r.NiceName}<T>({paramsString})");
                w.Line("    where T : WlObject");
                w.OpenBlock();
                createReturn = "return (T) Activator.CreateInstance(typeof(T), new [] { ptr });";
            }
            else
            {
                w.BeginMethod(r.NiceName, ret, paramsString);
                createReturn = $"return new {ret}(ptr);";
            }

            var methodName = "Marshal";

            var array = args.Count > 0 && (args.Count != 1 || newId);
            if (array)
            {
                methodName += "Array";
                var varArgs = args.Aggregate((s1, s2) => s1 + ", " + s2);
                w.Line($"var args = new ArgumentList({varArgs});");
                argsString += ", args.Pointer" + ifaceArg;
            }
            else
            {
                argsString += ifaceArg;
                if (newId)
                    argsString += ", IntPtr.Zero";
            }

            if (newId)
                methodName += "Constructor";

            var newPtr = ret != "void" ? "var ptr = " : string.Empty;
            w.Line($"{newPtr}{methodName}({argsString});");
            if (array)
                w.Line("args.Dispose();");
            if (ret != "void")
                w.Line(createReturn);

            w.CloseBlock();
        }

        private static void ParseDescription(XElement e, CSharpWriter w)
        {
            if (GenerateComments)
            {
                var descr = e.Element(DescriptionElement);
                if (descr != null)
                {
                    var lines = descr.Value
                        .Trim()
                        .Split('\n')
                        .Select(s => s.Trim());
                    w.DocSummary(lines);
                }
            }
        }

        private static void ParseArgsDescription(XElement request, CSharpWriter w)
        {
            if (GenerateComments)
            {
                var args = request.Elements(ArgElement);
                foreach (var a in args)
                {
                    var name = a.Attribute(NameAttrib);
                    var descr = a.Attribute(SummaryAttrib);
                    if (descr != null)
                        w.DocParam(name.Value, descr.Value);
                }
            }
        }
    }
}
