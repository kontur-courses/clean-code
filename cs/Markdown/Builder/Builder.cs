using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Builder : IBuilder
    {
        public string Build(IEnumerable<IToken> tokens)
        {
            var stringBuilder = new StringBuilder();
            var prevToken = default(IToken);
            foreach (var token in tokens)
            {
                var content = token.Content;
                if (prevToken != null && prevToken.TagState == TagState.SelfClosing)
                    content = token.Content.TrimStart(' ');

                stringBuilder.Append(content);
                prevToken = token;
            }
            return stringBuilder.ToString();
        }
    }
}
