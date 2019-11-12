using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Token> GetAllTokens(string text)
        {
            throw new NotImplementedException();
        }

        private string ConvertToHtml(string text, IEnumerable<Token> tokens)
        {
            throw new NotImplementedException();
        }

        private string ConvertTokenToHtml(Token token)
        {
            throw new NotImplementedException();
        }

        private bool IsSeparator(string text, int position)
        {
            throw new NotImplementedException();
        }

        private string GetSeparator(string text, int position)
        {
            throw new NotImplementedException();
        }
    }
}