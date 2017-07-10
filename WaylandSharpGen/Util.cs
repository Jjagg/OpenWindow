using System.Text;

namespace WaylandSharpGen
{
    public class Util
    {
        public static string ToPascalCase(string text)
        {
            if (text == null)
                return null;

            var sb = new StringBuilder();
            sb.Append(char.ToUpperInvariant(text[0]));
            for (var i = 1; i < text.Length; i++)
            {
                if (text[i] == '_')
                {
                    if (i + 1 >= text.Length)
                        break;
                    sb.Append(char.ToUpperInvariant(text[i + 1]));
                    i++;
                }
                else
                    sb.Append(text[i]);
            }
            return sb.ToString();
        }
    
    }
}