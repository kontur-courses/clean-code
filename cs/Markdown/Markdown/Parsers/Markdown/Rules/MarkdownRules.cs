using Markdown.Parsers.Markdown.Rules.TagsRules;
using Markdown.Tags;
using Markdown.Tags.TagsContainers;
using Markdown.Tokens;
using System.Collections.Immutable;
using System.Diagnostics;

namespace Markdown.Parsers.Markdown.Rules
{
    public static class MarkdownRules
    {
        public static char EmptyChar { get; } = '\0';
        private static readonly ImmutableDictionary<TagDefinition, string> _tags = MarkdownTagsContainer.GetTags();
        private static readonly HashSet<string> _singleTags = new() { _tags[TagDefinition.Header] };

        private static readonly Dictionary<string, HashSet<string>> _tagsToIgnoredNestedTags = new()
        {
                {_tags[TagDefinition.Italic], new HashSet<string>(){ _tags[TagDefinition.Strong] } }
        };

        private static readonly Dictionary<string, Func<char, bool>> _tagsFuncThatReturnsIFSymbolMakesThemIgnored = new()
        {
                {_tags[TagDefinition.Italic],  ItalicTagRules.IsItalicTagIgnoredBySymbol},
                {_tags[TagDefinition.Strong],  StrongTagRules.IsStrongTagIgnoredBySymbol}
        };

        private static readonly Dictionary<string, Func<char, char, bool>> _tagsToFuncThatReturnsAreTheyOpen = new()
        {
                {_tags[TagDefinition.Italic], ItalicTagRules.IsItalicTagOpen},
                {_tags[TagDefinition.Strong],  StrongTagRules.IsStrongTagOpen}
        };

        private static readonly Dictionary<string, Func<char, char, bool>> _tagsToFuncThatReturnsAreTheyClosing = new()
        {
                {_tags[TagDefinition.Italic], ItalicTagRules.IsItalicTagClosing},
                {_tags[TagDefinition.Strong],  StrongTagRules.IsStrongTagClosing}
        };

        private static readonly Dictionary<string, Func<TagToken, TagToken, Dictionary<int, TagToken>, bool>> 
            _TagsToFuncThatReturnAreTheyPaired = new()
        {
            {_tags[TagDefinition.Italic], ItalicTagRules.IsItalicTagsPaired},
            {_tags[TagDefinition.Strong], StrongTagRules.IsStrongTagsPaired}
        };

        private static readonly Dictionary<string, Func<TagToken, TagToken, bool>> _TagsToFuncThatReturnAreTheyIgnored = new()
        {
            {_tags[TagDefinition.Italic], ItalicTagRules.IsItalicTagsIgnored},
            {_tags[TagDefinition.Strong], StrongTagRules.IsStrongTagsIgnored}
        };

        public static HashSet<string> GetIgnoredNestedTags(string tag)
        {
            if (_tagsToIgnoredNestedTags.ContainsKey(tag))
                return _tagsToIgnoredNestedTags[tag];
            return new HashSet<string>();
        }

        public static bool IsTagIgnoredBySymbol(string tag, char symbol)
        {
            if (!_tagsFuncThatReturnsIFSymbolMakesThemIgnored.ContainsKey(tag))
                return false;

            var function = _tagsFuncThatReturnsIFSymbolMakesThemIgnored[tag];
            return function(symbol);
        }

        public static bool IsTagOpen(string tag, char previousSymbol, char nextSymbol)
        {
            if (!_tagsToFuncThatReturnsAreTheyOpen.ContainsKey(tag))
                return false;

            var function = _tagsToFuncThatReturnsAreTheyOpen[tag];
            return function(previousSymbol, nextSymbol);
        }

        public static bool IsTagClosing(string tag, char previousSymbol, char nextSymbol)
        {
            if (!_tagsToFuncThatReturnsAreTheyClosing.ContainsKey(tag))
                return false;

            var function = _tagsToFuncThatReturnsAreTheyClosing[tag];
            return function(previousSymbol, nextSymbol);
        }

        public static bool IsTagSingle(string tag) =>
            _singleTags.Contains(tag);

        public static bool IsTagsPaired(TagToken firstTag, TagToken secondTag, Dictionary<int, TagToken> parsedTokens)
        {
            if (firstTag.ToString() != secondTag.ToString())
                return false;
            if (!_TagsToFuncThatReturnAreTheyPaired.ContainsKey(firstTag.ToString()))
                return false;

            var function = _TagsToFuncThatReturnAreTheyPaired[firstTag.ToString()];
            return function(firstTag, secondTag, parsedTokens);
        }

        public static bool IsTagsIgnored(TagToken firstTag, TagToken secondTag)
        {
            if(firstTag.ToString() != secondTag.ToString())
                return false;
            if(!_TagsToFuncThatReturnAreTheyIgnored.ContainsKey(firstTag.ToString()))
                return false;

            var function = _TagsToFuncThatReturnAreTheyIgnored[firstTag.ToString()];
            return function(firstTag, secondTag);
        }

        public static bool IsTagsInsideOneWord(TagToken firstTag, TagToken secondTag, Dictionary<int, TagToken> parsedTokens)
        {
            var nextTokenAfterFirstTag = parsedTokens[firstTag.EndIndex + 1];
            return nextTokenAfterFirstTag.EndIndex == secondTag.StartIndex - 1
                   && !nextTokenAfterFirstTag.ToString().Contains(' ');
        }
    }
}
