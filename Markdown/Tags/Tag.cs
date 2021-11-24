using System;
using System.Collections.Concurrent;

namespace Markdown
{
    internal class Tag
    {
        private static readonly ConcurrentDictionary<string, Tag> Tags = new();
        private readonly string start;

        public string Start { get; }
        public string End { get; }

        private Tag() {}

        private Tag(string openWord, string closeWord = null)
        {
            start = openWord;

            Start = start;
            if (closeWord is not null) End = closeWord;
        }

        public static Tag GetOrAddSingleTag(string tag)
        {
            return Tags.GetOrAdd(tag, new Tag(tag));
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

        public override bool Equals(object obj)
        {
            return obj is Tag tag && Equals(tag);
        }

        private bool Equals(Tag other)
        {
            return start == other.start;
        }

        public override int GetHashCode()
        {
            return start.GetHashCode();
        }
    }
}