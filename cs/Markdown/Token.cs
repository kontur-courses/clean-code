using System.Collections;
using Markdown.Parsers;
using Markdown.TagsType;

namespace Markdown;

public class Token
{
    public string Context { get; set; }
    public int Length => Context.Length;
    public static int IdCounter = 0;
    public readonly int Id;
    public ITagsType Type { get; }

    public Token(string context, ITagsType type)
    {
        Context = context;
        Type = type;
        Id = IdCounter++;
    }
}