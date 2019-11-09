using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TokenReader
    {
        private readonly string text;
        private Dictionary<TokenType, TokenDescription> tokenTypeToDescription;
        private List<TokenDescription> tokenRules;
        private Token rootToken;

        public TokenReader(string text, IEnumerable<TokenDescription> tokenRules)
        {
            this.text = text;
            this.tokenRules = tokenRules.OrderByDescending(descr => descr.tag.Length).ToList();
            tokenTypeToDescription = this.tokenRules.ToDictionary(descr => descr.tokenType);
        }

        public Token TokenizeText()
        {
            if (rootToken != null)
                return rootToken;
            return rootToken = ReadToken(tokenRules, new List<TokenDescription>{ tokenRules.Last()});
            
        }

        // пока не совсем понимаю, как должен выглядеть сам метод разбиения на токены
        private Token ReadToken(
            List<TokenDescription> openingTokens, List<TokenDescription> closingTokens)
        {
            throw new NotImplementedException();
        }
    }
}
