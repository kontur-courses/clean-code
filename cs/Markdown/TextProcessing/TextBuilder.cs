using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Types;

namespace Markdown.TextProcessing
{
    public class TextBuilder
    {
        public List<Token> Tokens { get; set; }

        public TextBuilder(List<Token> tokens)
        {
            Tokens = tokens;
        }
        public string BuildText()
        {
            var strBuilder = new StringBuilder();
            foreach (var token in Tokens)
            {
                if (token.TypeToken == TypeToken.SimpleText)
                    strBuilder.Append(token.Value);
                else
                    strBuilder.AppendFormat("<{0}>{1}</{0}>", token.TypeToken.ToString().ToLower(), token.Value);
            }

            return strBuilder.ToString();
        }
    }
}