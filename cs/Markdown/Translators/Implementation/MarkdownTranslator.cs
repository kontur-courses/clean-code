using Markdown.Helpers;
using Markdown.Tags;

namespace Markdown.Translators.Implementation;

public class MarkdownTranslator : ITranslator
{
    private List<ITag> tags;
    private Stack<ITag> stackOfTags;

    public MarkdownTranslator()
    {
        tags = TagHelper.GetAllTags<ITag>();
        stackOfTags = new Stack<ITag>();
    }
    
    public string Translate(string input)
    {
        throw new NotImplementedException();
    }
}