using System;

namespace Markdown
{
    public class Token
    {
        private string value;
        
        public bool Pair => throw new NotImplementedException();
        public bool Symmetric => throw new NotImplementedException();
        public int Lenght => throw new NotImplementedException();
        
        private Token(){}

        public static Token GetSingleToken(string token)
        {
            throw new NotImplementedException();
        }
        
        public static Token GetSymmetricToken(string token)
        {
            throw new NotImplementedException();
        }
        
        public static Token GetPairToken(string token, Token endToken = null)
        {
            throw new NotImplementedException();
        }
        
        public override string ToString()
        {
            return value;
        }
    }
}