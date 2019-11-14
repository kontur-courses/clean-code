using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkDown.TokenDeclarations
{
    public class DeclarationGetter
    {

        private Dictionary<TokenType, TokenDeclaration> tokenDeclarations;

        public DeclarationGetter()
        {
            tokenDeclarations = new Dictionary<TokenType, TokenDeclaration>()
            {
                {TokenType.EM, new EMDeclaration() },
                {TokenType.Strong, new StrongDeclaration() },
            };
        }

        public List<TokenDeclaration> GetTokenDeclarations() => tokenDeclarations.Values.ToList();

        public TokenDeclaration GetDeclarationFromType(TokenType type) => tokenDeclarations[type];

    }
}
