using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Factories;
using Markdown.Markings;
using Markdown.Tokens;

namespace Markdown.Parsers
{
    public class MarkdownParser : IParser<IMarkingTree<MarkdownToken>>
    {
        private readonly ITokenFactory<MarkdownToken> tokenFactory;
        private readonly IMarkingTreeFactory<MarkdownToken> markingTreeFactory;

        public MarkdownParser(ITokenFactory<MarkdownToken> tokenFactory,
            IMarkingTreeFactory<MarkdownToken> markingTreeFactory)
        {
            this.tokenFactory = tokenFactory;
            this.markingTreeFactory = markingTreeFactory;
        }

        public IMarkingTree<MarkdownToken> Parse(string markdown)
        {
            var rootTokenValues = ParseTokens(markdown);

            var rootToken = tokenFactory.NewToken(TokenType.TreeRoot, null, rootTokenValues);

            return markingTreeFactory.NewMarking(rootToken);
        }

        private List<MarkdownToken> ParseTokens(string markdown)
        {
            var tokens = new List<MarkdownToken>();

            var index = 0;

            while (index < markdown.Length)
            {
                if (markdown[index] == '_')
                {
                    var substring = new StringBuilder();
                    while (!char.IsWhiteSpace(markdown[index]) && markdown[index] != '_')
                    {
                        substring.Append(markdown[index]);
                        ++index;
                    }

                    var word = tokenFactory.NewToken(TokenType.Word, substring.ToString(), null);

                    tokens.Add(tokenFactory.NewToken(TokenType.Tag, "_", new[] {word}));
                }
                else if (char.IsWhiteSpace(markdown[index]))
                {
                    tokens.Add(tokenFactory.NewToken(TokenType.Word, markdown[index].ToString(), null));
                }
                else
                {
                    var substring = new StringBuilder();
                    while (index < markdown.Length && !char.IsWhiteSpace(markdown[index]) && markdown[index] != '_')
                    {
                        substring.Append(markdown[index]);
                        ++index;
                    }

                    var word = tokenFactory.NewToken(TokenType.Word, substring.ToString(), null);

                    tokens.Add(word);
                }
            }

            return tokens;
        }
    }
}