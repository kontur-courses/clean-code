using System;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public int Start { get; }
        public int Lenght => Value.Length;
        public string Value => valueBuilder.ToString();
        public string Tag { get; }

        private StringBuilder valueBuilder;

        public void Add(char c)
        {
            throw new NotImplementedException();
        }

        public static Token BuildNextToken(string text, int startIndex)
        {
            throw new NotImplementedException();
        }

        public string WrapTokenInTag()
        {
            throw new NotImplementedException();
        }
    }
}