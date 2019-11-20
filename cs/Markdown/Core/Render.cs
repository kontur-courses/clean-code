using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Core.Rules;
using Markdown.Core.Tags;

namespace Markdown.Core
{
    class Render
    {
        private readonly IEnumerable<IRule> rules;

        public Render(IEnumerable<IRule> rules)
        {
            this.rules = rules;
        }

        private IEnumerable<(int, TagToken)> TranslateTags(string line, IEnumerable<TagToken> tokens)
        {
            foreach (var token in tokens)
            {
                var currentRule = rules.FirstOrDefault(r => r.SourceTag == token.Tag);
                if (currentRule == null)
                    continue;
                var resultTag = currentRule.ResultTag;
                if (token.Tag is ISingleTag && resultTag is IDoubleTag tag)
                {
                    yield return (token.Length,
                        new TagToken(token.StartPosition, tag, tag.Opening, true, token.IsSkipped));
                    yield return (0,
                        new TagToken(line.Length, tag, tag.Closing, false, token.IsSkipped));
                }
                else
                    switch (resultTag)
                    {
                        case IDoubleTag doubleTag:
                        {
                            var valueToken = token.IsOpening ? doubleTag.Opening : doubleTag.Closing;
                            yield return (token.Length, new TagToken(token.StartPosition, doubleTag,
                                valueToken,
                                token.IsOpening, token.IsSkipped));
                            break;
                        }
                        case ISingleTag singleTag:
                            yield return (token.Length,
                                new TagToken(token.StartPosition, singleTag, singleTag.Opening, true, token.IsSkipped));
                            break;
                    }
            }
        }

        public string RenderLine(string line, IEnumerable<TagToken> tokens)
        {
            var offsetAfterAdding = 0;
            var tuplesSourceLengthAndResultTag =
                TranslateTags(line, tokens).OrderBy(pair => pair.Item2.StartPosition).ThenBy(pair => pair.Item2.Length)
                    .ToList();

            var renderedLine = new StringBuilder(line);

            foreach (var (sourceTagLength, token) in tuplesSourceLengthAndResultTag)
            {
                if (token.IsSkipped)
                {
                    renderedLine.Remove(token.StartPosition + offsetAfterAdding - 1, 1);
                    offsetAfterAdding -= 1;
                }
                else
                {
                    renderedLine.Remove(token.StartPosition + offsetAfterAdding, sourceTagLength);
                    renderedLine.Insert(token.StartPosition + offsetAfterAdding, token.Value);
                    offsetAfterAdding += token.Length - sourceTagLength;
                }
            }

            return renderedLine.ToString();
        }
    }
}