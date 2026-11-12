using System.Text;

namespace Markdown.MDParsing
{
    public static class MarkdownTokenizer
    {
        // Специальные символы разметки, которые можно экранировать
        private static readonly HashSet<char> SpecialChars = new HashSet<char> { '_', '#', '\\' };

        public static List<Token> Tokenize(string text)
        {
            var tokens = new List<Token>();
            var currentText = new StringBuilder();
            int i = 0;

            while (i < text.Length)
            {
                if (text[i] == '\\')
                {
                    if (i + 1 < text.Length && SpecialChars.Contains(text[i + 1]))
                    {
                        if (currentText.Length > 0)
                        {
                            tokens.Add(Token.CreateText(currentText.ToString()));
                            currentText.Clear();
                        }

                        tokens.Add(Token.CreateEscaped(text[i + 1]));
                        i += 2; 
                    }
                    else
                    {
                        currentText.Append('\\');
                        i++;
                    }
                }
                else if (text[i] == '_')
                {
                    if (currentText.Length > 0)
                    {
                        tokens.Add(Token.CreateText(currentText.ToString()));
                        currentText.Clear();
                    }

                    int count = 0;
                    while (i < text.Length && text[i] == '_')
                    {
                        count++;
                        i++;
                    }

                    tokens.Add(Token.CreateUnderscores(count));
                }
                else
                {
                    currentText.Append(text[i]);
                    i++;
                }
            }

            if (currentText.Length > 0)
            {
                tokens.Add(Token.CreateText(currentText.ToString()));
            }

            return tokens;
        }
    }
}