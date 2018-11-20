using System;

namespace Markdown
{
    internal interface IPairTag
    {
        string StartTag
        {
            get;
        }

        string EndTag
        {
            get;
        }


        Func<Tag, bool> CanIContainThisTagRule { get; }
    }
}
