using System.Collections.Immutable;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Parsers;

public class MarkdownParser : ITextParser<Tag>
{
    private readonly HashSet<string> _escapeCharacters;
    private readonly HashSet<string> _tags;
    private readonly ImmutableDictionary<TagDefenition, string> _tagsDefenitions;

    public MarkdownParser(HashSet<string> escapeCharacters, ImmutableDictionary<TagDefenition, string> tagsDefenitions)
    {
        _escapeCharacters = escapeCharacters;
        _tagsDefenitions = tagsDefenitions;
        _tags = tagsDefenitions.Values.ToHashSet();
    }

    public List<IToken<Tag>> ParseText(string text)
    {
        throw new NotImplementedException();
    }

    private List<TagToken> ParseParagraph(int startIndex, string paragraph)
    {
        var paredTags = new Stack<TagToken>();
        throw new NotImplementedException();
    }

    private bool IsHeader(string paragraph)
    {
        throw new NotImplementedException();
    }

    private bool IsTag(int startIndex, int endIndex)
    {
        throw new NotImplementedException();
    }

    private TagToken GetTag(int startIndex, int endIndex)
    {
        throw new NotImplementedException();
    }

    private TagType GetTagType(int startIndex, int endIndex)
    {
        throw new NotImplementedException();
    }

    private bool IsEscapeCharacter(int startIndex, int endIndex)
    {
        throw new NotImplementedException();
    }

    private TagToken GetEscapeSequence(int startIndex, int endIndex)
    {
        throw new NotImplementedException();
    }

    private List<TagToken> SortTokens(Dictionary<int, TagToken> tokens)
    {
        throw new NotImplementedException();
    }
}