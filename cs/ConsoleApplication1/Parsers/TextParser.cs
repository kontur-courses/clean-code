using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ConsoleApplication1.Extensions;
using ConsoleApplication1.Interfaces;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.Parsers
{
    public class TextParser : IParser
    {
        private readonly IReader textReader;
        private readonly HashSet<char> parseSymbols;
        private readonly bool parseWhiteSpaces;
        private bool isFinished;
        private const char Slash = '\\';
        private char? lastCharacter;
        private CharacterType lastCharacterType;
        public bool AnyParts()
            => !isFinished;

        public TextPart GetNextPart()
        {
            RaiseIfParsingIsFinished();
            if (!ContainsAnyLetters())
                return GetEndPart();
            return GetNextNonEmptyPart();
        }

        private bool ContainsAnyLetters()
            => lastCharacter.HasValue || textReader.AnySymbols();

        private TextPart GetNextNonEmptyPart()
        {
            var nextCharacter = GetNextSymbol();
            return GetNextPartWithConditions(lastCharacterType);
        }

        private CharacterType GetTypeNextCharacter()
        {
            if (!lastCharacter.HasValue)
                ReadNextSymbol();
            return lastCharacterType;
        }

        
        private TextPart GetNextPartWithConditions(CharacterType characterType)
        {
            var text = new StringBuilder();
            while (ContainsAnyLetters() && GetTypeNextCharacter() == characterType)
            {
                text.Append(GetNextSymbol());
                ClearLastReadLetter();
            }
            return new TextPart(text.ToString(), characterType.BelongsTo());
        }

        private TextPart GetEndPart()
        {
            isFinished = true;
            return new TextPart("", TextType.End);
        }

        private void ClearLastReadLetter()
        {
            lastCharacter = null;
        }

        private void ReadNextSymbol()
        {
            var character = textReader.ReadNextSymbol();
            var isEscaped = false;
            if (character == Slash && textReader.AnySymbols())
            {
                isEscaped = true;
                character = textReader.ReadNextSymbol();
            }
            lastCharacterType = GetCharacterType(character, isEscaped);
            lastCharacter = character;
        }

        private char GetNextSymbol()
        {
            if (!lastCharacter.HasValue)
                ReadNextSymbol();
            return lastCharacter.Value;
        }

        private CharacterType GetCharacterType(char character, bool isEscaped)
        {
            if (parseWhiteSpaces && Char.IsWhiteSpace(character))
                return CharacterType.WhiteSpaces;
            return isEscaped || !parseSymbols.Contains(character)
                 ? CharacterType.SimpleCharacter
                 : CharacterType.SpecialCharacter;
        }

        private void RaiseIfParsingIsFinished()
        {
            if (isFinished)
                throw new InvalidOperationException("Parsing is finished already");
        }

        private void RaiseIfParserCreationArgumentsAreIncorrect(IReader textReader, ReadOnlyCollection<char> parseSymbols, bool parseWhiteSpaces)
        {
            if (textReader == null)
                throw new ArgumentException("Text reader should not be null");
            if (parseSymbols == null)
                throw new ArgumentException("Parse symbols should not be empty array");
            if (parseWhiteSpaces && parseSymbols.Any(x => Char.IsWhiteSpace(x)))
                throw new ArgumentException("Parse symbols shouldn't contain white spaces");
            if (parseSymbols.Contains(Slash))
                throw new ArgumentException("Parse symbols shouldn't contain slash");
        }

        public TextParser(IReader textReader, ReadOnlyCollection<char> parseSymbols, bool parseWhiteSpaces)
        {
            RaiseIfParserCreationArgumentsAreIncorrect(textReader, parseSymbols, parseWhiteSpaces);
            this.textReader = textReader;
            this.parseSymbols = new HashSet<char>(parseSymbols);
            this.parseWhiteSpaces = parseWhiteSpaces;
        }
    }
}
