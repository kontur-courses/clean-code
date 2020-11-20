using System.Collections.Generic;
using System.Text;
using MarkdownParser.Infrastructure.Tokenization.Abstract;

namespace MarkdownParser.Infrastructure.Tokenization.Workers
{
    public class TokenizationWorker
    {
        private readonly string paragraphText;
        private readonly StringBuilder textTokenBuilder = new StringBuilder();
        private readonly TokenCreator tokenCreator;
        private bool isScreened;
        private int currentIndex;
        private char CurrentChar => paragraphText[currentIndex];

        public List<Token> ParsedTokens;

        public TokenizationWorker(string paragraphText, ITokenBuilder[] builders)
        {
            this.paragraphText = paragraphText;
            tokenCreator = new TokenCreator(builders, paragraphText);
        }

        public IList<Token> ParseTokens()
        {
            if (ParsedTokens != null)
                return ParsedTokens;

            ParsedTokens = new List<Token>();
            while (currentIndex < paragraphText.Length)
                MoveNext();

            AppendCurrentTextToResult();

            return ParsedTokens;
        }

        private void MoveNext()
        {
            if (CurrentChar == '\\')
            {
                ProcessScreeningChar();
                currentIndex++;
                return;
            }

            if (tokenCreator.TryCreateOnPosition(currentIndex, out var token))
            {
                if (!isScreened)
                {
                    AppendCurrentTextToResult();
                    ParsedTokens.Add(token);
                    currentIndex += token.RawValue.Length; // эти символы мы уже "прошли" внутри TryCreate, пропускаем
                    return;
                }

                isScreened = false;
            }

            AppendToTextCurrentSymbol();
            currentIndex++;
        }

        private void AppendToTextCurrentSymbol()
        {
            if (isScreened) AppendScreeningCharAsText();
            textTokenBuilder.Append(CurrentChar);
        }

        private void ProcessScreeningChar()
        {
            if (isScreened) AppendScreeningCharAsText();
            else isScreened = true;
        }

        private void AppendScreeningCharAsText()
        {
            textTokenBuilder.Append(@"\");
            isScreened = false;
        }

        private void AppendCurrentTextToResult()
        {
            if (textTokenBuilder.Length != 0)
            {
                var token = TokenCreator.CreateDefault(
                    currentIndex - textTokenBuilder.Length,
                    textTokenBuilder.ToString());
                ParsedTokens.Add(token);
                textTokenBuilder.Clear();
            }
        }
    }
}