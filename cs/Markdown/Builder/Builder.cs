using Markdown.TokenNamespace;
using System.Collections.Generic;
using System.Text;

namespace Markdown.BuilderNamespace
{
    public class Builder : IBuilder
    {
        public string Build(IEnumerable<IToken> tokens)
        {
            var stringBuilder = new StringBuilder();
            foreach (var token in tokens)
            {
                stringBuilder.Append(token.Content);
            }
            return stringBuilder.ToString();
        }
    }
}
