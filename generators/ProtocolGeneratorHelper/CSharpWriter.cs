using System.Collections.Generic;
using System.Text;

namespace ProtocolGeneratorHelper
{
    public class CSharpWriter
    {
        private int _indentation;
        private readonly StringBuilder _sb;
        private bool _newline = true;

        public AccessModifier DefaultAm { get; set; }
        public bool DefaultStatic { get; set; }
        public bool DefaultAbstract { get; set; }
        public bool DefaultExtern { get; set; }

        public CSharpWriter()
        {
            _sb = new StringBuilder();
        }

        public void NewLine()
        {
            _sb.AppendLine();
            _newline = true;
        }

        public void Line(string text)
        {
            AppendLine(text);
        }

        public void LineIndented(string text)
        {
            Indent();
            Line(text);
            Dedent();
        }
        
        public void LineComment(string l)
        {
            AppendLine($"// {l}");
        }

        public void BlockComment(string t)
        {
            var lines = t.Split('\n');
            AppendLine("/*");
            foreach (var l in lines)
                AppendLine(l);
            AppendLine("*/");
        }

        public void Using(string u)
        {
            AppendLine($"using {u};");
        }

        public void BeginNs(string ns)
        {
            AppendLine($"namespace {ns}");
            OpenBlock();
        }

        public void BeginClass(string name, AccessModifier? am = null,
            bool? sttic = null, bool? ptial = null)
        {
            var ams = ArgAm(am);
            var sts = ArgStatic(sttic);
            var pts = (ptial ?? false) ? "partial " : "";
            AppendLine($"{ams} {sts}{pts}class {name}");
            OpenBlock();
        }

        public void BeginEnum(string name, AccessModifier? am = null)
        {
            var ams = ArgAm(am);
            AppendLine($"{ams} enum {name}");
            OpenBlock();
        }

        public void Constant(string type, string name, string value, AccessModifier? am = null)
        {
            var ams = ArgAm(am);
            AppendLine($"{ams} const {type} {name} = {value};");
        }

        public void Field(string type, string name, bool? sttic = null, AccessModifier? am = null, string init = null)
        {
            var ams = ArgAm(am);
            var sts = ArgStatic(sttic);
            var ins = init == null ? string.Empty : $" = {init}";
            AppendLine($"{ams} {sts}{type} {name}{ins};");
        }

        public void BeginMethod(string name, string ret = null, string ps = null,
            bool? sttic = null, AccessModifier? am = null)
        {
            var ams = ArgAm(am);
            var sts = ArgStatic(sttic);
            var res = ret ?? "void";
            AppendLine($"{ams} {sts}{res} {name}({ps ?? string.Empty})");
            OpenBlock();
        }

        public void DocSummary(string summary)
        {
            AppendLine("/// <summary>");
            if (!string.IsNullOrEmpty(summary))
            {
                foreach (var line in summary.Split('\n'))
                    DocComment(line);
            }
            AppendLine("/// </summary>");
        }

        public void DocSummary(IEnumerable<string> lines)
        {
            AppendLine("/// <summary>");
            foreach (var line in lines)
                DocComment(line);
            AppendLine("/// </summary>");
        }

        public void DocParam(string name, string descr)
        {
            Append($"/// <param name=\"{name}\">");
            Append(descr);
            Append("</param>");
            NewLine();
        }

        public void DocComment(string l)
        {
            AppendLine($"/// {l}");
        }

        public void OpenBlock()
        {
            AppendLine("{");
            Indent();
        }

        public void CloseBlock()
        {
            Dedent();
            AppendLine("}");
        }

        public override string ToString()
        {
            return _sb.ToString();
        }

        private void Append(string text)
        {
            if (_newline)
                text = Indent(text);
            _sb.Append(text);
            _newline = false;
        }

        private void AppendLine(string text)
        {
            if (_newline)
                text = Indent(text);
            _sb.AppendLine(text);
            _newline = true;
        }

        private string Indent(string text)
        {
            return string.Empty.PadLeft(_indentation * 4, ' ') + text;
        }
        
        public void Indent()
        {
            _indentation++;
        }

        public void Dedent()
        {
            _indentation--;
            if (_indentation < 0)
                _indentation = 0;
        }

        private string ArgAm(AccessModifier? am)
        {
            return (am ?? DefaultAm).ToString().ToLowerInvariant();
        }

        private string ArgStatic(bool? sttic)
        {
            return (sttic ?? DefaultStatic) ? "static " : string.Empty;
        }

        private string ArgAbstract(bool? abstrct)
        {
            return (abstrct ?? DefaultAbstract) ? "abstract " : string.Empty;
        }

        private string ArgExtern(bool? ext)
        {
            return (ext ?? DefaultExtern) ? "extern " : string.Empty;
        }
    }
}
