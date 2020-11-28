using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Markdown.TokenInfo;

namespace Markdown
{
    public class TokenReader
    {
        private readonly Dictionary<TagType, ITokenInfo> _tokenInfos = new Dictionary<TagType, ITokenInfo>
        {
            {TagType.Bold, new BoldTokenInfo()},
            {TagType.Heading, new HeadingTokenInfo()},
            {TagType.Italics, new ItalicsTokenInfo()},
            {TagType.Text, new TextTokenInfo()},
            {TagType.EntireText, new EntireTextTokenInfo()}
        };


        public TokenReader()
        {
            _tokenInfos = _tokenInfos.OrderByDescending(x => x.Value.NestedTypes.Length).ToDictionary(x => x.Key, x => x.Value);
        }

        public IEnumerable<Token> ReadTokens(string text)
        {
            var tokens = new List<Token>();
            var root = new Token(TagType.EntireText, 0, text.Length - 1);
            var queue = new Queue<Token>();
            queue.Enqueue(root);
            while (queue.Any())
            {
                var currentToken = queue.Dequeue();
                tokens.Add(currentToken);
                var tokenInfoCurrentToken = _tokenInfos[currentToken.TagType];

                if (tokenInfoCurrentToken.NestedTypes.Length <= 0) continue;

                var startIndex = tokenInfoCurrentToken.GetValueStartIndex(currentToken.Start);
                var finishIndex = tokenInfoCurrentToken.GetValueFinishIndex(currentToken.Finish);
                while (startIndex < finishIndex)
                {
                    foreach (var tokenInfo in _tokenInfos.Where(x => x.Key != TagType.EntireText).ToDictionary(x => x.Key, x => x.Value).Values)
                    {
                        if (tokenInfo.TryReadToken(startIndex, finishIndex, text, out var token))
                        {
                            queue.Enqueue(token);
                            currentToken.NestedTokens.Add(token);
                            startIndex = token.Finish;
                            break;
                        }
                    }
                }
            }

            return tokens.ToArray();
        }
    }
}
