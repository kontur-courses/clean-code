using System.Text;

namespace Markdown
{
    public class Token
    {
        public Tag Tag;
        public Token Parent;
        public int StartOpenMark;
        public int EndCloseMark;
        public Dictionary<string, bool> CustomBools;

        public Token(Tag tag, Token parent, int startOpenMark = -1, int endCloseMark = -1)
        {
            Tag = tag;
            Parent = parent;
            StartOpenMark = startOpenMark;
            EndCloseMark = endCloseMark;

            CustomBools = new Dictionary<string, bool>();
            if (tag == null!) return;
            foreach (var name in tag?.CustomBoolNames)
                CustomBools[name] = false;
        }

        public bool IntersectsWith(Token other)
        {
            return StartOpenMark > other.StartOpenMark && StartOpenMark < other.EndCloseMark && EndCloseMark > other.EndCloseMark
                   || other.StartOpenMark > StartOpenMark && other.StartOpenMark < EndCloseMark && other.EndCloseMark > EndCloseMark;
        }

        public bool CheckCharAndGetIsIncorrectToken(string text, int index)
        {
            return  Tag != null! && Tag.CharProcessingRule(Tag, this, text, index);
        }

        public void ChangeCustomBool(string boolName, bool state)
        {
            var cur = this;
            while (cur.Tag != null!)
            {
                if (!cur.CustomBools.ContainsKey(boolName))
                {
                    cur = cur.Parent;
                    continue;
                }
                if (cur.CustomBools[boolName] == state) break;

                cur.CustomBools[boolName] = true;
                cur = cur.Parent;
            }
        }

        public Token? TryOpenChildToken(Tag tag, string text, ref int startIndex)
        {
            Token child;
            if (tag.OpenMarkRule(tag, this, text, startIndex))
            {
                child = new Token(tag, this, startIndex);
                startIndex += tag.OpenMark.Length - 1;
                return child;
            }
            child = null!;
            return this;
        }

        public bool TryCloseToken(Tag tag, string text, ref int endIndex, List<Token> tokens)
        {
            if (!tag.CloseMarkRule(tag, this, text, endIndex))
                return false;

            var token = FindTokenWithTag(tag);
            if (token != null && tag.TokenCloseRule(tag, token, text, endIndex))
            {
                token.EndCloseMark = endIndex + tag.CloseMark.Length - 1;
                if (token.IntersectsWith(token.Parent))
                {
                    tokens.Remove(token.Parent);
                    token.Parent = token.Parent.Parent;
                    token.EndCloseMark = -1;
                    return false;
                }
                endIndex += tag.CloseMark.Length - 1;
                tokens.Add(token);
                return true;
            }

            return false;
        }

        private Token? FindTokenWithTag(Tag tag)
        {
            var cur = this;
            while (cur.Tag != null!)
            {
                if (cur.Tag.Equals(tag))
                    return cur;
                cur = cur.Parent;
            }

            return null!;
        }

        public string GetProperties(string text, ref int startInd)
        {
            if (Tag.PropertiesAndTheirReceiving == null!)
                return "";
            var properties = new StringBuilder();
            foreach (var pair in Tag.PropertiesAndTheirReceiving)
            {
                properties.Append(" ");
                var value = pair.Value(text, startInd);
                startInd += value.Length;
                properties.Append($"{pair.Key}=\"{value}\"");
            }

            startInd += 2;
            return properties.ToString();
        }
    }
}
