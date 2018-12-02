using System.Collections.Generic;
using Markdown.TokenEssences;

namespace Markdown.MarkdownConfigurations
{
    public interface IConfig
    {
        Dictionary<TypeToken,(string StartToken, string EndOfToken)> TokenExtractor { get; }
    }
}