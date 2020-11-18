using System.Text;

namespace Markdown
{
    public class TagTokenConverter : ITagTokenConverter

    {
        protected string OpenTag;
        protected string CloseTag;

        public string ConvertToken(TextToken textToken)
        {
            if (textToken.SubTokens == null)
                return textToken.Text;
            var converterGetter = new TokenConverterFactory();
            var htmlText = new StringBuilder();
            htmlText.Append(OpenTag);
            foreach (var token in textToken.SubTokens)
            {
                var tokenConverter = converterGetter.GetTokenConverter(token.Type);
                htmlText.Append(tokenConverter.ConvertToken(token));
            }

            htmlText.Append(CloseTag);
            return htmlText.ToString();
        }
    }
}