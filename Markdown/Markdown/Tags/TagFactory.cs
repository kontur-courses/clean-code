using System;
using System.Collections.Concurrent;

namespace Markdown
{
    internal static class TagFactory
    {
        private static readonly ConcurrentDictionary<string, Tag> Tags = new();
        
        public static Tag GetOrAddSingleTag(string tag)
        {
            return Tags.GetOrAdd(tag, new Tag(tag, null));
        }
        
        public static Tag GetOrAddSymmetricTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag)) 
                throw new ArgumentException("Tag word can not be null, empty string or white space");
            return Tags.GetOrAdd(tag, new Tag(tag, tag));
        }
        
        public static Tag GetOrAddPairTag(string startTag, string endTag)
        {
            if (string.IsNullOrWhiteSpace(startTag) || string.IsNullOrWhiteSpace(endTag)) 
                throw new ArgumentException("Tag word can not be null, empty string or white space");
            return Tags.GetOrAdd(startTag, new Tag(startTag, endTag));
        }

        public static Tag GetTagByChars(string startChars)
        {
            if (!Tags.ContainsKey(startChars))
                throw new ArgumentException($"Tag {startChars} is not registered"); 
            return Tags[startChars];
        }
    }
}