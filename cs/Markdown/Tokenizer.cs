using Markdown.Interfaces;
using Markdown.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class Tokenizer : IParser
    {
        private readonly List<IToken> bufferTagTokens;

        public Tokenizer()
        {
            bufferTagTokens = new List<IToken>();
        }

        public IEnumerable<IToken> Parse(string markDownText)
        {
            bufferTagTokens.Clear();
            if (string.IsNullOrEmpty(markDownText))
            {
                return bufferTagTokens;
            }

            var pointerToCurrentChar = 0;

            while (pointerToCurrentChar < markDownText.Length)
            {
                CreateToken(markDownText, ref pointerToCurrentChar);
            }

            CheckHeader();
            FindCloseTag();
            return bufferTagTokens;
        }

        private void CreateToken(string markDownText, ref int pointer)
        {
            if (pointer + 1 < markDownText.Length)
            {
                if (CheckDoubleTag(markDownText, ref pointer))
                    return;
            }

            if (pointer <= markDownText.Length)
            {
                if (CheckSingleTag(markDownText, ref pointer))
                    return;

                CheckTextTag(markDownText, ref pointer);
            }
        }

        private bool CheckDoubleTag(string markDownText, ref int pointer)
        {
            var tmpTag = markDownText[pointer].ToString() + markDownText[pointer + 1].ToString();

            if (IsTagCharacter(tmpTag))
            {
                if (CreateHeaderTag(markDownText, ref pointer))
                    return true;

                bufferTagTokens.Add(TokenFactory.CreateToken(tmpTag));
                pointer += 2;
                return true;
            }

            return false;
        }

        private bool CheckSingleTag(string markDownText, ref int pointer)
        {
            if (IsTagCharacter(markDownText[pointer].ToString()))
            {
                bufferTagTokens.Add(TokenFactory.CreateToken(markDownText[pointer].ToString()));
                pointer++;
                return true;
            }

            return false;
        }

        private bool CheckTextTag(string markDownText, ref int pointer)
        {
            var bufferBuilderText = new StringBuilder();
            while (pointer < markDownText.Length && !IsTagCharacter(markDownText[pointer].ToString()))
            {
                if (markDownText[pointer] == ' ')
                {
                    if (bufferBuilderText.Length > 0)
                    {
                        bufferTagTokens.Add(new TextToken() { Value = bufferBuilderText.ToString() });
                        return true;
                    }

                    bufferBuilderText.Append(markDownText[pointer].ToString());
                    bufferTagTokens.Add(new TextToken() { Value = bufferBuilderText.ToString() });
                    pointer++;
                    return true;
                }
                bufferBuilderText.Append(markDownText[pointer].ToString());
                pointer++;
            }

            bufferTagTokens.Add(new TextToken() { Value = bufferBuilderText.ToString() });
            return true;
        }

        private void CheckHeader()
        {
            if (bufferTagTokens.First().GetType() == typeof(HeaderToken))
            {
                bufferTagTokens.First().IsOpenTag = true;
                bufferTagTokens.Add(TokenFactory.CreateToken("# "));
            }
        }

        // TODO доработать если не нашел отктые или конец
        private void FindCloseTag()
        {
            var indexEnd = bufferTagTokens.Count - 1;

            for (var i = 0; i <= bufferTagTokens.Count / 2; i++)
            {
                if (bufferTagTokens[i] is TextToken)
                    continue;

                bufferTagTokens[i].IsOpenTag = true;

                while (bufferTagTokens[indexEnd].GetType() != bufferTagTokens[i].GetType() && (indexEnd > i) &&
                       indexEnd > 0)
                {
                    indexEnd--;
                }

                if (indexEnd == i)
                    bufferTagTokens[indexEnd].IsOpenTag = false;
            }
        }

        private bool IsTagCharacter(string c)
        {
            if (c == BoldToken.MarkTag || c == ItalicToken.MarkTag || c == HeaderToken.MarkTag)
            {
                return true;
            }

            return false;
        }

        private bool CreateHeaderTag(string markDownText, ref int pointer)
        {
            var tmpTag = markDownText[pointer].ToString() + markDownText[pointer + 1].ToString();

            if (tmpTag == HeaderToken.MarkTag)
            {
                if (bufferTagTokens.Count == 0)
                {
                    bufferTagTokens.Add(TokenFactory.CreateToken(tmpTag));
                    pointer += 2;
                    return true;
                }

                bufferTagTokens.Add(new TextToken() { Value = markDownText[pointer].ToString() });
                pointer++;
                return true;
            }

            return false;
        }
    }
}