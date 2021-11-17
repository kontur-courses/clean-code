using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Token
    {
        public readonly string Value;
        public List<Token> InnerTokens;

        protected Token(string value, List<Token> innerTokens)
        {
            Value = value;
            InnerTokens = innerTokens;
        }

        // простой текст, не обернутый в теги (токен по умолчанию)
        // для сложных токенов - переопределение с  использованием InnerTokens 
        public virtual string ToHtml()
        {
            return EscapeSymbols(Value);
        }

        //Экранирование, вызов в ToHtml() чтобы экранировать Value
        protected string EscapeSymbols(string text)
        {
            throw new NotImplementedException();
        }
    }
    
    public class Header : Token
    {
        public Header(string value, List<Token> innerTokens = null) :
            base(value, innerTokens)
        {
        }

        public override string ToHtml()
        {
            throw new NotImplementedException();
        }
    }

    public class Paragraph : Token
    {
        public Paragraph(string value, List<Token> innerTokens = null) :
            base(value, innerTokens)
        {
        }

        public override string ToHtml()
        {
            throw new NotImplementedException();
        }
    }

    public class StrongText : Token
    {
        public StrongText(string value, List<Token> innerTokens = null) :
            base(value, innerTokens)
        {
        }

        public override string ToHtml()
        {
            throw new NotImplementedException();
        }
    }

    public class ItalicText : Token
    {
        public ItalicText(string value, List<Token> innerTokens = null) :
            base(value, innerTokens)
        {
        }

        public override string ToHtml()
        {
            throw new NotImplementedException();
        }
    }
}
