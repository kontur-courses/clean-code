using Markdown.Tokens;

namespace Markdown
{
    internal class HTMLConverter : ITextConverter
    {
        public string Convert(Token token, ref string source)
        {
            var htmlTag = GetHtmlTag(token);
            string result = $"<{htmlTag}>";
            if (token.Children.Count > 0)
            {
                foreach (var child in token.Children)
                    result += Convert(child, ref source);
            }
            else
                result += source.Substring(token.BeginIndex, token.EndIndex - token.BeginIndex);
            result += $"<{htmlTag}/>";

            return result;
        }

        private string GetHtmlTag(Token token)
        {
            switch (token.GetType().Name)
            {
                case nameof(ItalicStyle):
                    return "em";
                default:
                    return string.Empty;
            }
        }
    }
}
