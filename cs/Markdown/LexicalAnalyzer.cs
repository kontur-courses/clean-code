﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class LexicalAnalyzer
    {
        public LexicalAnalyzer()
        {

        }

        public List<(int, LexType)> Analyze(string text)
        {
            var lexemes = new List<(int, LexType)>();
            var reader = new InputReader(text);
            while (reader.Current() != default)
            {
                switch (reader.Current())
                {
                    case '_':
                        ProcessUnderscore(reader, lexemes);
                        break;
                    case ' ':
                        ProcessSpace(reader, lexemes);
                        break;
                    case '\\':
                        ProcessBackslash(reader, lexemes);
                        break;
                    case '[':
                        ProcessBracket(reader, lexemes, reader.Current());
                        break;
                    case ']':
                        ProcessBracket(reader, lexemes, reader.Current());
                        break;
                    case ')':
                        ProcessBracket(reader, lexemes, reader.Current());
                        break;
                    case '(':
                        ProcessBracket(reader, lexemes, reader.Current());
                        break;
                    default:
                        ProcessLetter(reader, lexemes);
                        break;
                }
            }


            return lexemes;
        }

        private void ProcessBackslash(InputReader reader, List<(int, LexType)> lexemes)
        {
            lexemes.Add((reader.CurrentPosition, LexType.TextWithBackslash));
            reader.Next();// мы пропускаем один символ считая его текстом, возможно придется сделать послжнее конструкцию
            reader.Next();
        }

        private void ProcessSpace(InputReader reader, List<(int, LexType)> lexemes)
        {
            lexemes.Add((reader.CurrentPosition, LexType.Space));
            while(reader.Current()==' ')
                reader.Next();
        }

        public void ProcessUnderscore(InputReader reader, List<(int, LexType)> lexemes)
        {
            if (reader.PeekNext()!='_')
                lexemes.Add((reader.CurrentPosition, LexType.Underscore));
            else
            {
                reader.Next();
                if (reader.PeekNext()!='_')
                    lexemes.Add((reader.CurrentPosition-1, LexType.DoubleUnderscore));
                else
                {
                    lexemes.Add((reader.CurrentPosition - 1, LexType.Text));//обнаружили более двух подчеркиваний, считаем это просто текстом
                    while(reader.PeekNext()=='_')
                        reader.Next();
                }
            }
            reader.Next();
        }

        public void ProcessLetter(InputReader reader, List<(int, LexType)> lexemes)
        {
            if (lexemes.Count==0 || lexemes[lexemes.Count-1].Item2!=LexType.Text)
                lexemes.Add((reader.CurrentPosition, LexType.Text));
            reader.Next();
        }

        public void ProcessBracket(InputReader reader, List<(int, LexType)> lexemes, char bracket)
        {
            switch (bracket)
            {
                case '[':
                    lexemes.Add((reader.CurrentPosition, LexType.SquareBracketOpen));
                    reader.Next();
                    break;
                case ']':
                    lexemes.Add((reader.CurrentPosition, LexType.SquareBracketClose));
                    reader.Next();
                    break;
                case '(':
                    lexemes.Add((reader.CurrentPosition, LexType.BracketOpen));
                    reader.Next();
                    break;
                case ')':
                    lexemes.Add((reader.CurrentPosition, LexType.BracketClose));
                    reader.Next();
                    break;
            }
        }
    }

    public enum LexType
    {
        Text,
        TextWithBackslash,
        Space,
        Underscore,
        DoubleUnderscore,
        SquareBracketOpen,
        SquareBracketClose,
        BracketOpen,
        BracketClose

    }
}
