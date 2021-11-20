using System;

namespace Markdown
{
    public static class TagConverter
    {
        public static T2 Convert<T1, T2>(T1 from, Func<T1, T2> converter)
            where T1 : ITag
            where T2 : ITag
        {
            return converter(from);
        }
    }
}