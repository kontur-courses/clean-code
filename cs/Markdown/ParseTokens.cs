using Markdown.Tag;
using static Markdown.Tag.TagConfig;

namespace Markdown;

public class ParseTokens
{
    private static Stack<MdTag>? _needClosingTags;
    private static Queue<TagType>? _offsetTags;
    private static readonly Dictionary<TagType, TagType> _tagTypes = TagTypes;
    private static readonly Dictionary<TagType, string> _mdTags = MdTags;
        
    public List<Token> ParserTokens(string markdownText)
    {
        var lines = markdownText.Split("\n");
        var foundTokens = new List<Token>();

        foreach (var line in lines)
        {
            _needClosingTags = new Stack<MdTag>();
            _offsetTags = new Queue<TagType>();
            ParseTokensInLine(line, foundTokens);
        }
        
        return foundTokens;
    }

    private static void ParseTokensInLine(string line, ICollection<Token> foundTokens)
    {
        if (line.StartsWith("# "))
            foundTokens.Add(new Token(TagType.Header, 0, line.Length - 1));
        for (var i = 0; i < line.Length; i++)
        {
            if (line[i] == '_')
            {
                TagType intendedTagType;
                if (i < line.Length - 1 && line[i + 1] == '_')
                {
                    if (i < line.Length - 2 && line[i + 2] == '_')
                    {
                        i = FindEndOfInvalidTag(line, i);
                        intendedTagType = TagType.NotATag;
                    }
                    else
                    {
                        intendedTagType = TagType.Bold;
                        i++;
                    }
                }
                else
                    intendedTagType = TagType.Italic;

                if (intendedTagType != TagType.NotATag)
                    AddToken(intendedTagType, i, line, foundTokens);
            }

            if (line[i] != '\\') continue;
            if (i >= line.Length - 1 || (line[i + 1] != '\\' && line[i + 1] != '_' && line[i + 1] != '#')) continue;
            foundTokens.Add(new Token(TagType.EscapedSymbol, i, i));
            i++;
        }
    }
    private static int FindEndOfInvalidTag(string line, int index) 
    {
        var endIndex = index;
        while (endIndex < line.Length && line[endIndex] == '_')
            endIndex++;
        return endIndex;
    }

    private static void AddToken(TagType tagType, int index, string line, ICollection<Token> foundTokens)
    {
        var openingTag = OpenTag(tagType, index);

        if (openingTag.Type == TagType.NotATag)
        {
            if (index < line.Length - 1 && !char.IsWhiteSpace(line[index + 1]))
                _needClosingTags?.Push(new MdTag(tagType, index));
        }
        else
        {
            var token = new Token(tagType, openingTag.Index, index);
            if (IsPossibleToAdd(token, line))
                foundTokens.Add(token);
            else if (_offsetTags is { Count: > 0 } && _offsetTags.Peek() == tagType)
            {
                _needClosingTags?.Push(new MdTag(tagType, index));
                _offsetTags.Dequeue();
            }
        }
    }

    private static bool IsPossibleToAdd(Token token, string line)
    {
        var subString = line.Substring(token.StartIndex + 1, token.EndIndex - token.StartIndex - 1);
        var shift = _mdTags[token.TagType].Length;

        if (char.IsWhiteSpace(line[token.EndIndex - shift]))
            return false;
        if (_offsetTags!.Dequeue() == _tagTypes[token.TagType])
            return false;
        if (token.EndIndex < line.Length - 1 && !char.IsWhiteSpace(line[token.EndIndex + 1]) && subString.Any(char.IsWhiteSpace))
            return false;
        if (_needClosingTags != null && token.TagType == TagType.Bold && _needClosingTags.Any(tag => tag.Type == _tagTypes[token.TagType]))
            return false;
        if (char.IsWhiteSpace(subString[0]) || char.IsWhiteSpace(subString[^1]))
            return false;

        return token.StartIndex - 1 <= 0 || char.IsWhiteSpace(line[token.StartIndex - shift]) || !subString.Any(char.IsWhiteSpace);
    }

    private static MdTag OpenTag(TagType tagType, int index)
    {
        var openTag = new MdTag(TagType.NotATag, index);
            
        while (_needClosingTags != null && _needClosingTags.Any(tag => tag.Type == tagType))
        {
            var removeClosingTag = _needClosingTags.Pop();
            openTag = new MdTag(removeClosingTag.Type, removeClosingTag.Index);
            _offsetTags?.Enqueue(removeClosingTag.Type);
        }

        return openTag;
    }
}