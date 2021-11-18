using Markdown.Interfaces;

namespace Markdown
{
    public class Md
    {
        public string Render(string text, IConvectorMarkup convecter)
        {
            Tokenizer tokenizer = new Tokenizer();
            return convecter.Convert(tokenizer.Parse(text));
        }
    }
}
