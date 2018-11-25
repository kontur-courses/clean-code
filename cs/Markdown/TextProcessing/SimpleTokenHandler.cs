using Markdown.TokenEssences;
using System;

namespace Markdown.TextProcessing
{
    public class SimpleTokenHandler : ITokenHandler
    {
        public string TokenAssociation { get; set; }
        public char StopChar { get; set; }
        public bool IsNestedToken { get; set; }

        public SimpleTokenHandler()
        {
            TokenAssociation = "";
            StopChar = '_';
        }

        public virtual TypeToken GetNextTypeToken(string content, int position)
        {
            var emToken = new EmTokenHandler();
            var strongToken = new StrongTokenHandler();
            if (strongToken.IsStartToken(content, position))
                return TypeToken.Strong;
            if (emToken.IsStartToken(content, position))
                return TypeToken.Em;
            return TypeToken.Simple;
        }

        public virtual bool IsStopToken(string content, int position)
        {
            var emTokenHandler = new EmTokenHandler();
            var strongToken = new StrongTokenHandler();
            return emTokenHandler.IsStartToken(content, position) || strongToken.IsStartToken(content, position);
        }

        public virtual bool IsStartToken(string content, int position)
        {
            return false;
        }

        public virtual bool ContainsNestedToken(string content, int position)
        {
            return false;
        }

        public virtual ITokenHandler GetNextNestedToken(string content, int position)
        {
            return new SimpleTokenHandler();
        }
    }
}