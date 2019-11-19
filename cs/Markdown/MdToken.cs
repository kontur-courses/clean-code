using System.Collections.Generic;

namespace Markdown
{
    /// <summary>
    ///     Token data container
    /// </summary>
    public class MdToken
    {
        public readonly int Pos;
        public readonly string Value;
        public MdTokenMark Mark;
        public bool Skipped;

        public MdToken(int position, string value, MdTokenMark mark)
        {
            Pos = position;
            Value = value;
            Mark = mark;
        }

        public void SetSkipped(bool skipped = true)
        {
            Skipped = skipped;
        }
    }

    public static class TokenListExtension
    {
        public static IEnumerable<MdToken> GetSubTokens(this List<MdToken> tokenList, MdToken tagLeftBorder,
            MdToken tagRightBorder)
        {
            var leftBorderIndex = tokenList.IndexOf(tagLeftBorder);
            if (leftBorderIndex < 0)
                yield break;
            for (var i = leftBorderIndex + 1; i < tokenList.Count && tokenList[i] != tagRightBorder; i++)
                yield return tokenList[i];
        }
    }
}