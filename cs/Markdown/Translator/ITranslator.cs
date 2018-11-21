using System.Text;
using Markdown.Tokens;

namespace Markdown
{
    public interface ITranslator
    {
        void VisitText(TextToken textToken, StringBuilder stringBuilder);
        void VisitTag(TagToken tagToken, StringBuilder stringBuilder);
    }
}