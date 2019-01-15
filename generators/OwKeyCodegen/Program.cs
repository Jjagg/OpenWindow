using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace OwKeyCodegen
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Expected OwKeyCodegen <enum name>");
                Console.WriteLine("                      validate <code file>");
                return;
            }

            var validate = args.Length == 2 && args[0].Equals("validate");

            var lines = File.ReadAllLines("keys.txt");
            var enumEntries = new List<(string Name, int Value)>();
            var nextValue = 0;

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    enumEntries.Add((string.Empty, -1));
                    continue;
                }

                var entries = line.Split('/');
                foreach (var e in entries)
                {
                    enumEntries.Add((e, nextValue));
                }

                nextValue++;
            }

            var valueCount = nextValue;

            if (validate)
            {
                var scanLines = File.ReadAllLines(args[1]);
                var scanEntries = new List<int>();

                for (var j = 0; j < scanLines.Length; j++)
                {
                    var sl = scanLines[j];
                    var commaIndex = sl.IndexOf(',');
                    if (commaIndex == -1)
                        continue;

                    var name = sl.Substring(0, commaIndex).Trim();
                    if (enumEntries.Any(e => e.Name.Equals(name)))
                    {
                        var entry = enumEntries.First(e => e.Name.Equals(name));
                        scanEntries.Add(entry.Value);
                    }
                }

                var distinct = enumEntries
                    .GroupBy(e => e.Value)
                    .Select(e => e.First())
                    .Where(e => !string.IsNullOrEmpty(e.Name))
                    .ToArray();
                var present = distinct.Where(e => scanEntries.Contains(e.Value)).ToArray();
                var missing = distinct.Except(present).ToArray();
                Console.WriteLine($"Present entries ({present.Length}):");
                Console.WriteLine($"Missing entries ({missing.Length}):");
                foreach (var me in missing)
                    Console.WriteLine("  - " + me.Name);
            }
            else
            {
                Console.WriteLine("// This file is generated, do not edit it directly");

                Console.WriteLine();
                Console.WriteLine("namespace OpenWindow");
                Console.WriteLine("{");

                Console.WriteLine("    public enum " + args[0]);
                Console.WriteLine("    {");

                foreach (var e in enumEntries)
                {
                    if (string.IsNullOrWhiteSpace(e.Name))
                    {
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("        " + e.Name + " = " + e.Value + ",");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("        Count = " + valueCount);

                Console.WriteLine("    }");
                Console.WriteLine("}");
            }
        }

    }
}
