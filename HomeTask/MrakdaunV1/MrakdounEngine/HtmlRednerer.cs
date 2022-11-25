using System;
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

        private static readonly Dictionary<CharState, (int mdTagSize, string open, string close)> tagInfo = new()
        {
            [CharState.Bold] = (2, "<strong>", "</strong>"),
            [CharState.Italic] = (1, "<em>", "</em>"),
            [CharState.Header1] = (2, "<h1>", "</h1>")
        };

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

        private void ParseAppearedState(
            CharState state,
            char currentChar,
            out int additionalOffset)
        {
            additionalOffset = 0;

            if (!IsStateAppeared(_currentState, _prevState, state))
                return;

            _htmlStringBuilder.Append(tagInfo[state].open);
            additionalOffset = tagInfo[state].mdTagSize - 1;
        }

        private void ParseDisappearedState(
            CharState state,
            char currentChar)
        {
            if (!IsStateDisappeared(_currentState, _prevState, state))
                return;

            var putCloseTagsBeforeLastChar = state == CharState.Header1;

            if (putCloseTagsBeforeLastChar)
            {
                var lastChar = _htmlStringBuilder[^1];
                _htmlStringBuilder.Remove(_htmlStringBuilder.Length - 1, 1);
                _htmlStringBuilder.Append(tagInfo[state].close);
                _htmlStringBuilder.Append(lastChar);
                _htmlStringBuilder.Append(currentChar);
                return;
            }
            _htmlStringBuilder.Append(tagInfo[state].close);
            _htmlStringBuilder.Append(currentChar);
        }

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
                    ParseAppearedState(charState, text[i], out var additionalOffset);
                    ParseDisappearedState(charState, text[i]);
                    i += additionalOffset;
                }

                if (!_currentState.HasFlag(CharState.Special))
                    _htmlStringBuilder.Append(text[i]);
            }

            return _htmlStringBuilder.ToString();
        }
    }
}