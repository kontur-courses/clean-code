using System;
using System.Collections.Generic;
using Markdown.MdTagRecognizers;

namespace Markdown
{
    public class MdParser : IMdParser
    {
        private IMdTagRecognizer emphasisRecognizer;

        public Token[] Parse(string str)
        {

            if (str == null)
            {
                throw new ArgumentException("Given string can't be null", nameof(str));
            }

            var result = new List<Token>();

            using (emphasisRecognizer = new EmphasisRecognizer(result))
            {
                for (var position = 0; position < str.Length; position++)
                {
                    var token = GetToken(str, position);
                    result.Add(token);
                    position += token.Value.Length - 1;
                }
            }

            return result.ToArray();
        }

        private Token GetToken(string str, int position)
        {
            if (emphasisRecognizer.TryRecognize(str, position, out var type))
            {
                return new Token(type, MdSpecification.Tags[type]);
            }

            if (MdSpecification.IsText(str, position, out var result))
            {
                if (result == "")
                {
                    result += str[position];
                }

                return new Token(MdType.Text, result);
            }

            throw new NotSupportedException("Can't get token from given string");
        }
    }
}