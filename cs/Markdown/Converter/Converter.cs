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
                switch (token.Type)
                {
                    case TokenType.Text:
                        convertedText.Append(inputText.Substring(token.Start, token.Length));
                        break;
                    case TokenType.Tag:
                        convertedText.Append(GetConvertedSubTag(token));
                        break;
                }
            }

            return convertedText.ToString();
        }

        private string GetConvertedSubTag(TypedToken tagToken)
        {
            return tagToken.Type != TokenType.Tag ? "" : targetTags.GetSubTag(tagToken.TagType, tagToken.Order);
        }
    }
}
