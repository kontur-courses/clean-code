﻿namespace Markdown
{
    public class SimpleElement : IElement
    {
        public IElement Child { get; set; }

        public string Value { get;}

        public SimpleElement(Token token)
        {
            Value = token.Value;
        }
    }
}
