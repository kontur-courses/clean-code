using System;

namespace Markdown
{
    class Tag
    {
        public readonly string Opening;
        public readonly string Ending;

        public Tag(string openeing, string ending)
        {
            Opening = openeing;
            Ending = ending;
        }

        public string GetTagValue(TagRole role)
        {
            if (role == TagRole.Opening)
                return Opening;
            if (role == TagRole.Ending)
                return Ending;
            throw new ArgumentException();
        }
    }
}
