using System;

namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
          throw  new NotImplementedException();
        }
    }

    internal static class MarkdownParser
    {
        public static Token ReadItalicToken(string line, int startIndex)
        {
            throw  new NotImplementedException();
        }
        
        public static Token ReadBoldToken(string line, int startIndex)
        {
            throw  new NotImplementedException();
        }
        
        public static Token ReadHeaderToken(string line, int startIndex)
        {
            throw  new NotImplementedException();
        }

        public static int SkipNotStyleWords(string line, int startIndex)
        {
            throw  new NotImplementedException();
        }
    } 
}