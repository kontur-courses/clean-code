using System.Collections.Generic;
using System.Text;
using MrakdaunV1.Enums;
using MrakdaunV1.Interfaces;

namespace MrakdaunV1.MrakdounEngine
{
    public class HtmlRednerer : IHtmlRenderer
    {
        private StringBuilder _htmlStringBuilder = new();
        private CharState _currentState = CharState.Default;
        private CharState _prevState = CharState.Default;

        // здесь собрана информация о каждом теге, такая, как:
        // - размер тега (из синтаксиса маркдауна)
        // - текст тегов HTML для открытия и закрытия
        private static readonly Dictionary<CharState, (int mdTagSize, string open, string close)> TagInfo = new()
        {
            [CharState.Bold] = (2, "<strong>", "</strong>"),
            [CharState.Italic] = (1, "<em>", "</em>"),
            [CharState.Header1] = (2, "<h1>", "</h1>")
        };
        
        /// <summary>
        /// Метод вернет HTML строку, принимая на вход исходный текст и массив стейтов
        /// </summary>
        public string Render(string text, CharState[] states)
        {
            _htmlStringBuilder = new();
            var charStatesForParsing = new[]
            {
                CharState.Header1,
                CharState.Italic,
                CharState.Bold
            };


            for (int i = 0; i < states.Length; i++)
            {
                _prevState = _currentState;
                _currentState = states[i];

                foreach (var charState in charStatesForParsing)
                {
                    ParseAppearedState(charState, out var additionalOffset);
                    ParseDisappearedState(charState, text[i]);
                    i += additionalOffset;
                }

                if (!_currentState.HasFlag(CharState.Special))
                    _htmlStringBuilder.Append(text[i]);
            }
            
            // в конце закроем теги, которые не были закрыты ранее
            _prevState = _currentState;
            _currentState = CharState.Default;
            foreach (var charState in charStatesForParsing)
                ParseDisappearedState(charState, null);
            
            return _htmlStringBuilder.ToString();
        }

        private bool IsStateAppeared(CharState current, CharState prev, CharState must)
        {
            // стейт появился - это когда сейчас есть, а раньше не было
            return current.HasFlag(must) && !prev.HasFlag(must);
        }

        private bool IsStateDisappeared(CharState current, CharState prev, CharState must)
        {
            // стейт исчез - это когда раньше был, а щас нет
            return prev.HasFlag(must) && !current.HasFlag(must);
        }

        /// <summary>
        /// Метод обработает начало тега, если оно есть, а также
        /// вернет дополнительное смещение (нужно для тегов с длиной более 1 символа)
        /// </summary>
        private void ParseAppearedState(
            CharState state,
            out int additionalOffset)
        {
            
            additionalOffset = 0;

            if (!IsStateAppeared(_currentState, _prevState, state))
                return;

            _htmlStringBuilder.Append(TagInfo[state].open);
            additionalOffset = TagInfo[state].mdTagSize - 1;
        }

        /// <summary>
        /// Метод обработает конец тега, если он есть
        /// </summary>
        private void ParseDisappearedState(
            CharState state,
            char? currentChar)
        {
            if (!IsStateDisappeared(_currentState, _prevState, state))
                return;

            var putCloseTagsBeforeLastChar = state == CharState.Header1 && !(currentChar is null);

            // в случае с, например, заголовком, необходимо вставить тег перед
            // предыдущим символом
            if (putCloseTagsBeforeLastChar)
            {
                var lastChar = _htmlStringBuilder[^1];
                _htmlStringBuilder.Remove(_htmlStringBuilder.Length - 1, 1);
                _htmlStringBuilder.Append(TagInfo[state].close);
                _htmlStringBuilder.Append(lastChar);
                
                if (!(currentChar is null))
                    _htmlStringBuilder.Append(currentChar);
                return;
            }
            _htmlStringBuilder.Append(TagInfo[state].close);
            
            if (!(currentChar is null))
                _htmlStringBuilder.Append(currentChar);
        }
    }
}