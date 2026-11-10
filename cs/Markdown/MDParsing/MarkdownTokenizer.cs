using System.Collections.Generic;
using System.Text;

namespace Markdown.Parsing
{
    public class Token
    {
        public string Text { get; set; }
        public int UnderscoreCount { get; set; } 
    }

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
                        tokens.Add(new Token { Text = currentText.ToString() });
                        currentText.Clear();
                    }

                    int count = 0;
                    while (i < text.Length && text[i] == '_')
                    {
                        count++;
                        i++;
                    }

                    // Один токен со всем количеством подчеркиваний
                    tokens.Add(new Token { UnderscoreCount = count });
                }
                else
                {
                    currentText.Append(text[i]);
                    i++;
                }
            }

            if (currentText.Length > 0)
            {
                tokens.Add(new Token { Text = currentText.ToString() });
            }

            return tokens;
        }
    }
}