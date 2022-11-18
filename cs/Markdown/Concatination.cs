using System.Collections.Generic;

namespace Markdown
{
    public class Concatination
    {
        public List<Concatination> innerConcs;
        public List<Token> tokens;
        public Mod concType;
        public int startPos;
        public bool IsClosed;

        public Concatination(Mod type, int startPos)
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
