using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HtmlConverter
    {
        private Stack<Token> upperTokens = new Stack<Token>();

        public string Convert(IEnumerable<Token> tokens)
        {
            var sb = new StringBuilder();
            foreach (var token in tokens)
                sb.Append(TokenToHtml(token));
            return sb.ToString()[..(sb.Length - 2)];
        }

        private string TokenToHtml(Token token)
        {
            switch (token)
            {
                case StrongText t:
                    return TokenToHtml(t);
                case ItalicText t:
                    return TokenToHtml(t);
                case Paragraph t:
                    return TokenToHtml(t);
                case Header t:
                    return TokenToHtml(t);
                default:
                    return token.Value;
            }
        }

        private string TokenToHtml(StrongText token)
            => DecorateWithTag(token, "strong", "__");

        private string TokenToHtml(ItalicText token)
            => DecorateWithTag(token, "em", "_");

        private string DecorateWithTag(Token token, string tag, string altTag)
        {
            if (token.Closed)
            {
                var goodOpenClosing = !token.HaveInner || token.InnerTokens[0].Value[0] != ' ' &&
                    !IsLastElementWhiteSpace(token);
                var canBeTag = token.Valid && token.HaveInner &&
                    !(token is StrongText && upperTokens.Peek() is ItalicText);
                return canBeTag && goodOpenClosing ?
                    $"<{tag}>{AddTextInTag(token)}</{tag}>" :
                    $"{altTag}{AddTextInTag(token)}{altTag}";
            }
            return $"{altTag}{AddTextInTag(token)}";
        }

        private bool IsLastElementWhiteSpace(Token token)
            => token.HaveInner && !string.IsNullOrEmpty(token.InnerTokens.Last().Value)
                && token.InnerTokens.Last().Value.Last() == ' ';

        private string TokenToHtml(Paragraph token) 
            => $"<div>{AddTextInTag(token)}</div>\r\n";

        private string TokenToHtml(Header token) 
            => $"<h1>{AddTextInTag(token)}</h1>\r\n";

        private string AddTextInTag(Token token)
        {
            if(!token.HaveInner)
                return token.Value;
            var sb = new StringBuilder();
            upperTokens.Push(token);
            foreach (var innerToken in token.InnerTokens)
                sb.Append(TokenToHtml(innerToken));
            upperTokens.Pop();
            return EscapeSymbols(sb.ToString());
        }

        private string EscapeSymbols(string text)
        {
            var res = new StringBuilder();
            for (var i = 0; i < text.Length; i++)
            {
                if (text[i] == '\\' && i + 1 < text.Length && text[i + 1] == '<') 
                        continue;
                res.Append(text[i]);
            }
            return res.ToString();
        }
    }
}
