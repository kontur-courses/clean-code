using System;

namespace Markdown
{
    interface IPairTag
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
