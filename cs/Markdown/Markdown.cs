using Markdown.Styles;
using Markdown.Tokens;
using System.Collections.Generic;

namespace Markdown
{
    public class Md
    {
        public string Render(string mdText)
        {
            if (string.IsNullOrWhiteSpace(mdText))
                return mdText;

            var tokens = RecognizeTokens(ref mdText);

            var styles = RecognizeStyles(tokens);

            return TextConverter.HTMLConverter().Convert(styles);
        }

        private List<Token> RecognizeTokens(ref string mdText)
        {
            var tokens = new List<Token>();
            int i = 0;
            int wordBegin = 0;
            int wordLength = 0;
            while (i < mdText.Length)
            {
                if (SpecSymbol.IsSpecSymbol(mdText[i], out SpecSymbol specSymbol))
                {
                    if (wordLength > 0)
                    {
                        tokens.Add(new Word(mdText.Substring(wordBegin, wordLength)));
                        wordLength = 0;
                    }
                    tokens.Add(specSymbol);
                }
                else
                {
                    if (wordLength == 0)
                        wordBegin = i;
                    wordLength++;
                }

                i++;
            }
            if (wordLength > 0)
                tokens.Add(new Word(mdText.Substring(wordBegin, wordLength)));

            CorrectBackslashedTokens(tokens);

            return tokens;
        }

        private void CorrectBackslashedTokens(List<Token> tokens)
        {
            int i = 0;
            while (i < tokens.Count - 1)
            {
                if (tokens[i] is Backslash && tokens[i + 1] is Underline underline)
                {
                    tokens[i + 1] = new Word(underline.Symbol.ToString());
                    tokens.RemoveAt(i);
                }
                i++;
            }
        }

        private Style RecognizeStyles(List<Token> tokens)
        {
            var result = new DefaultStyle();
            result.ChildTokens.AddRange(tokens);
            return result;
        }
    }
}
