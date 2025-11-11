using System.Text;
using Markdown.Dto;
using Markdown.TagUtils.Abstractions;
using Markdown.TokensUtils;

namespace Markdown.TagUtils.Implementations;

public class TagContext : ITagContext
{
    public Stack<Tag> Tags { get; } = new();
    public StringBuilder Content { get; } = new();
    public bool SkipNextAsMarkup { get; set; } 

    public void Append(string content)
    {
        if (Tags.Count > 0)
        {
            Tags.Peek().Append(content);
        }
        else
        {
            Content.Append(content);
        }
    }

    public void Open(Tag tag)
    {
        Tags.Push(tag);
    }


    public void CloseTop(bool isFinal = false)
    {
        var top = Tags.Pop();
        var content = top.Content.ToString();
        var wrapped = new StringBuilder();

        if (isFinal)
        {
            wrapped.Append(top.Token.Type == TokenType.Header 
                ? TagRender.Wrap(top.Token.Type, content[1..]) 
                : content);
        }
        else
        {
            var markerLength = top.Token.Value.Length;
            var innerContent = content.Length > markerLength ? content[markerLength..] : string.Empty;
            wrapped.Append(TagRender.Wrap(top.Token.Type, innerContent));
        }

        Append(wrapped.ToString());
    }

    public void Dispose()
    {
        Tags.Clear();
        Content.Clear();
    }
}