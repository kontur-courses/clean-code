using System;

namespace Markdown
{
    class TagWithToken
    {
        public Tag Tag { get; }
        public Token Token { get; }
        public bool IsOpen { get; set; }
        public bool IsClose { get; set; }
        public bool IsTag { get { return Tag != null; } }

        public TagWithToken(Tag tag, Token token)
        {
            Tag = tag;
            Token = token;
            IsOpen = false;
            IsClose = false;
        }

        public bool CanTagBeClosing(string text)
        {
            var index = Token.Index - 1;
            if (index < 0 || text[index] == ' ')
                return false;
            return true;
        }

        public bool CanTagBeOpening(string text)
        {
            var index = Token.Index + Token.Length;
            if (index >= text.Length || text[index] == ' ')
                return false;
            return true;
        }

        public bool CanBeTag(string text)
        {
            int num;
            switch (Tag.MarkdownTag)
            {
                case "_":
                    if (Token.Index + Token.Length < text.Length &&
                        int.TryParse(text[Token.Index + Token.Length].ToString(), out num) &&
                        Token.Index - 1 >= 0 &&
                        int.TryParse(text[Token.Index - 1].ToString(), out num))
                    {
                        return false;
                    }
                    break;
                case "__":
                    if (Token.Index + Token.Length < text.Length &&
                        int.TryParse(text[Token.Index + Token.Length].ToString(), out num) &&
                        Token.Index - 1 >= 0 &&
                        int.TryParse(text[Token.Index - 1].ToString(), out num))
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }
    }
}
