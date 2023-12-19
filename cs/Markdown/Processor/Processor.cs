using System.Text;
using Markdown.Syntax;
using Markdown.Token;

namespace Markdown.Processor;

public class Processor : IProcessor
{
    private readonly string source;
    private readonly ISyntax syntax;
    private readonly IReadOnlyDictionary<string, Func<int, IToken>> stringToToken;

    public Processor(string source, ISyntax syntax)
    {
        this.source = source;
        this.syntax = syntax;
        stringToToken = syntax.StringToToken;
    }

    public IList<IToken> ParseTags()
    {
        var tags = FindAllTags();
        tags = RemoveEscapedTags(tags);
        tags = ValidateTagPositioning(tags);
        return tags;
    }

    private IList<IToken> FindAllTags()
    {
        var tags = new List<IToken>();
        var possibleTag = new StringBuilder();
        for (var i = 0; i < source.Length; i++)
        {
            if (stringToToken.Keys.Any(s => s.StartsWith(possibleTag.ToString() + source[i])))
            {
                possibleTag.Append(source[i]);
                continue;
            }
            
            if (source[i] == '\n')
                possibleTag.Clear();

            var tag = possibleTag.ToString();
            
            if (stringToToken.ContainsKey(tag))
                tags.Add(stringToToken[tag].Invoke(i-tag.Length));
            
            possibleTag.Clear();
            
            if (stringToToken.Keys.Any(s => s.StartsWith(possibleTag.ToString() + source[i])))
                possibleTag.Append(source[i]);
        }
        
        if (possibleTag.Length > 0)
            tags.Add(stringToToken[possibleTag.ToString()].Invoke(source.Length-possibleTag.Length));

        return tags;
    }

    private IList<IToken> RemoveEscapedTags(IList<IToken> tags)
    {
        var result = new List<IToken>();
        var isEscaped = false;
        foreach (var tag in tags)
        {
            if (isEscaped)
            {
                isEscaped = false;
                continue;
            }
            
            if (tag.GetType() == syntax.EscapeToken)
                isEscaped = true;
            else
                result.Add(tag);
        }

        return result;
    }

    private IList<IToken> ValidateTagPositioning(IList<IToken> tags)
    {
        var result = new List<IToken>();
        var openedTags = new Dictionary<string, IToken>();
        
        foreach (var tag in tags)
        {
            if (openedTags.ContainsKey(tag.Separator))
            {
                tag.IsClosed = true;
                if (tag.IsValid(source) && tag.IsValidPositioned(openedTags[tag.Separator], source))
                {
                    result.Add(openedTags[tag.Separator]);
                    result.Add(tag);
                    openedTags.Remove(tag.Separator);
                }
            }
            else if (tag.IsValid(source))
            {
                if (tag.IsPair)
                    openedTags[tag.Separator] = tag;
            }
        }

        return result;
    }
}