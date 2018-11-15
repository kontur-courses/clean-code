using System;

namespace Markdown
{
    public class TokenReader
    {
        private int position;
        private string line;

        public TokenReader(int position, string line)
        {
            throw new NotImplementedException();
        }

        Token ReadUntil(Func<string, bool> isStopString)
        {
            throw new NotImplementedException();
        }
    }
}
