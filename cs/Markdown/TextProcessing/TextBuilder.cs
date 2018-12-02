using System.Collections.Generic;
using System.Text;
using Markdown.MarkdownConfigurations;
using Markdown.TokenEssences;

namespace Markdown.TextProcessing
{
    public class TextBuilder
    {
        public IConfig MdConfiguration { get; }

        public TextBuilder(IConfig mdConfiguration)
        {
            MdConfiguration = mdConfiguration;
        }

        public string BuildText(List<IToken> tokens)
        {
            var strBuilder = new StringBuilder();
            foreach (var token in tokens)
            {
                    strBuilder.Append(MdConfiguration.TokenExtractor[token.TypeToken].StartToken);
                    strBuilder.Append(token.Value);
                    strBuilder.Append(MdConfiguration.TokenExtractor[token.TypeToken].EndOfToken);
            }
            return strBuilder.ToString();
        }

        public string BuildText(List<string> paragraphs)
        {
            var strBuilder = new StringBuilder();
            for (int i = 0; i < paragraphs.Count; i++)
            {
                strBuilder.Append(paragraphs[i]);
                if (i != paragraphs.Count - 1)
                    strBuilder.Append("\r\n\r\n");
            }
            return strBuilder.ToString();
        }

        public string BuildTokenValue(IToken token)
        {
            if (token.Value.Length == 0) return "";
            return MdConfiguration.TokenExtractor[token.TypeToken].StartToken +
                 token.Value + MdConfiguration.TokenExtractor[token.TypeToken].EndOfToken;
    }
}
}