namespace Markdown
{
    public class MdToken : IMdToken
    {        
        public int Position { get; }
        public int Length { get; }
        public string Value { get; }
        public MdTokenType TokenType { get; set; }

        protected MdToken(int position, int length, string value, MdTokenType tokenType)
        {
            Position = position;
            Length = length;
            Value = value;
            TokenType = tokenType;
        }
    }
}