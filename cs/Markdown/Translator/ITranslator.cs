using Markdown.Tokens;

namespace Markdown
{
    public interface ITranslator
    {
        string VisitTag(TagToken tagToken);
        string VisitText(TextToken textToken);
    }
}