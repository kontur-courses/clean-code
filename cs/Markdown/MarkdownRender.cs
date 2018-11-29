using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class MarkdownRender
    {
        static private readonly List<Tag> tags = new List<Tag>()
        {
            new Tag("_", "<em>", "</em>"),
            new Tag("__", "<strong>", "</strong>")
        };
        static public IReadOnlyList<Tag> Tags => tags.AsReadOnly();

        private readonly Stack<Token> stack;
        private readonly List<Tag> usedTags;
        private readonly string markdown;




        public MarkdownRender(string markdown)
        {
            this.stack = new Stack<Token>();
            this.usedTags = new List<Tag>();
            this.markdown = markdown;
        }

        public List<Token> GetTokens()
        {
            var tokenList = new List<Token> ();
            var reader = new TokenReader(Tags, markdown);
            var currentPos = 0;
            while (currentPos < markdown.Length)
            {
                var token = reader.TryReadUpToNextTag(currentPos);
                if (token.Tag != null)
                {
                    if (usedTags.Contains(token.Tag))
                    {
                        var newToken = TryGetToken(token.Tag);
                        if (newToken != null)
                            tokenList.Add(newToken);
                    }
                    else
                    {
                        
                    }
                }
                currentPos = token.Start;                
            }
            return tokenList;
        }

        

        private void AddTokenToStack(Token token)
        {
            if (IsValidStartEntry(markdown, token.Start, token.End))
            {
                usedTags.Add(token.Tag);
                stack.Push(token);
            }
        }

        

        private bool IsValidStartEntry(string line, int entryStart, int entryEnd)
        {
            if (line.Length <= entryEnd + 1) return false;
            if (line[entryEnd + 1] == ' ') return false;
            if (!IsValidEntry(line, entryStart, entryEnd)) return false;
            return true;
        }

        private bool IsValidEndEntry(string line, int entryStart, int entryEnd)
        {
            if (entryStart == 0) return false;
            if (line[entryStart - 1] == ' ') return false;
            if (!IsValidEntry(line, entryStart, entryEnd)) return false;
            return true;
        }

        private bool IsDigitNextToTag(string line, int tagStart, int tagEnd)
        {
            return tagStart - 1 >= 0 &&
                    char.IsDigit(line[tagStart - 1]) &&
                    tagEnd + 1 < line.Length &&
                    char.IsDigit(line[tagEnd + 1]);
        }

        private bool IsSlashBeforeTag(string line, int tagStart)
        {
            return tagStart - 1 >= 0 && line[tagStart - 1] == '\\';
        }

        private bool IsValidEntry(string line, int tagStart, int tagEnd)
        {

            if (IsDigitNextToTag(line, tagStart, tagEnd)) return false;

            if (tagStart - 1 >= 0 &&
                line[tagStart - 1] == '_' ||
                tagEnd + 1 < line.Length &&
                line[tagEnd + 1] == '_') return false;

            return !IsSlashBeforeTag(line, tagStart);
        }
    }
}
}
