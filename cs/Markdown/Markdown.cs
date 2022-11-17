using Markdown.Readers;
using Markdown.Readers.Implementation;
using Markdown.Translators;
using Markdown.Translators.Implementation;

namespace Markdown;

public class Markdown
{
    private IReader reader;
    private ITranslator translator;

    public Markdown()
    {
        reader = new HtmlReader();
        translator = new MarkdownTranslator();
    }

    public Markdown(IReader reader, ITranslator translator)
    {
        this.reader = reader;
        this.translator = translator;
    }

    public string Render(string markdownInput)
    {
        if (string.IsNullOrEmpty(markdownInput))
            throw new ArgumentNullException();

        return reader.Reader(translator.Translate(markdownInput));
    }
}