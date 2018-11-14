using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleApplication1.Parsers;

namespace ConsoleApplication1.Extensions
{
    public static class TextParserExtensions
    {
        public static List<TextPart> GetAllParts(this TextParser parser)
        {
            var textParts = new List<TextPart>();

            while (parser.AnyParts())
                textParts.Add(parser.GetNextPart());

            return textParts;
        }

        public static string GetText(this TextParser parser)
        {
            var text = new StringBuilder();
            foreach (var textPart in parser.GetAllParts().Select(x => x.Text))
                text.Append(textPart);
            return text.ToString();
        }
    }
}
