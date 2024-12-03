using Markdown.TagInfo;
using Markdown.TokenInfo;

namespace Markdown.MarkdownParsers;

internal class SingleTagParser(string tag, Tag tagType) : IMarkdownParser
{
    private const int CloseTagLength = 0;
    private readonly int openTagLength = tag.Length;

    public IEnumerable<Token> Parse(string markdown)
    {
        var tagSearchStart = 0;
        var tokens = new List<Token>();

        foreach (var paragraph in markdown.Split('\n'))
        {
            var length = paragraph.Length;

            if (IsSingleTag(paragraph))
            {
                AddSingleTag(tokens, tagSearchStart, length);
            }

            tagSearchStart += length;
        }

        return tokens;
    }

    private void AddSingleTag(List<Token> tokens, int position, int length)
    {
        tokens.Add(CreateSingleTagToken(position, true));
        tokens.Add(CreateSingleTagToken(position + length, false));
    }

    private bool IsSingleTag(string paragraph) => paragraph.StartsWith(tag);

    private Token CreateSingleTagToken(int position, bool isOpeningTag)
    {
        return new Token(tagType,
            position,
            isOpeningTag ? TagType.Open : TagType.Close,
            isOpeningTag ? openTagLength : CloseTagLength);
    }
}