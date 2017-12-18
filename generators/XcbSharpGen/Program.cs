// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ProtocolGeneratorHelper;

namespace XcbSharpGen
{
    /// <summary>
    /// </summary>
    internal class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "-h" || args[0] == "--help")
            {
                Console.WriteLine("usage:  dotnet XcbSharpGen.dll [output] [protocol]*");
                return 0;
            }

            if (args.Length <= 0)
            {
                Console.WriteLine("Need at least an output file and 1 header file.");
                Console.WriteLine("usage:  dotnet XcbSharpGen.dll [output] [protocol]*");
                return 0;
            }

            var headerFiles = args.Skip(1).Select(Path.GetFullPath);
            var w = new CSharpWriter
            {
                DefaultAm = AccessModifier.Public,
                DefaultStatic = false
            };

            w.Using("System");
            w.Using("System.Runtime.InteropServices");
            w.Using("SMarshal = System.Runtime.InteropServices.Marshal");
            w.NewLine();
            w.BeginNs("OpenWindow.Backends.Wayland");

            w.LineComment("This file was generated from xml XCB header specifications");
            w.LineComment("by XcbSharpGen. https://github.com/Jjagg/OpenWindow/tree/master/generators/XcbSharpGen");
            w.NewLine();

            var importedHeaders = new List<string>();
            foreach (var path in headerFiles)
            {
                Console.WriteLine($"Using protocol at '{path}'.");
                if (!File.Exists(path))
                {
                    Console.WriteLine("Protocol file not found. Exiting...");
                    return 1;
                }

                var doc = XDocument.Load(path);
                ParseHeader(doc, w, importedHeaders);
            }

            var fp = args[0];
            Directory.CreateDirectory(Path.GetDirectoryName(fp));
            File.WriteAllText(fp, w.ToString());
            Console.WriteLine($"Wrote output to '{fp}'.");
            return 0;
        }

        private static readonly bool GenerateComments = true;
        private static readonly bool GenerateRegion = true;

        private const string XcbElement = "xcb";

        private static void ParseHeader(XContainer doc, CSharpWriter w, List<string> importedHeaders)
        {
            var xcbElement = doc.Element(XcbElement);
            if (xcbElement == null)
                throw new Exception("No xcb element found!");
            
        }
    }
}
