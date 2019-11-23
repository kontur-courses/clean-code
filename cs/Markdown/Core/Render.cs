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
                else if (!lastSymbolIsBackslash && FirstTokenStartsFromPosition(linkedTranslatingTags, currentIndex))
                {
                    var (source, result) = linkedTranslatingTags.First.Value;
                    currentIndex += source.Length;
                    renderedLine.Append(result.Value);
                    linkedTranslatingTags.RemoveFirst();
                }
                else
                {
                    currentIndex++;
                    renderedLine.Append(currentSymbol);
                    lastSymbolIsBackslash = false;
                }
            }

            if (FirstTokenStartsFromPosition(linkedTranslatingTags, currentIndex))
                renderedLine.Append(linkedTranslatingTags.First.Value.result.Value);
            return renderedLine.ToString();
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

        private bool FirstTokenStartsFromPosition(LinkedList<(TagToken source, TagToken result)> tokens, int position)
        {
            return tokens.Count != 0 && position == tokens.First.Value.result.StartPosition;
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
    }
}