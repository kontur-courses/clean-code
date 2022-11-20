using System.Text;
using MarkdownProcessor.Markdown;

namespace MarkdownProcessor;

public class Md
{
    private readonly Dictionary<string, ITagMarkdownConfig> configs =
        new ITagMarkdownConfig[] { new ItalicConfig(), new BoldConfig() }
            .ToDictionary(c => c.Sign);

    private ITag? tree = null;

    public string Render(string text)
    {
        var result = new StringBuilder();
        var paragraphs = text.Split('\n');

        var closedTags = new List<ITag>();
        var paragraphIterator = 0;
        foreach (var paragraph in paragraphs)
        {
            var tokens = new List<Token>();
            string? assembledTag = null;
            var backslash = false;
            foreach (var symbol in paragraph)
            {
                if (backslash)
                {
                    if (!(symbol == '\\' || configs.Any(t => t.Key.StartsWith(symbol))))
                        result.Append('\\');
                    result.Append(symbol);

                    backslash = false;
                    continue;
                }

                if (symbol == '\\')
                {
                    backslash = true;
                    continue;
                }

                if (assembledTag is null)
                {
                    if (configs.Any(t => t.Key.StartsWith(symbol)))
                        assembledTag = symbol.ToString();
                }
                else
                {
                    if (configs.Any(t => t.Key.StartsWith(assembledTag + symbol)))
                    {
                        assembledTag += symbol.ToString();
                    }
                    else
                    {
                        if (configs.ContainsKey(assembledTag))
                        {
                            var tagFirstCharIndex = result.Length - assembledTag.Length;
                            char? charBefore = tagFirstCharIndex == 0
                                ? null
                                : paragraph[tagFirstCharIndex - 1 - paragraphIterator];
                            tokens.Add(new Token(
                                configs[assembledTag],
                                tagFirstCharIndex,
                                charBefore,
                                symbol));
                        }

                        assembledTag = null;
                    }
                }

                result.Append(symbol);
            }

            if (assembledTag is not null && configs.ContainsKey(assembledTag))
            {
                var tagFirstCharIndex = result.Length - assembledTag.Length;
                Console.WriteLine((tagFirstCharIndex - 1 - paragraphIterator, paragraph.Length));
                char? charBefore = tagFirstCharIndex == 0 ? null : paragraph[tagFirstCharIndex - 1 - paragraphIterator];
                tokens.Add(new Token(
                    configs[assembledTag],
                    tagFirstCharIndex,
                    charBefore,
                    null));
            }

            ITag? openedItalic = null;
            ITag? openedBold = null;

            foreach (var token in tokens)
            {
                Console.WriteLine(openedBold);
                Console.WriteLine(openedItalic);
                switch (token.Config.TextType)
                {
                    case TextType.Italic:
                        if (openedItalic is null)
                        {
                            openedItalic = token.Config.TryCreate(token);
                        }
                        else if (token.Config.IsClosingToken(token))
                        {
                            openedItalic.EndIndex = token.TagFirstCharIndex;
                            closedTags.Add(openedItalic);
                            openedItalic = null;
                        }

                        break;
                    case TextType.Bold:
                        if (openedBold is null)
                        {
                            if (openedItalic is null)
                                openedBold = token.Config.TryCreate(token);
                        }
                        else if (token.Config.IsClosingToken(token))
                        {
                            openedBold.EndIndex = token.TagFirstCharIndex;
                            closedTags.Add(openedBold);
                            openedBold = null;
                        }

                        break;
                }
            }

            result.Append('\n');
            paragraphIterator = result.Length;
        }

        result.Remove(result.Length - 1, 1);
        Console.WriteLine(closedTags.Count);
        return new HtmlRenderer().Render(closedTags, result);
    }
}