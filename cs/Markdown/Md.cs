using System;

namespace Markdown
{
    public class Md
    {
        private readonly ITokenType[] fieldTypes = {new ItalicField(), new StrongField()};

        public string Render(string mdText)
        {
            var parser = new Parser(mdText, fieldTypes);
            var tokens = parser.GetTokens();

            throw new NotImplementedException();
        }
    }
}