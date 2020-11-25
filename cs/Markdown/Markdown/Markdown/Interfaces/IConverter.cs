using System.Collections.Generic;

namespace Markdown
{
    public interface IConverter
    {
        public List<TokenMd> GetTokens();

        public List<TokenMd> FormatToken(List<TokenMd> tokens);

        public string GetHTML(List<TokenMd> tokens);
    }
}