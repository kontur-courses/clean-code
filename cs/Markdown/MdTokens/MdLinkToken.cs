namespace Markdown
{
    public class MdLinkToken : Token
    {
        public MdLinkTextToken TextToken;
        public MdLinkUrlToken UrlToken;

        public MdLinkToken(int position, int length = 0, Token parent = null) : base(position, length, parent)
        {
        }

        public class MdLinkTextToken : BasicToken
        {
            public MdLinkTextToken() : base(0, 0, null)
            {
            }
        }

        public class MdLinkUrlToken : BasicToken
        {
            public MdLinkUrlToken() : base(0, 0, null)
            {
            }
        }
    }
}