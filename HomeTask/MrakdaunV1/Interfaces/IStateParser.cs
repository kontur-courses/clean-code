using System.Collections.Generic;
using MrakdaunV1.Enums;
using MrakdaunV1.MrakdounEngine;

namespace MrakdaunV1.Interfaces
{
    public interface IStateParser
    {
        public CharState[] GetParsedStates(List<TokenPart> tokenParts, CharState[] charStates, string text);
    }
}