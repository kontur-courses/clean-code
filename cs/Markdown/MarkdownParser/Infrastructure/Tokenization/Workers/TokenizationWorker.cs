using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Infrastructure.Tokenization.Workers
{
    public class TokenizationWorker
    {
        private readonly string paragraphText;
        private readonly ITokenBuilder[] builders;
        private readonly StringBuilder textTokenBuilder = new StringBuilder();
        private bool isScreened;
        private int currentIndex;
        private char CurrentChar => paragraphText[currentIndex];

        public List<Token> ParsedTokens;

        private TokenizationWorker(string paragraphText, ITokenBuilder[] builders)
        {
            this.paragraphText = paragraphText;
            this.builders = builders;
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

            if (TokenCreator.TryCreateFrom(builders, new TokenizationContext(paragraphText, currentIndex),
                out var token))
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

        public static TokenizationWorker CreateForParagraph(string paragraph, IEnumerable<ITokenBuilder> builders) =>
            new TokenizationWorker(paragraph, builders.ToArray());
    }
}