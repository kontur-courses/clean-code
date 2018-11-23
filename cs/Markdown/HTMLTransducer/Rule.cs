using System;

namespace Markdown.HTMLTransducer
{
    public class Rule
    {
        private readonly Token token;
        private readonly string openTag;
        private readonly string closeTag;
        private readonly bool doubleTagged;
        
        public Rule(Token token, string openTag, string closeTag = null, 
            bool doubleTagged = false)
        {
            this.token = token;
            this.openTag = openTag;
            this.doubleTagged = doubleTagged;
            
            if (doubleTagged)
                this.closeTag = closeTag ?? openTag.Insert(1, "/");
        }

        public bool For(Token token) =>
            Equals(this.token, token);
        
        public Token Perform(Token token, bool isClosed) =>
            new Token(isClosed ? closeTag : openTag, false);
    }
}