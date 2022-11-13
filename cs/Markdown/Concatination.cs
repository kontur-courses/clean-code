using System.Collections.Generic;

namespace Markdown
{
    public class Concatination
    {
        public List<Concatination> innerConcs;
        List<Token> tokens;
        public ConcType concType;

        public Concatination(ConcType type)
        {
            innerConcs = new List<Concatination>();
            tokens = new List<Token>();
            concType = type;
        }

        public void AddTokens(Token token)
        {
            tokens.Add(token);
        }
    }
}
