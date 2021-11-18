﻿namespace Markdown.Models
{
    public interface ITagConverter
    {
        public string HtmlOpenTag { get; init; }
        public string HtmlCloseTag { get; init; }
        public int TrimFromStartCount { get; init; }
        public int TrimFromEndCount { get; init; }
    }
}