// MIT License - Copyright (C) Jesse "Jjagg" Gielen
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Linq;
using System.Xml.Linq;

namespace WaylandSharpGen
{
    public class WlEnum
    {
        private const string EntryElement = "entry";
        private const string NameAttrib = "name";
        private const string BitfieldAttrib = "bitfield";
        
        public readonly string Name;
        public readonly bool Bitfield;
        public readonly Entry[] Entries;

        public readonly XElement Element;

        public WlEnum(XElement element)
        {
            Element = element;

            Name = element.Attribute(NameAttrib).Value;
            var bitfield = element.Attribute(BitfieldAttrib);
            Bitfield = bitfield == null ? false : bool.Parse(bitfield.Value);
            Entries = element.Elements(EntryElement).Select(e => new Entry(e)).ToArray();
        }

        public class Entry
        {
            private const string SummaryAttrib = "summary";
            private const string ValueAttrib = "value";
            
            public readonly string Name;
            public readonly string Value;
            public readonly string Summary;

            public Entry(XElement element)
            {
                Name = element.Attribute(NameAttrib).Value;
                Value = element.Attribute(ValueAttrib).Value;
                Summary = element.Attribute(SummaryAttrib).Value;
            }
        }
    }
}