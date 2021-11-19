using System;
using System.Collections;
using System.Collections.Generic;

namespace Markdown
{
    public class HtmlConverter
    {
        public string Convert(IEnumerable<Token> tokens)
        {
            //foreach
            // TokenToHtml
            throw new NotImplementedException();
        }

        private string TokenToHtml(Token token)
        {
            //switch(token) by Type => ToHtml
            throw new NotImplementedException();
        }

        private string HeaderToHtml(Token token)
        {
            throw new NotImplementedException();
        }
        private string ParagraphToHtml(Token token)
        {
            throw new NotImplementedException();
        }

        private string StrongToHtml(Token token)
        {
            throw new NotImplementedException();
        }

        private string ItalicToHtml(Token token)
        {
            throw new NotImplementedException();
        }

        private string EscapeSymbols(string text)
        {
            throw new NotImplementedException();
        }
    }
}
