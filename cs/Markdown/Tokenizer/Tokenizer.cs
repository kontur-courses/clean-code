using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Tokenizer : ITokenizer
    {
        private int index;
        private readonly string text;
        private readonly IList<IToken> tokens;

        public Tokenizer(string text)
        {
            index = 0;
            this.text = text;
            tokens = new List<IToken>();
        }

        public IEnumerable<IToken> Tokenize()
        {
            AddToken(TokenType.Start);
            for (; index < text.Length; index++)
            {
                var ch = text[index];

                if (Environment.NewLine.StartsWith(ch))
                    AddToken(TokenType.LineBreak);
                else if (ch.IsTagStart<MarkdownTag>())
                    AddToken(TokenType.Tag);
                else
                    AddToken(TokenType.Text);
            }
            AddToken(TokenType.End);

            return tokens;
        }

        private void AddToken(TokenType tokenType)
        {
            var content = string.Empty;
            Func<bool> stop = () => true;

            if (tokenType == TokenType.Text)
                stop = () => text[index].IsTagStart<MarkdownTag>()
                || Environment.NewLine.StartsWith(text[index]);
            else if (tokenType == TokenType.Tag)
                stop = () => !(content + text[index]).IsPossibleTag<MarkdownTag>();
            else if (tokenType == TokenType.LineBreak)
                stop = () => content == Environment.NewLine;

            for (; index < text.Length; index++)
            {
                if (stop())
                {
                    if (tokenType != TokenType.Start && tokenType != TokenType.End)
                        index--;
                    break;
                }
                content += text[index];
            }
            tokens.Add(new Token(tokenType, content));
        }
    }
}
