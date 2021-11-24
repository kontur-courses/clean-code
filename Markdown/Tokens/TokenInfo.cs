﻿namespace Markdown
{
    internal record TokenInfo
    {
        public int Position { get; }
        public string Token { get; }
        public bool CloseValid { get; }
        public bool OpenValid { get; }
        public bool WordPartPlaced { get; }
        public bool Valid { get; set; }

        public TokenInfo(int position, string token, bool closeValid, bool openValid, bool wordPartPlaced, bool valid)
        {
            Position = position;
            Token = token;
            CloseValid = closeValid;
            OpenValid = openValid;
            WordPartPlaced = wordPartPlaced;
            Valid = valid;
        }
        
        public void Deconstruct(out int position, out string token, out bool closeValid, out bool openValid, out bool wordPartPlaced, out bool valid)
        {
            (position, token, closeValid, openValid, wordPartPlaced, valid) = (Position, Token, CloseValid, OpenValid, WordPartPlaced, Valid);
        }

        public TokenInfo WithPosition(int position)
        {
            return new TokenInfo(position, Token, CloseValid, OpenValid, WordPartPlaced, Valid);
        }
    }
}