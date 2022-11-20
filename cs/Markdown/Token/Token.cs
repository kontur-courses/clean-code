using System;

namespace Markdown
{
    public class Token<TTag> : IToken<TTag>
    {
        public TTag Tag => throw new NotImplementedException();

        public TagState TagState => throw new NotImplementedException();

        public string Content => throw new NotImplementedException();
    }
}
