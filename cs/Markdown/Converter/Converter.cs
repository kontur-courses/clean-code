using System.Text;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Converter
{
    public class Converter : IConverter
    {
        private readonly TagStorage targetTags;

        private readonly TokenReader tokenReader;

        public Converter(TagStorage sourceTags, TagStorage targetTags)
        {
            tokenReader = new TokenReader(sourceTags);
            this.targetTags = targetTags;
        }

        public string Convert(string inputText)
        {
            var convertedText = new StringBuilder();

            // операции с коллекцией токенов, полученной из TokenReader'a
            // замена исходных тэгов на целевые

            return convertedText.ToString();
        }
    }
}
