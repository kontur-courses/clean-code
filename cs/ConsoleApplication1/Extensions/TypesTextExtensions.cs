using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.Extensions
{
    public static class TextTypesExtensions
    {
        public static bool IsItTextType(this TextType textType)
        {
            return textType == TextType.SimpleText;
        }
    }
}
