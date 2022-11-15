using System.Collections.Generic;

namespace Markdown
{
    public class Concatination
    {
        public List<Concatination> innerConcs;
        public List<Token> tokens;
        public ConcType concType;
        public int startPos;

        public Concatination(ConcType type, int startPos)
        {
            innerConcs = new List<Concatination>();
            tokens = new List<Token>();
            concType = type;
            this.startPos = startPos;
        }

        public void AddTokens(Token token)
        {
            tokens.Add(token);
        }
    }
}
