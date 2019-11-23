using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Core.Rules;
using Markdown.Core.Tags;

namespace Markdown.Core
{
    public class Render
    {
        private readonly IEnumerable<IRule> rules;
        private const char BackSlash = '\\';

        public Render(IEnumerable<IRule> rules)
        {
            this.rules = rules;
        }

        private bool IsSkippedTag(string line, int index) => index != 0 && line[index - 1] == BackSlash;

        private IEnumerable<TagToken> TranslateSingleToDouble(string line, TagToken token, IDoubleTag tag)
        {
            var openingTagToken = new TagToken(token.StartPosition, tag, tag.Opening, true);
            var closingTagToken = new TagToken(line.Length, tag, tag.Closing, false);
            yield return openingTagToken;
            yield return closingTagToken;
        }

        private TagToken TranslateToSingleTag(TagToken token, ISingleTag singleTag)
        {
            var openingSingleTagToken = new TagToken(token.StartPosition, singleTag, singleTag.Opening, true);
            return openingSingleTagToken;
        }

        private TagToken TranslateToDoubleTag(TagToken token, IDoubleTag doubleTag)
        {
            var valueToken = token.IsOpening ? doubleTag.Opening : doubleTag.Closing;
            var openingDoubleTagToken = new TagToken(token.StartPosition, doubleTag, valueToken,
                token.IsOpening);
            return openingDoubleTagToken;
        }

        private IEnumerable<(TagToken source, TagToken result)> TranslateTags(string line, IEnumerable<TagToken> tokens)
        {
            foreach (var token in tokens)
            {
                if (IsSkippedTag(line, token.StartPosition))
                {
                    yield return (token, token);
                    continue;
                }

                var currentRule = rules.FirstOrDefault(r => r.SourceTag == token.Tag);
                if (currentRule == null)
                    continue;
                var resultTag = currentRule.ResultTag;
                if (token.Tag is ISingleTag && resultTag is IDoubleTag tag)
                {
                    foreach (var resultToken in TranslateSingleToDouble(line, token, tag))
                        yield return (token, resultToken);
                }
                else
                {
                    if (resultTag is IDoubleTag doubleTag)
                        yield return (token, TranslateToDoubleTag(token, doubleTag));
                    else if (resultTag is ISingleTag singleTag)
                        yield return (token, TranslateToSingleTag(token, singleTag));
                }
            }
        }

        public string RenderLine(string line, IEnumerable<TagToken> tokens)
        {
            var translatingTags = TranslateTags(line, tokens)
                .OrderBy(pair => pair.Item2.StartPosition)
                .ThenBy(pair => pair.Item2.Length);
            var linkedTranslatingTags = new LinkedList<(TagToken source, TagToken result)>(translatingTags);

            var currentIndex = 0;
            var lastSymbolIsBackslash = false;
            var renderedLine = new StringBuilder();
            while (currentIndex < line.Length)
            {
                var currentSymbol = line[currentIndex];
                if (currentSymbol == BackSlash)
                {
                    if (lastSymbolIsBackslash)
                        renderedLine.Append(currentSymbol);
                    currentIndex++;
                    lastSymbolIsBackslash = !lastSymbolIsBackslash;
                }
                else if (!lastSymbolIsBackslash && linkedTranslatingTags.Count != 0 &&
                         currentIndex == linkedTranslatingTags.First.Value.result.StartPosition)
                {
                    var currentTranslate = linkedTranslatingTags.First.Value;
                    currentIndex += currentTranslate.source.Length;
                    renderedLine.Append(currentTranslate.result.Value);
                    linkedTranslatingTags.RemoveFirst();
                }
                else
                {
                    currentIndex++;
                    renderedLine.Append(currentSymbol);
                    lastSymbolIsBackslash = false;
                }
            }

            var (source, result) = linkedTranslatingTags.FirstOrDefault();
            if (source != null && result != null && result.StartPosition == currentIndex)
                renderedLine.Append(result.Value);
            return renderedLine.ToString();
        }
    }
}