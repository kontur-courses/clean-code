using System;

namespace MarkdownProcessor
{
    public class Reader
    {
        private readonly char escapeCharacter;
        private readonly string text;
        private int position;
        private bool previousCharacterIsEscaping;

        public Reader(int startPosition, string text, bool previousCharacterIsEscaping, char escapeCharacter)
        {
            position = startPosition;
            this.text = text;
            this.previousCharacterIsEscaping = previousCharacterIsEscaping;
            this.escapeCharacter = escapeCharacter;
        }

        private CharacterContext CurrentCharacterContext =>
            new CharacterContext(position, text, previousCharacterIsEscaping);

        /// <summary>
        ///     Returns index of marker found in text string. If marker was not found, returns -1.
        /// </summary>
        public int ReadUntil(Func<CharacterContext, bool> isMarker)
        {
            while (position < text.Length)
            {
                if (CurrentCharacterIsEscaping())
                {
                    position++;
                    previousCharacterIsEscaping = true;
                    continue;
                }

                var markerFound = isMarker(CurrentCharacterContext);

                previousCharacterIsEscaping = false;
                position++;

                if (markerFound)
                    return position - 1;
            }

            return -1;
        }

        public void Skip(int charactersCount) => position += charactersCount;

        private bool CurrentCharacterIsEscaping() => !previousCharacterIsEscaping && text[position] == escapeCharacter;
    }
}