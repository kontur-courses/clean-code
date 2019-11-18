using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class Md
    {
        private const char EscapeChar = '\\';
        private static readonly List<MdTagDescriptor> TagDescriptors = new List<MdTagDescriptor>();
        private static readonly HashSet<string> Borders = new HashSet<string>();
        private static readonly HashSet<char> AllowedTagCharacters = new HashSet<char>();

        static Md()
        {
            var emphasizeTagDescr = new MdTagDescriptor("_", "<em>", "</em>");
            var strongTagDescr = new MdTagDescriptor("__", "<strong>", "</strong>");
            strongTagDescr.SetForbiddenInside(emphasizeTagDescr);
            TagDescriptors.Add(emphasizeTagDescr);
            TagDescriptors.Add(strongTagDescr);
            PrepareBorders();
        }

        public static void PrepareBorders()
        {
            foreach (var descriptor in TagDescriptors)
            {
                Borders.Add(descriptor.Border);
                foreach (var character in descriptor.Border) AllowedTagCharacters.Add(character);
            }
        }

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
        public MdTokenMark Mark;
        public int Pos;

        public string Value;

        public MdToken(int position, string value, MdTokenMark mark)
        {
            Pos = position;
            Value = value;
            Mark = mark;
        }
    }

    public class MdTag
    {
        public MdTagDescriptor Descriptor;
        public MdToken LeftBorder;
        public MdToken RightBorder;

        public MdTag(MdToken leftBorder, MdToken rightBorder, MdTagDescriptor descriptor)
        {
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
            Descriptor = descriptor;
        }

        public bool ForbiddenInside(MdTagDescriptor parentTagDescriptor)
        {
            return Descriptor.ForbiddenInside(parentTagDescriptor);
        }
    }

    public class MdTagDescriptor
    {
        public readonly string Border;

        public readonly List<MdTagDescriptor> ForbiddenTagContexts = new List<MdTagDescriptor>();

        public string LeftReplacement;
        public string RightReplacement;


        public MdTagDescriptor(string border, string leftReplacement, string rightReplacement)
        {
            Border = border;
            LeftReplacement = leftReplacement;
            RightReplacement = rightReplacement;
        }

        public void SetForbiddenInside(MdTagDescriptor tagDescriptor)
        {
            ForbiddenTagContexts.Add(tagDescriptor);
        }

        public bool ForbiddenInside(MdTagDescriptor parentTagDescriptor)
        {
            return ForbiddenTagContexts.Contains(parentTagDescriptor);
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