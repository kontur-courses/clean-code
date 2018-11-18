﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Languages;
using Markdown.Tokenizing;

namespace Markdown.Translator
{
    public class Translator
    {
        private readonly Language language;

        public Translator(Language language)
        {
            this.language = language;
        }

        public string Translate(List<Token> tokens)
        {
            var stringBuilder = new StringBuilder();

            foreach (var token in tokens)
            {
                stringBuilder.Append(TranslateToken(token));
            }

            return stringBuilder.ToString();
        }

        private string TranslateToken(Token token)
        {
            if (token.Tag == Tag.Raw)
                return token.Content;

            return language.OpeningTags.FirstOrDefault(pair => pair.Key == token.Tag).Value ??
                   language.ClosingTags.First(pair => pair.Key == token.Tag).Value;
        }
    }
}
