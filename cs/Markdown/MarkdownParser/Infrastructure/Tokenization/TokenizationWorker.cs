using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkdownParser.Concrete.Default;
using MarkdownParser.Infrastructure.Tokenization.Abstract;
using MarkdownParser.Infrastructure.Tokenization.Models;

namespace MarkdownParser.Infrastructure.Tokenization
{
    public class TokenizationWorker
    {
        private readonly string paragraphText;
        private readonly ITokenBuilder[] builders;
        private readonly StringBuilder textTokenBuilder = new StringBuilder();
        private bool isScreened = false;
        private int currentIndex = 0;
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
            }
            else if (TryCreateTokenFrom(builders, new TokenizationContext(paragraphText, currentIndex), out var token))
            {
                ProcessParsedToken(token);
                currentIndex += token.RawValue.Length; // эти символы мы уже "прошли" внутри TryCreate, пропускаем
            }
            else
            {
                AppendToTextNextSymbol();
                currentIndex++;
            }
        }

        private void AppendToTextNextSymbol()
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

        private void ProcessParsedToken(Token token)
        {
            if (isScreened)
            {
                textTokenBuilder.Append(token.RawValue);
                isScreened = false;
            }
            else
            {
                AppendCurrentTextToResult();
                ParsedTokens.Add(token);
            }
        }

        private void AppendCurrentTextToResult()
        {
            if (textTokenBuilder.Length != 0)
            {
                var token = CreateDefaultToken(currentIndex - textTokenBuilder.Length, textTokenBuilder.ToString());
                ParsedTokens.Add(token);
                textTokenBuilder.Clear();
            }
        }

        // TODO rewrite this method to make it more readable
        private static bool TryCreateTokenFrom(ICollection<ITokenBuilder> tokenBuilders, TokenizationContext context,
            out Token token)
        {
            // take text from source until text end, or limit iterations count with max TokenSymbol length
            var upperBound = Math.Min(
                context.CurrentStartIndex + tokenBuilders.Max(tb => tb.TokenSymbol.Length),
                context.Source.Length);
            var toRemove = new List<ITokenBuilder>();
            tokenBuilders = tokenBuilders.ToList();

            ITokenBuilder currentTokenBuilder = null;
            var text = new StringBuilder();
            for (var i = context.CurrentStartIndex; i < upperBound; i++)
            {
                text.Append(context.Source[i]);
                foreach (var builder in tokenBuilders)
                {
                    if (builder.TokenSymbol.StartsWith(text.ToString()))
                    {
                        if (builder.TokenSymbol.Length != text.Length) continue;
                        if (currentTokenBuilder?.TokenSymbol.Length == builder.TokenSymbol.Length)
                            throw new InvalidOperationException($"Multiple matching builders found for {text}");
                        currentTokenBuilder = builder;
                        toRemove.Add(builder);
                    }
                    else
                        toRemove.Add(builder);
                }

                foreach (var builder in toRemove)
                    tokenBuilders.Remove(builder);
                toRemove.Clear();
            }

            if (currentTokenBuilder == null)
            {
                token = default;
                return false;
            }

            if (currentTokenBuilder.CanCreate(context))
                token = currentTokenBuilder.Create(context);
            else
                token = CreateDefaultToken(context.CurrentStartIndex,
                    text.ToString().Substring(0, currentTokenBuilder.TokenSymbol.Length));
            return true;
        }

        private static TextToken CreateDefaultToken(int startPosition, string rawValue) =>
            new TextToken(startPosition, rawValue);

        public static TokenizationWorker CreateForParagraph(string paragraph, IEnumerable<ITokenBuilder> builders) =>
            new TokenizationWorker(paragraph, builders.ToArray());
    }
}