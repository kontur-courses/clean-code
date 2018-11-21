using System.Text;

namespace Markdown.Tags
{
    public interface IToken
    {
        string Text { get; }
        int Position { get; }
        void Translate(ITranslator translator, StringBuilder stringBuilder);
    }
}
