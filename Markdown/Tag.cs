using System;
using System.Collections.Concurrent;

namespace Markdown
{
    public class Tag
    {
        private static readonly ConcurrentDictionary<string, Tag> Tags = new();

        private readonly string start;
        private readonly string end;

        public Token Start { get; }
        public Token End { get; }

        private Tag() {}

        private Tag(string tagWord, string endWord = null)
        {
            start = tagWord;
            end = endWord;

            Start = new Token(start);
            End = new Token(end);
        }

        private static void CheckForValidityForRegistration(string startTag)
        {
            if (string.IsNullOrWhiteSpace(startTag))
                throw new ArgumentException($"Tag word can not be null, empty string or white space");
            if (Tags.ContainsKey(startTag))
                throw new ArgumentException($"Tag {startTag} is already registered"); 
        }

        public static Tag RegisterSingleTag(string tag)
        {
            CheckForValidityForRegistration(tag);
            Tags.TryAdd(tag, new Tag(tag));
            return Tags[tag];
        }
        
        public static Tag RegisterSymmetricTag(string tag)
        {
            CheckForValidityForRegistration(tag);
            Tags.TryAdd(tag, new Tag(tag, tag));
            return Tags[tag];
        }
        
        public static Tag RegisterPairTag(string startTag, string endTag)
        {
            CheckForValidityForRegistration(startTag);
            CheckForValidityForRegistration(endTag);
            Tags.TryAdd(startTag, new Tag(startTag, endTag));
            return Tags[startTag];
        }

        public static Tag GetTagByChars(string startChars)
        {
            if (!Tags.ContainsKey(startChars))
                throw new ArgumentException($"Tag {startChars} is not registered"); 
            return Tags[startChars];
        }

        public static void ClearTagBase()
        {
            Tags?.Clear();
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