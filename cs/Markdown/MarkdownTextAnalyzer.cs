using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class MarkdownTextAnalyzer
    {
        public static readonly Dictionary<string, string> MarkdownWithTagsNames = new Dictionary<string, string>
        {
            {"_", "em"}, {"__", "strong"}, {"#", "h1"}
        };

        public static List<Tag> GetAllTags(string markdownText)
        {
            var result = new List<Tag>();
            result.AddRange(GetTitleTags(markdownText));
            result.AddRange(GetTagsForPairSeparators(markdownText));
            return result;
        }

        private static IEnumerable<Tag> GetTitleTags(string markdownText)
        {
            var isItTitle = false;
            var titleStart = 0;
            var tagName = MarkdownWithTagsNames["#"];
            for (var i = 0; i < markdownText.Length; i++)
            {
                if (markdownText[i] == '#' && (i == 0 || markdownText[i - 1] == '\n'))
                {
                    isItTitle = true;
                    titleStart = i;
                }

                if ((markdownText[i] == '\n' || i == markdownText.Length - 1) && isItTitle)
                {
                    yield return new Tag($"<{tagName}>", titleStart, "#");
                    yield return new Tag($"</{tagName}>", i + 1);
                    isItTitle = false;
                }
            }
        }

        private static IEnumerable<Tag> GetTagsForPairSeparators(string markdownText)
        {
            var separators = new Stack<ISeparator>();
            for (var i = 0; i < markdownText.Length; i++)
            {
                var currentSeparator = GetCurrentSeparator(markdownText, i);
                if (currentSeparator != null)
                {
                    if (IsItClosingSeparator(currentSeparator, separators))
                    {
                        var openingSeparator = separators.Pop();
                        var tagName = MarkdownWithTagsNames[currentSeparator.Value];
                        var interactionIsCorrect =
                            currentSeparator.IsSeparatorsInteractionCorrect(openingSeparator, separators);
                        if (openingSeparator.IsItCorrectOpeningSeparator() &&
                            currentSeparator.IsItCorrectClosingSeparator() &&
                            interactionIsCorrect)
                        {
                            yield return new Tag($"<{tagName}>", openingSeparator.Position,
                                openingSeparator.Value);
                            yield return new Tag($"</{tagName}>", currentSeparator.Position,
                                currentSeparator.Value);
                        }
                        else if (interactionIsCorrect)
                        {
                            separators.Push(openingSeparator);
                        }
                    }
                    else
                    {
                        separators.Push(currentSeparator);
                    }

                    i += currentSeparator.Value.Length;
                }
                else if (markdownText[i] == '\\')
                {
                    i++;
                }
                else if (markdownText[i] == '\n')
                {
                    separators = new Stack<ISeparator>();
                }
            }
        }

        private static bool IsItClosingSeparator(ISeparator separator, Stack<ISeparator> separators)
        {
            return separators.Any() && separators.Peek().Value == separator.Value;
        }

        private static ISeparator GetCurrentSeparator(string text, int position)
        {
            if (text[position] == '_')
                return position + 1 < text.Length && text[position + 1] == '_'
                    ? new BoldSeparator(position, text)
                    : new ItalicSeparator(position, text);
            return null;
        }
    }
}