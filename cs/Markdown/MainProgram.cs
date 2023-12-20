﻿using System.Collections.Immutable;

namespace Markdown;

public class MainProgram
{
    public static void Main(string[] args)
    {
        var tagDictionary = new Dictionary<string, TagType>()
        {
            {"_", TagType.Italic},
            {"__", TagType.Bold},
            {"# ", TagType.Heading},
            {"\n", TagType.LineBreaker},
            {"\r\n", TagType.LineBreaker}
            
        };
        var text = "_text_" + "\r\n";
        var sut = new Md();
        Console.WriteLine(sut.Render(text));
    }
}