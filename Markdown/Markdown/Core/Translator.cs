using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Translator
    {
        private static List<MarkTranslatorElement> _dictionary = new List<MarkTranslatorElement>()
        {
            new MarkHtml("_", "<em>", false),
            new MarkHtml("__", "<strong>")
        };

        private static char _escapeCharacter = '\\';

        public static string Translate(string text)
        {
            var tokenizer = new Tokenizer(_dictionary, _escapeCharacter);
            var tokens = tokenizer.Parse(text);

            return HTMLBuilder.Build(tokens, text, _dictionary, _escapeCharacter);
        }
    }
}