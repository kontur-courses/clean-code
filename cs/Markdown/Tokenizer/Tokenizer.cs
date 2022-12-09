using System;
using System.Collections.Generic;
using Markdown.Enums;
using Markdown.Extensions;
using Markdown.Tag;
using Markdown.TokenNamespace;

namespace Markdown.TokenizerNamespace
{
    public class Tokenizer : ITokenizer
    {
        private int index;
        private string content;
        private readonly string text;
        private readonly IList<IToken> tokens;

        public Tokenizer(string text)
        {
            index = 0;
            content = string.Empty;
            this.text = text;
            tokens = new List<IToken>();
        }

        public IEnumerable<IToken> Tokenize()
        {
            AddToken(TokenType.Start);
            for (; index < text.Length; index++)
            {
                var ch = text[index];

                if (ch.IsNewLine())
                    AddToken(TokenType.LineBreak);
                else if (ch.IsTagStart<MarkdownTag>())
                    AddToken(TokenType.Tag);
                else if (ch.IsEscapeCharacter())
                    AddToken(TokenType.Escape);
                else
                    AddToken(TokenType.Text);
            }
            AddToken(TokenType.End);

            return tokens;
        }

        private void AddToken(TokenType tokenType)
        {
            content = string.Empty;
            var stop = GetTokenStop(tokenType);

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

        private Func<bool> GetTokenStop(TokenType tokenType)
        {
            return tokenType switch
            {
                TokenType.Text => () => text[index].IsTagStart<MarkdownTag>() ||
                                        text[index].IsNewLine() || text[index].IsEscapeCharacter(),
                TokenType.Tag => () => !(content + text[index]).IsPossibleTag<MarkdownTag>(),
                TokenType.LineBreak => () => content == "\n",
                TokenType.Escape => () => content == @"\",
                _ => () => true
            };
        }
    }
}
