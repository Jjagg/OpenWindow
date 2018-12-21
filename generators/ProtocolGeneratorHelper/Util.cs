using System.Linq;
using System.Text;

namespace ProtocolGeneratorHelper
{
    public static class Util
    {
        public static string ToPascalCase(string text, bool upperCaseStart = true)
        {
            if (text == null)
                return null;

            var uppercaseChars = new [] {'_', '.'};
            var skipChars = new[] {'_'};
            var upperCase = upperCaseStart;
            var sb = new StringBuilder();
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (!skipChars.Contains(c))
                    sb.Append(upperCase ? char.ToUpperInvariant(c) : c);

                upperCase = uppercaseChars.Contains(c);
            }
            return sb.ToString();
        }

        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/
        private static readonly string[] Keywords = {
            "abstract", "as", "base", "bool",
            "break", "byte", "case", "catch",
            "char", "checked", "class", "const",
            "continue", "decimal", "default", "delegate",
            "do", "double", "else", "enum",
            "event", "explicit", "extern", "false",
            "finally", "fixed", "float", "for",
            "foreach", "goto", "if", "implicit",
            "in", "int", "interface",
            "internal", "is", "lock", "long",
            "namespace", "new", "null", "object",
            "operator ", "out", "override",
            "params", "private", "protected", "public",
            "readonly", "ref", "return", "sbyte",
            "sealed", "short", "sizeof", "stackalloc",
            "static", "string", "struct", "switch",
            "this", "throw", "true", "try",
            "typeof", "uint", "ulong", "unchecked",
            "unsafe", "ushort", "using", "using", "static",
            "virtual", "void", "volatile", "while"
        };

        public static bool IsCSharpKeyword(string identifier)
        {
            return Keywords.Contains(identifier);
        }
    }
}
