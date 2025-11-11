using System.Text;
using Markdown.Dto;

namespace Markdown.TagUtils.Abstractions;

public interface ITagProcessor
{
    public string Process(IEnumerable<Token> tokens);
}