﻿using Markdown.Models;

namespace Markdown.Tokens
{
    public class TagConverter : ITagConverter
    {
        public string OpenTag { get; init; }
        public string CloseTag { get; init; }
        public int GetTrimFromStartCount { get; init; }
        public int GetTrimFromEndCount { get; init; }
    }
}