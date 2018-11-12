using System;
using System.Collections.Generic;

namespace Markdown.MdTagRecognizers
{
    public class EmphasisRecognizer : IMdTagRecognizer
    {
        private readonly Queue<(int, MdType)> openingEmphasis = new Queue<(int, MdType)>();
        private readonly Stack<(int, MdType)> closingEmphasis = new Stack<(int, MdType)>();
        private readonly IList<Token> resultTokens;

        public EmphasisRecognizer(IList<Token> resultTokens)
        {
            this.resultTokens = resultTokens;
        }

        public bool TryRecognize(string str, int position, out MdType type)
        {
            type = Recognize(str, position);

            if (type == MdType.Text)
            {
                return false;
            }

            var tokenPosition = resultTokens.Count;

            switch (type)
            {
                case MdType.OpenEmphasis:
                case MdType.OpenStrongEmphasis:

                    openingEmphasis.Enqueue((tokenPosition, type));

                    break;
                case MdType.CloseEmphasis:
                case MdType.CloseStrongEmphasis:
                    closingEmphasis.Push((tokenPosition, type));

                    break;
            }

            return true;
        }

        private static MdType Recognize(string str, int position)
        {
            if (MdSpecification.IsEscape(str, position))
            {
                return MdType.Text;
            }

            if (IsStrongEmphasis(str, position)
                && IsClosedEmphasis(str, position)
                && (position + 2 >= str.Length || str[position + 2] != '_'))
            {
                return MdType.CloseStrongEmphasis;
            }

            if (IsStrongEmphasis(str, position)
                && IsOpenedEmphasis(str, position + 1)
                && (position - 1 < 0 || str[position - 1] == ' '))
            {
                return MdType.OpenStrongEmphasis;
            }

            if (IsEmphasis(str, position)
                && IsClosedEmphasis(str, position)
                && (position - 1 < 0 || str[position - 1] != '_'))
            {
                return MdType.CloseEmphasis;
            }

            if (IsEmphasis(str, position)
                && IsOpenedEmphasis(str, position))
            {
                return MdType.OpenEmphasis;
            }

            return MdType.Text;
        }

        private static bool IsOpenedEmphasis(string str, int position)
        {
            return
                position + 1 < str.Length
                && !char.IsWhiteSpace(str[position + 1])
                && !char.IsNumber(str[position + 1]);
        }

        private static bool IsClosedEmphasis(string str, int position)
        {
            return
                position - 1 >= 0
                && !char.IsWhiteSpace(str[position - 1])
                && !char.IsNumber(str[position - 1]);
        }

        private static bool IsEmphasis(string str, int position)
        {
            return str[position] == '_';
        }

        private static bool IsStrongEmphasis(string str, int position)
        {
            return position + 1 < str.Length
                && str[position] == '_'
                && str[position + 1] == '_';
        }

        private void HandleTags()
        {
            var inEmphasis = false;

            while (openingEmphasis.Count != 0 && closingEmphasis.Count != 0)
            {
                var openType = openingEmphasis.Peek()
                    .Item2;
                var closeType = closingEmphasis.Peek()
                    .Item2;

                switch (openType)
                {
                    case MdType.OpenEmphasis:
                        inEmphasis = true;

                        break;
                    case MdType.CloseEmphasis:
                        inEmphasis = false;

                        break;
                }

                if (inEmphasis
                    && (openType == MdType.OpenStrongEmphasis
                        || openType == MdType.CloseStrongEmphasis))
                {
                    ConvertTokenToTextInResult(
                        openingEmphasis.Peek()
                            .Item1);

                    ConvertTokenToTextInResult(
                        closingEmphasis.Peek()
                            .Item1);

                    openingEmphasis.Dequeue();
                    closingEmphasis.Pop();

                    continue;
                }

                if (IsStrongEmphasis(openType) && IsStrongEmphasis(closeType)
                    || IsEmphasis(openType) || IsEmphasis(closeType))
                {
                    openingEmphasis.Dequeue();
                    closingEmphasis.Pop();
                }
                else
                {
                    break;
                }
            }
        }

        private void ConvertUnclosedTagsToText()
        {
            while (openingEmphasis.Count != 0)
            {
                var position = openingEmphasis
                    .Dequeue()
                    .Item1;
                ConvertTokenToTextInResult(position);
            }

            while (closingEmphasis.Count != 0)
            {
                var position = closingEmphasis
                    .Pop()
                    .Item1;
                ConvertTokenToTextInResult(position);
            }
        }

        public void Dispose()
        {
            HandleTags();
            ConvertUnclosedTagsToText();
            openingEmphasis.Clear();
            closingEmphasis.Clear();
        }

        private void ConvertTokenToTextInResult(int position)
        {
            resultTokens[position] = new Token(
                MdType.Text,
                resultTokens[position]
                    .Value);
        }

        private bool IsEmphasis(MdType type)
        {
            return type == MdType.OpenEmphasis || type == MdType.CloseEmphasis;
        }

        private bool IsStrongEmphasis(MdType type)
        {
            return type == MdType.OpenStrongEmphasis || type == MdType.CloseStrongEmphasis;
        }
    }
}