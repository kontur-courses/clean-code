﻿namespace MarkdownParser.Infrastructure.Tokenization.Abstract
{
    /// <summary>
    /// Однородная группа символов из текста
    /// Может быть просто текстом, либо каким-либо управляющим символом, например _ или __
    /// </summary>
    public abstract class Token
    {
        public Token(int startPosition, string rawValue)
        {
            StartPosition = startPosition;
            RawValue = rawValue;
        }

        public int StartPosition { get; }

        public string RawValue { get; }

        public override string ToString() => $"{GetType().Name}[{StartPosition}]: {RawValue}";
    }
}