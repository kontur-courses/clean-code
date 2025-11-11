using System.Text;
using Markdown.Dto;
using Markdown.Extensions;
using Markdown.TagUtils.Abstractions;
using Markdown.TagUtils.Implementations;
using Markdown.TokensUtils;
using Markdown.TokensUtils.Abstractions;
using Markdown.TokensUtils.Implementations;

namespace Markdown
{
    public class MarkdownRender(ITokenizer tokenizer, ITagProcessor tagProcessor) : IRender
    {
        public string RenderText(string markdown)
        {
            var markDownLines = markdown.EnumerateLines();
            var result = new StringBuilder();
            foreach (var line in markDownLines)
            {
                if (result.Length > 0) result.Append('\n');
                result.Append(RenderLine(line));
            }

            return result.ToString();
        }

        private string RenderLine(string line)
        {
            var tokens = tokenizer.Tokenize(line).ToList();
            return tagProcessor.Process(tokens);
        }
    }
}