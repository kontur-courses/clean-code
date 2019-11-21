using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Token
    { 
        internal List<Token> NestedTokens { get; } = new List<Token>();
        internal string MdTag { get; }
        internal string HtmlTagName { get; }
        internal string Data { get; private set; }
        internal int Position { get; }

        /// <summary>
        /// Token is invalid when its parent doesn't support nesting.
        /// </summary>
        internal bool IsValid { get; set; }

        internal bool IsClosed { get; set; }

        public Token(int position, string mdTag, string htmlTagName, string data = "", bool isClosed = false,
            bool isValid = false)
        {
            Position = position;
            MdTag = mdTag;
            HtmlTagName = htmlTagName;
            Data = data;
            IsClosed = isClosed;
            IsValid = isValid;
        }

        public void AddNestedToken(Token token)
        {
            NestedTokens.Add(token);
        }

        public Token GetLastNestedToken()
        {
            return NestedTokens.Count == 0
                ? null
                : NestedTokens[NestedTokens.Count - 1];
        }

        public void RemoveLastNestedToken()
        {
            if (NestedTokens.Count != 0)
                NestedTokens.RemoveAt(NestedTokens.Count - 1);
        }

        public void AppendData(string data)
        {
            Data += data;
        }

        public string ToHtml()
        {
            var stringBuilder = new StringBuilder(Data);

            //inserting from the end, in other case we will need to store token length
            for (var i = 0; i < NestedTokens.Count; i++)
            {
                var tempToken = NestedTokens[NestedTokens.Count - 1 - i];
                stringBuilder.Insert(tempToken.Position, tempToken.ToHtml()); //should be pos not 0
                //Console.WriteLine(stringBuilder.ToString());
            }

            if (IsValid && IsClosed)
            {
                stringBuilder.Insert(0, GetHtmlTag(false));
                stringBuilder.Append(GetHtmlTag(true));
            }
            else if (!IsValid)
            {
                stringBuilder.Insert(0, MdTag);
                if (IsClosed)
                    stringBuilder.Append(MdTag);
            }
            
            return stringBuilder.ToString();
        }

        private string GetHtmlTag(bool isClosing)
        {
            if (HtmlTagName == "")
                return HtmlTagName;
            return isClosing
                ? "</" + HtmlTagName + ">"
                : "<" + HtmlTagName + ">";
        }
    }
}