
using System;

namespace Markdown.Types
{
    public class SimpleToken : IToken
    {
        public int Position { get; set; }
        public int Length { get; set; }
        public string Value { get; set; }
        public TypeToken TypeToken { get; set; }
        public string TokenAssociation { get; set; }
        public Func<char, bool> IsStopChar { get; set; }

        public SimpleToken(int position, int length, string value)
        {
            Position = position;
            Length = length;
            Value = value;
            TypeToken = TypeToken.Simple;
            TokenAssociation = "";
            IsStopChar = stopChar => stopChar =='_';
        }

        public SimpleToken()
        {
            TypeToken = TypeToken.Simple;
            Value = "";
            TokenAssociation = "";
            IsStopChar = stopChar => stopChar == '_';
        }

        public virtual TypeToken GetNextTypeToken(string content, int position)
        {
            var emToken = new EmToken();
            var strongToken = new StrongToken();
            if (strongToken.IsStartToken(content, position))
                return TypeToken.Strong;
            if (emToken.IsStartToken(content, position))
                return TypeToken.Em;
            return TypeToken.Simple;
        }

        public virtual bool IsStopToken(string content, int position)
        {
            var emToken = new EmToken();
            var strongToken = new StrongToken();
            return emToken.IsStartToken(content, position) || strongToken.IsStartToken(content, position);
        }

        public virtual bool IsStartToken(string content, int position)
        {
            return false;
        }

        public virtual bool IsNestedToken(string content, int position)
        {
            return false;
        }

        public virtual IToken GetNextNestedToken(string content, int position)
        {
            return new SimpleToken();
        }
    }
}