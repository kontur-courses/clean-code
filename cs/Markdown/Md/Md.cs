namespace Markdown.Md
{
    public class Md : IConverter
    {
        private readonly IParser<MdToken> parser;
        private readonly IRenderer renderer;

        public Md(IParser<MdToken> parser, IRenderer renderer)
        {
            this.parser = parser;
            this.renderer = renderer;
        }

        public string Convert(string markdown)
        {
            var result = parser.Parse(markdown);

            return renderer.Render(result);
        }
    }
}