using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Translator
    {
        private readonly List<MarkTranslatorElement> _dictionary = new List<MarkTranslatorElement>();
        private readonly char _escapeCharacter;

        public Translator(char escapeCharacter, params MarkTranslatorElement[] dictionaryElements )
        {
            this._escapeCharacter = escapeCharacter;
            foreach (var element in dictionaryElements)
                _dictionary.Add(element);
            
        }

        public string Translate(string text)
        {
            var tokenizer = new Tokenizer(_dictionary, _escapeCharacter);
            var tokens = tokenizer.Parse(text);

            return  HTMLBuilder.Build(tokens, text, _dictionary, _escapeCharacter);
        }
    }
    
}