using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    class HtmlWriter
    {
        private readonly List<Token> tokens;
        private readonly string mdParagraph;

        public HtmlWriter(string mdParagraph, List<Token> tokens)
        {
            this.tokens = tokens;
            this.mdParagraph = mdParagraph;
        }

        public string GetHtmlParagraph()
        {
            if (tokens.Count == 0)
                return GetLineWithoutEscaping(mdParagraph.Replace(@"\", ""));

            var result = new StringBuilder();
            var position = 0;
            var currentTagId = 0;
            var htmlTagsList = GetHtmlTagsList();

            while (position < mdParagraph.Length)
            {
                if (currentTagId < htmlTagsList.Count)
                {
                    var currentTag = htmlTagsList[currentTagId];
                    var shift = currentTag.IsClose ? 0 : currentTag.Md.Length;
                    result.Append(
                        mdParagraph.Substring(
                            position, currentTag.Position - position - shift) + currentTag.Html);
                    position = currentTag.Position;
                    position += currentTag.IsClose ? currentTag.Md.Length : 0;
                    currentTagId++;
                }
                else
                {
                    result.Append(mdParagraph.Substring(position));
                    break;
                }
            }

            return GetLineWithoutEscaping(result.ToString());
        }

        private List<(string Md, string Html, int Position, bool IsClose)> GetHtmlTagsList()
        {
            var openTags = tokens.Select(token =>
            (
                Md: token.Tag.Md,
                Html: token.Tag.HtmlOpen,
                Position: token.StartPosition,
                IsClose: false
            ));
            var closeTags = tokens.Select(token =>
            (
                Md: token.Tag.Md,
                Html: token.Tag.HtmlClose,
                Position: token.EndPosition,
                IsClose: true
            ));

            return openTags
                .Concat(closeTags)
                .OrderBy(tag => tag.Position)
                .ToList();
        }

        private string GetLineWithoutEscaping(string line)
        {
            var position = line.IndexOf(@"\");
            if (position == -1)
                return line;

            var result = new StringBuilder();
            var previousPosition = 0;
            while (position > -1)
            {
                var repeatCount = GeneralFunctions.SymbolInRowCount('\\', line, position);
                var newCount = (int)(repeatCount / 2);
                result.Append(line.Substring(previousPosition, position - previousPosition) +
                              new string('\\', newCount));
                previousPosition = position + repeatCount;
                position += repeatCount;
                position = line.IndexOf(@"\", position);
            }

            if (previousPosition <= line.Length - 1)
                result.Append(line.Substring(previousPosition, line.Length - previousPosition));

            return result.ToString();
        }
    }
}
