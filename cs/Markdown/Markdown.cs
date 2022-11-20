using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown;

namespace Markdown
{
    public enum MorphemeType
    {
        Word,
        Postfix,
        Prefix
    }


    public interface IWord
    {
        string View { get; }
    }


    public interface IMorpheme : IWord
    {

        MorphemeType Type { get; }

         bool CheckForCompliance(string textContext, int position);
    }

    public abstract class Prefix : IMorpheme
    {
        public abstract string View { get; }
        public MorphemeType Type => MorphemeType.Prefix;
        public abstract bool CheckForCompliance(string textContext, int position);
    }

    public abstract class Postfix : IMorpheme
    {
        public abstract string View { get; }
        public MorphemeType Type => MorphemeType.Postfix;
        public abstract bool CheckForCompliance(string textContext, int position);
    }



    public class PostfixItalic : Postfix
    {
        public override string View => "_";

        //bad_a
        public override bool CheckForCompliance(string textContext, int position)
        {

            if (textContext[position] != '_' || position < 3)
                return false;

            if (textContext[position - 1] == '_'
                || position + 1 < textContext.Length && textContext[position + 1] == '_')
                return false;

            if (char.IsWhiteSpace(textContext[position - 1]))
                return false;

            if (position + 1 < textContext.Length
                && char.IsDigit(textContext[position + 1]) || char.IsDigit(textContext[position - 1]))
                return false;

            return true;
        }
    }

    public class PostfixBold : Postfix
    {
        public override string View => "__";
        public override bool CheckForCompliance(string textContext, int position)
        {
            if (position < 1)
                return false;

            if (textContext[position] != '_' || textContext[position - 1] != '_' || position < 3)
                return false;

            if (char.IsWhiteSpace(textContext[position - 2]))
                return false;

            if (position + 1 < textContext.Length && char.IsDigit(textContext[position + 1]) || (char.IsDigit(textContext[position - 2])))
                return false;

            return true;
        }
    }


    public class PrefixItalic : Prefix
    {
        public override string View => "_";
        public override bool CheckForCompliance(string textContext, int position)
        {
            if (textContext[position] != '_' || position >= textContext.Length - 1)
                return false;

            if (char.IsWhiteSpace(textContext[position + 1]))
                return false;

            if (position - 1 >= 0 && textContext[position - 1] == '_' || textContext[position + 1] == '_')
                return false;

            if (char.IsDigit(textContext[position + 1]) || position - 1 >= 0 && char.IsDigit(textContext[position - 1]))
                return false;

            return true;
        }
    }


    public class PrefixBold : Prefix{
        public override string View => "__";
        public override bool CheckForCompliance(string textContext, int position)
        {
            if (position + 2 >= textContext.Length)
                return false;

            if (textContext[position] != '_' ||
                position - 1 >= 0 && textContext[position - 1] != '_')
                return false;

            if (char.IsWhiteSpace(textContext[position + 1]))
                return false;

            if (position - 1 >= 0 && char.IsDigit(textContext[position - 1]) ||
                position - 2 >= 0 && char.IsDigit(textContext[position - 2]))
                return false;

            return true; ;
        }
    }


    public class Word : IMorpheme
    {
        public Word(string word)
        {
            View = word;
        }

        public string View { get; }
        
        public MorphemeType Type => MorphemeType.Word;

        public bool CheckForCompliance(string textContext, int position)
        {
            return char.IsLetter(textContext[position]);
        }
    }

    public class PrefixHeader : Prefix
    {
        public override string View => "# ";

        public override bool CheckForCompliance(string textContext, int position)
        {
            return position == 0 && textContext.Length > 2 && textContext[..2] == View;
        }
    }


    public  class ReaderWord
    {
        private static List<IMorpheme> morphemes;

        static ReaderWord()
        {
            morphemes = new()
            {
                new PostfixBold(),
                new PrefixBold(),
                new PostfixItalic(),
                new PrefixItalic(),
                new PrefixHeader()
            };
        }


        public static List<IMorpheme> ReadWord(string word)
        {
            
            var isShielding = false;
            var result = new List<IMorpheme>();


            for (var i = 0; i < word.Length;)
            {
                var morph = CheckForMorphemes(word, i);
                i = morph.NextIndex;

            }
        }

        private static (IMorpheme Morpheme, int NextIndex) CheckForMorphemes(string context, int position)
        {
            throw new NotImplementedException();
        }
    }




}
