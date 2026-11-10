using System.Collections.Generic;
using System.Text;
using Markdown.MDParsing;

namespace Markdown.Parsing
{
    public static class MarkdownTokenizer
    {
        public static List<Token> Tokenize(string text)
        {
            var tokens = new List<Token>();
            var currentText = new StringBuilder();
            int i = 0;

            while (i < text.Length)
            {
                if (text[i] == '_')
                {
                    if (currentText.Length > 0)
                    {
                        tokens.Add(new Token(text: currentText.ToString()));
                        currentText.Clear();
                    }

                    int count = 0;
                    while (i < text.Length && text[i] == '_')
                    {
                        count++;
                        i++;
                    }

                    tokens.Add(new Token(underscoreCount: count));
                }
                else
                {
                    currentText.Append(text[i]);
                    i++;
                }
            }

            if (currentText.Length > 0)
            {
                tokens.Add(new Token(text: currentText.ToString()));
            }

            return tokens;
        }
    }
}