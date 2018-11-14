using System.Text;
using ConsoleApplication1.Parsers;

namespace ConsoleApplication1.Extensions
{
    public static class StringReaderExtensions
    {
        public static string ReadToEnd(this StringReader reader)
        {
            var builder = new StringBuilder();

            while (reader.AnySymbols())
                builder.Append(reader.ReadNextSymbol());

            return builder.ToString();
        }
    }
}
