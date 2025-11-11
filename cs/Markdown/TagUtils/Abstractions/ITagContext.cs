using System.Text;
using Markdown.Dto;

namespace Markdown.TagUtils.Abstractions;

public interface ITagContext : IDisposable
{
    public void Append(string content);
    public void CloseTop(bool isFinal = false);
    public void Open(Tag tag);
    public Stack<Tag> Tags { get; }
    public StringBuilder Content { get; }
    public bool SkipNextAsMarkup { get; set; }
}