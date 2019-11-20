using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class Tag
    {
        public enum TagName
        {
            EmOpened,
            EmClosed,
            StrongOpened, 
            StrongClosed,
            DefaultOpened,
            DefaultClosed
        }
        
        public enum Markup
        {
            Html,
            Md
        }

        private static readonly Dictionary<string, string> htmlTags = new Dictionary<string, string>()
            {
                {"EmOpened", "<em>"}, 
                {"EmClosed", "</em>"},
                {"StrongOpened", "<strong>"},
                {"StrongClosed", "</strong>"},
                {"DefaultOpened", ""},
                {"DefaultClosed", ""}
            };

        private static readonly Dictionary<string, string> mdTags = new Dictionary<string, string>()
        {
            {"EmOpened", "_"}, 
            {"EmClosed", "_"},
            {"StrongOpened", "__"},
            {"StrongClosed", "__"},
            {"DefaultOpened", ""},
            {"DefaultClosed", ""}
        };

        public static string GetTag(TagName tagName, Markup markup)
        {
            if (markup == Markup.Html)
                return TryGetTag(tagName, htmlTags);
            if (markup == Markup.Md)
                return TryGetTag(tagName, mdTags);
            throw new ArgumentException();
        }

        private static string TryGetTag(TagName tagName, IReadOnlyDictionary<string, string> tags)
        {
            switch (tagName)
            {
                case TagName.DefaultOpened:
                case TagName.DefaultClosed:
                {
                    tags.TryGetValue("DefaultOpened", out var result);
                    return result;
                }
                case TagName.EmOpened:
                {
                    tags.TryGetValue("EmOpened", out var result);
                    return result;
                }
                case TagName.EmClosed:
                {
                    tags.TryGetValue("EmClosed", out var res);
                    return res;
                }
                case TagName.StrongOpened:
                {
                    tags.TryGetValue("StrongOpened", out var result);
                    return result;
                }
                case TagName.StrongClosed:
                {
                    tags.TryGetValue("StrongClosed", out var result);
                    return result;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(tagName), tagName, null);
            }
            
        }
    }
}