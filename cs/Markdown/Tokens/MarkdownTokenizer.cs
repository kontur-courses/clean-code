using Markdown.Extensions;
using Markdown.Tags;
using System.Text;
using Markdown.Interfaces;
using Markdown.Enums;

namespace Markdown.Tokens;

public static class MarkdownTokenizer
{
    private static readonly Dictionary<string, Func<IMarkdownTag>> CustomTags = TagsRepository.GetCustomTags();

    public static List<MarkdownToken> TransformTextToTokens(string inputText)
    {
        if (inputText is null)
        {
            throw new ArgumentNullException(nameof(inputText), "Исходный текст не может быть пустым.");
        }

        var tokenList = new List<MarkdownToken>();
        var collector = new StringBuilder();

        inputText = " " + inputText + " ";

        for (var i = 1; i < inputText.Length - 1; i++)
        {
            if (inputText[i + 1].IsEscapedBy(inputText[i]))
            {
                collector.Append(inputText[i + 1]);
                i += 1;
                continue;
            }

            var tagToken = GetTagTokenAtPosition(i, inputText);

            if (tagToken != null)
            {
                AddAndResetCollector(tokenList, collector);

                tokenList.Add(tagToken);

                HandleBrokenTags(tokenList, tagToken);

                i += tagToken.AssociatedTag!.Info.GlobalTag.Length - 1;
            }
            else
            {
                collector.Append(inputText[i]);
            }
        }

        AddAndResetCollector(tokenList, collector);

        tokenList.Add(new MarkdownToken(new Newline()));

        var tagTokens = tokenList
            .Where(token => token.AssociatedTag != null)
            .ToList();

        tagTokens.SetTagStatuses();
        tagTokens.RemoveIntersectingTags();

        return tokenList;
    }

    private static void AddAndResetCollector(List<MarkdownToken> tokens, StringBuilder collector)
    {
        if (collector.Length > 0)
        {
            tokens.Add(new MarkdownToken(textContent: collector.ToString()));
            collector.Clear();
        }
    }

    private static void HandleBrokenTags(List<MarkdownToken> tokens, MarkdownToken tagToken)
    {
        if (tokens.Count > 1)
        {
            var previous = tokens[^2];

            if (previous.AssociatedTag != null && previous.AssociatedTag.Type == tagToken.AssociatedTag!.Type &&
                previous.AssociatedTag.Type != TagType.Newline)
            {
                previous.AssociatedTag.Status = TagStatus.Broken;
                tagToken.AssociatedTag.Status = TagStatus.Broken;
            }
        }
    }

    private static MarkdownToken? GetTagTokenAtPosition(int position, string text)
    {
        MarkdownToken? foundToken = null;

        if (text[position] == '+')
        {
            var context = new MarkdownContext(position, text);
            var liTag = TagsRepository.GetCustomTags()["+"]();
            liTag.MarkdownContext = context;
            foundToken = new MarkdownToken(liTag);
        }
        else
        {
            var prefix = string.Concat(text[position], text[position + 1]);
            var foundTag = GetInstanceViaMark(prefix);

            if (foundTag != null)
            {
                var context = new MarkdownContext(position, text);

                foundTag.MarkdownContext = context;
                foundToken = new MarkdownToken(foundTag);
            }
        }

        return foundToken;
    }

    private static IMarkdownTag? GetInstanceViaMark(string mark)
    {
        var customTags = TagsRepository.GetCustomTags();
        var foundTag = customTags.FirstOrDefault(pair => mark.StartsWith(pair.Key));

        return foundTag.Value?.Invoke();
    }
}