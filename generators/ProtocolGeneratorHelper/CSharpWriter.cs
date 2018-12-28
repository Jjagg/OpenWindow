using System.Collections.Generic;
using System.Text;

namespace ProtocolGeneratorHelper
{
    public class CSharpWriter
    {
        private int _indentation;
        private readonly StringBuilder _sb;
        private bool _newline = true;

        public CSharpWriter()
        {
            _sb = new StringBuilder();
        }

        public void Line()
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

        public void Write(CSharpWriter embedded)
        {
            var text = embedded.ToString();
            var start = 0;
            int end = -1;
            while ((end = text.IndexOf('\n', start)) != -1)
            {
                if (end == start)
                    Line();
                else
                    Line(text.Substring(start, end - start));
                start = end + 1;
            }

            Append(text.Substring(start));
        }

        public void DocSummary(string summary)
        {
            AppendLine("/// <summary>");
            if (!string.IsNullOrEmpty(summary))
            {
                foreach (var line in summary.Split('\n'))
                    AppendLine("/// " + line);
            }
            AppendLine("/// </summary>");
        }

        public void DocSummary(IEnumerable<string> lines)
        {
            AppendLine("/// <summary>");
            foreach (var line in lines)
                AppendLine("/// " + line);
            AppendLine("/// </summary>");
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

        public void Append(string text)
        {
            if (_newline)
                text = Indent(text);
            _sb.Append(text);
            _newline = false;
        }

        public void AppendIndented(string text)
        {
            Indent();
            Append(text);
            Dedent();
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
            return string.IsNullOrWhiteSpace(text) ? text : string.Empty.PadLeft(_indentation * 4, ' ') + text;
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
    }
}
