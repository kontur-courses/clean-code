using System;

namespace MrakdaunV1.Enums
{
    [Flags]
    public enum CharState
    {
        Default = 0,
        Special = 1,
        Italic = 2,
        Bold = 4,
        Header1 = 8
    }
}