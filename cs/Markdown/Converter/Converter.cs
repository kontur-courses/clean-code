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

            var tokens = tokenReader.Read(inputText);

            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Text)
                    convertedText.Append(inputText.Substring(token.Start, token.Length));

                if (token.Type == TokenType.Tag)
                    convertedText.Append(GetConvertedSubTag(token));
            }

            return convertedText.ToString();
        }

        private string GetConvertedSubTag(TypedToken tagToken)
        {
            if (tagToken.Type != TokenType.Tag)
                return "";

            return targetTags.GetSubTag(tagToken.TagType, tagToken.Order);
        }
    }
}
