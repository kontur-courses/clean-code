using System;

namespace Markdown
{
    public class TagSpace : IToken
    {
        public string Content => " ";

        public bool IsPrevent
        {
            get => false;

            set => throw new NotImplementedException();
        }
    }
}
