namespace Markdown
{
    public class Md
    {
        private readonly IMdParser _parser;

        public Md(IMdParser parser)
        {
            _parser = parser;
        }

        public string Render(string mdText)
        {            
            var mdTextToken = new StringToken(mdText);
            mdTextToken.BuildTokenTree(_parser);
            return mdTextToken.Render();
        }
    }
}
