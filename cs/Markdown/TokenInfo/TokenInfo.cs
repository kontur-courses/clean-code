using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class TokenInfo
    {
        public Dictionary<string, string> MdToHtmlTags => new Dictionary<string, string>
        {
            {"_", "em"},
            {"__", "strong"},
            {"`", "code"},
            {"```", "code"},
            {"pre", "pre"},  // ``` wrapper
            {"#", "h1"},
            {"##", "h2"},
            {"###", "h3"},
            {"####", "h4"},
            {"#####", "h5"},
            {"######", "h6"},
            {">", "blockquote"},
            {"-", "li"},
            {"ul", "ul"}  // - wrapper
        };

        public static Dictionary<string, string> MdPairTags => new Dictionary<string, string>
        {
            {"_", "_"},
            {"__", "__"},
            {"`", "`"},
            {"```", "```"},
            {"pre", "pre"},  // ``` wrapper
            {">", "\n\n"},
            {"#", "\n"},
            {"##", "\n"},
            {"###", "\n"},
            {"####", "\n"},
            {"#####", "\n"},
            {"######", "\n"},
            {"-","\n"},
            {"ul", "ul"}  // - wrapper
        };

        // tags that need to be wrapped in some other openTag (key is the html wrapping openTag)
        public Dictionary<string, string> TagsThatRequireExtraWrapping => new Dictionary<string, string>
        {
            {"```", "pre"},
            {"-", "ul"},
        };

        private HashSet<string> WrappingTags => new HashSet<string>{ "pre","ul"};
        
        private HashSet<string> TagsThatRequireSpaceAfterOpenTag => new HashSet<string>
            {"#", "##", "###", "####", "#####", "######", "-",">"};

        public HashSet<string> TagsThatRequireSpaceAfterCloseTag => new HashSet<string>
            {"_","__","`","```"};
        
        private HashSet<string> TagsThatRequireNewLineBeforeOpenTag => new HashSet<string>
            {">", "#", "##", "###", "####", "#####", "######" , "-"};

        private Dictionary<string, List<string>> MdAcceptedNestedTags =>
            new Dictionary<string, List<string>>
            {
                {"", new List<string> {"_", "__", "`", "```", ">", "#", "##", "###", "####", "#####", "######", "pre", "-", "ul"}},
                {"_", new List<string>()},
                {"__", new List<string> {"_"}},
                {"`", new List<string> {"_", "__"}},
                {"```", new List<string> {"_", "__"}},
                {"pre", new List<string> {"```"}}, // ``` wrapper
                {">", new List<string> {"_", "__", ">","`"}},
                {"#", new List<string>()},
                {"##", new List<string>()},
                {"###", new List<string>()},
                {"####", new List<string>()},
                {"#####", new List<string>()},
                {"######", new List<string>()},
                {"-", new List<string> {"_", "__"}},
                {"ul", new List<string> {"-"}}, // - wrapper
            };

        public bool IsTagPart(char symbol)
        {
            return (IsOpenTagPart(symbol) || IsOnlyCloseTagPart(symbol)) && !IsWrappingTagPart(symbol);
        }
        
        private static bool IsOpenTagPart(char symbol)
        {
            return MdPairTags.Keys.Any(k => k.StartsWith(symbol.ToString()));
        }

        public bool IsOnlyCloseTagPart(char symbol)
        {
            return !IsOpenTagPart(symbol)
                   && MdPairTags.Values.Any(k => k.StartsWith(symbol.ToString()));
        }
        
        private bool IsWrappingTagPart(char symbol)
        {
            return WrappingTags.Any(s => s.StartsWith(symbol.ToString()));
        }

        public bool IsCorrectNestedTag(string parentTag, string nestedTag)
        {
            return MdAcceptedNestedTags.ContainsKey(parentTag) && MdAcceptedNestedTags[parentTag].Contains(nestedTag);
        }

        public bool IsSpaceAfterCloseTagRequired(string closeTag)
        {
            return TagsThatRequireSpaceAfterCloseTag.Contains(closeTag);
        }
        
        public bool IsSpaceAfterOpenTagRequired(string openTag)
        {
            return TagsThatRequireSpaceAfterOpenTag.Contains(openTag);
        }

        public bool IsNewLineBeforeOpenTagRequired(int tagEndPosition,string openTag)
        {
            return tagEndPosition-openTag.Length+1!=0 &&TagsThatRequireNewLineBeforeOpenTag.Contains(openTag);
        }

        public bool IsExtraWrappingRequired(string tag)
        {
            return TagsThatRequireExtraWrapping.ContainsKey(tag);
        }

        public string GetExtraWrappingHtmlTagName(string tag)
        {
            return TagsThatRequireExtraWrapping.ContainsKey(tag) ? TagsThatRequireExtraWrapping[tag] : "";
        }

        public bool HasTagsStartingWith(string word)
        {
            //symmetric tags are counted twice
            return MdPairTags.Keys.Any(k => k.StartsWith(word)) || MdPairTags.Values.Any(k => k.StartsWith(word));
            //return MdPairTags.Keys.Count(k => k.StartsWith(word)) + MdPairTags.Values.Count(k => k.StartsWith(word)) != 0;
        }

        public string GetCloseTag(string openTag)
        {
            if (MdPairTags.Keys.Contains(openTag))
                return MdPairTags[openTag];
            throw new Exception($"Open openTag {openTag} doesn't exist");
        }
    }
}