using System.Collections.Generic;
using Markdown.TokenEssences;

namespace Markdown.MarkdownConfigurations
{
    public class HtmlConfig : IConfig
    {
        public Dictionary<TypeToken, (string StartToken, string EndOfToken)> TokenExtractor { get;  }

        public HtmlConfig()
        {
            TokenExtractor = new Dictionary<TypeToken, (string, string)>
            {
                {TypeToken.Simple, ("", "")},
                {TypeToken.Em,("<em>", "</em>")},
                {TypeToken.Strong,("<strong>", "</strong>")}
            };
        }
    }
}