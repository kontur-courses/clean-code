using System;

namespace Markdown
{
    public static class TagCollectorFactory
    {
        private static readonly char[] BeginingOfMdTags = {'_', '#', '*'};
        public static readonly TextWorker MdTextWorker = new TextWorker(BeginingOfMdTags);
        
        public static TagCollector<TTag> CreateCollectorFor<TTag>()
            where TTag : Tag, new()
        {
            return typeof(TTag) switch
            {
                var t when t == typeof(ItalicTag) => new TextStyleTagCollector<TTag>(MdTextWorker),
                var t when t == typeof(BoldTag) => new TextStyleTagCollector<TTag>(MdTextWorker),
                var t when t == typeof(HeaderTag) => new ParagraphStyleTagCollector<TTag>(MdTextWorker),
                var t when t == typeof(UnorderedListTag) => new ParagraphStyleTagCollector<TTag>(MdTextWorker),
                _ => throw new ArgumentException()
            };
        }
    }
}