using Markdown.Core.Interfaces;

namespace Markdown.Core
{
    public class Md
    {   
        private readonly IMdTokenizer _mdTokenizer;
        private readonly IMdRender _mpRender;
        private readonly IMdParser _mdParser;

        public Md(IMdTokenizer mdTokenizer, IMdParser mdParser, IMdRender mdRender)
        {
            _mdTokenizer = mdTokenizer;
            _mdParser = mdParser;
            _mpRender = mdRender;
        }

        public string Render(string mdText)
        {
            throw new NotImplementedException();
        }
    }
}