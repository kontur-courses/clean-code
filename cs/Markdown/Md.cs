using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string paragraph)
        {
            //TODO LINQ maybe
            var tokens = ExtractMdTokensFromText(paragraph);
            var tags = FilterMdTags(tokens);
            return RenderTags(tags, paragraph);
        }

        private static IEnumerable<MdToken> ExtractMdTokensFromText(string paragraph)
        {
            //TODO Go through paragraph and save suspicious chars to currentToken until
            //we Understand that there is no tokens that start by $currentToken
            //or Understand that conditions for char sequence to be token not satisfied
            //Then we clear currentToken

            //If found a letter or digit or space Then create MdToken instance with value 
            //of current token and position = lastTokenCharacterPos - tokenValue.Length+1;
            //add markings to token basing on whether token can be left or right or singular
            //multiple options are enabled

            //reset current token
            //yield token

            throw new NotImplementedException();
        }

        private static IEnumerable<MdTag> FilterMdTags(IEnumerable<MdToken> tokens)
        {
            //TODO For each token:
            //If it corresponds to left part of paired tag,
            //put it into stacklist(list used as a stack)

            //If it corresponds to right part of paired tag
            //search in stacklist in reverse until found it's
            //left counterpart Then create MdTag and yield it

            //If we found singular token create MdTag and yield it
            throw new NotImplementedException();
        }

        private static string RenderTags(IEnumerable<MdTag> tags, string paragraph)
        {
            //TODO create Builder for paragraph and
            //Foreach tag in tags:
            //render tag at paragraph
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///     Token data container
    /// </summary>
    public class MdToken
    {
        //TODO Replace by MarkContainer of something like this
        public List<MdTokenMark> TokenMarks;
        public int TokenPos;

        public string TokenValue;

        public MdToken(int position, string value, List<MdTokenMark> marks)
        {
            TokenPos = position;
            TokenValue = value;
            TokenMarks = marks;
        }

        //TODO Add utility methods to get whether token can be left or right or singular
        //MarkContainer getter
    }

    /// <summary>
    ///     Md tag instance created to enable multiple tag versions to be implemented
    /// </summary>
    public abstract class MdTag
    {
        public abstract void RenderAt(StringBuilder paragraphBuilder);
    }

    //TODO SingularTag : MdTag

    public class PairedMdTag : MdTag
    {
        public MdToken LeftBorder;
        public MdToken RightBorder;

        public override void RenderAt(StringBuilder paragraphBuilder)
        {
            //TODO Replace singular tag by it's corresponding html tag,
            //left border by opening html tag
            //right border by closing html tag
            throw new NotImplementedException();
        }
    }

    /// <summary>
    ///     Mark used to mark token if it can behave as left or right border of selection
    ///     or if it can only be singular
    /// </summary>
    public enum MdTokenMark
    {
        Left,
        Right,
        Singular
    }
}