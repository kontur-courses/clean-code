using Markdown.TagClasses.TagModels;
using System.Reflection;

namespace Markdown.TagClasses;

public class ItalicTag : Tag
{
    public ItalicTag() : base(new ItalicModel())
    {
    }
}