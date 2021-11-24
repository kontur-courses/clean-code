﻿using System.Collections.Generic;
using System.Linq;
using Markdown.Models;
using Markdown.Paragraphs;

namespace Markdown.Tokens
{
    public static class MarkdownRenderConfigFactory
    {
        public static IEnumerable<IToken> GetTokens()
        {
            yield return new ItalicToken();
            yield return new BoldToken();
            yield return new HeaderToken();
            yield return new ListToken();
        }

        public static Dictionary<ParagraphType, IParagraphGroup> GetParagraphGroups()
        {
            var groups = new List<IParagraphGroup> {new UnorderedListParagraphGroup()};
            return groups.ToDictionary(group => group.Type, group => group);
        }
    }
}