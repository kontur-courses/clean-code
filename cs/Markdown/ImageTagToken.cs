namespace Markdown
{
    public class ImageTagToken : ITagToken
    {
        public string Convert(Token token)
        {
            var altText = "";
            var url = "";

            for (var i = 0; i < token.Value.Length; ++i)
            {
                if (token.Value[i] == ']' && token.Value[i - 1] != '[')
                    altText = token.Value[2..i];

                if (token.Value[i] == ')' && token.Value[i - 1] != '(')
                    url = token.Value[(altText.Length + 4)..^1];
            }

            return $"<img src=\"{url}\" alt=\"{altText}\">";
        }
    }
}