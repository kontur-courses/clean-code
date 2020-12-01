using System;
using System.Collections.Generic;
using Markdown.TokenModels;

namespace Markdown.Core
{
    public static class TokenFactory
    {
        private static Dictionary<string, Func<string, int, IToken>> MdTagToTokenTable { get; }

        static TokenFactory()
        {
            MdTagToTokenTable = new Dictionary<string, Func<string, int, IToken>>
            {
                [BoldToken.MdTag] = (mdString, startIndex) => new BoldToken(mdString, startIndex),
                [ItalicToken.MdTag] = (mdString, startIndex) => new ItalicToken(mdString, startIndex),
                [LinkToken.MdTag] = (mdString, startIndex) => new LinkToken(mdString, startIndex)
            };
        }

        public static IToken CreateNewToken(string markdownTag, string tokenSource, int startIndex)
        {
            if (!MdTagToTokenTable.ContainsKey(markdownTag))
                throw new ArgumentException($"{markdownTag} is not supported!");

            return MdTagToTokenTable[markdownTag](tokenSource, startIndex);
        }
    }
}