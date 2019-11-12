using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Processor
    {
        private readonly Syntax syntax;

        public Processor(Syntax syntax)
        {
            this.syntax = syntax;
        }

        public string Render(string source)
        {
            if (source == null)
                throw new ArgumentException();

            var tokens = ParseText(source);


            return ReplaceAttributesWithTags(tokens, source);
        }

        private IEnumerable<Token> ParseText(string source)
        {
            var tokens = FindPossibleTokens(source);
            tokens = RemoveEscapedTokens(tokens, source);
            return tokens;
        }


        private IEnumerable<Token> FindPossibleTokens(string source)
        {
            var possibleTokens = new List<Token>();

            for (var i = 0; i < source.Length; i++)
                if (syntax.TypeDictionary.ContainsKey(source[i]))
                    possibleTokens.Add(new Token(syntax.TypeDictionary[source[i]], i));

            return possibleTokens;
        }

        private IEnumerable<Token> RemoveEscapedTokens(IEnumerable<Token> tokens, string source)
        {
            var unescapedTokens = new List<Token>();
            Token previous = null;

            foreach (var token in tokens)
            {
                if (previous != null && previous.Type == AttributeType.Escape && previous.Position == token.Position - 1)
                    continue;
                unescapedTokens.Add(token);
                previous = token;
            }

            return unescapedTokens;
        }


        private IEnumerable<Token> RemoveNonPairDelimiters(IEnumerable<Token> tokens)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Token> MergeAdjacentDelimiters(IEnumerable<Token> tokens)
        {
            throw new NotImplementedException();
        }

        private string ReplaceAttributesWithTags(IEnumerable<Token> tokens, string source)
        {
            var textPosition = 0;
            var sb = new StringBuilder();

            foreach (var token in tokens)
            {
                sb.Append(source.Substring(textPosition, token.Position - textPosition));

                if (token.Type == AttributeType.Escape &&
                    (token.Position == source.Length - 1 || !Syntax.CharCanBeEscaped(source[token.Position + 1])))
                    sb.Append('\\');
                else
                {
                    sb.Append($"<{(token.IsEnd ? "/" : "")}{HtmlConverter.TagDictionary[token.Type]}>");
                }

                textPosition = token.Position + 1;
            }

            sb.Append(source.Substring(textPosition, source.Length - textPosition));
            return sb.ToString();
        }
    }
}