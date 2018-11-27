using System;
using System.Collections.Generic;
using Fclp;
using Markdown.Md;
using Markdown.Renderers;

namespace Markdown.CmdInterface
{
    public class CmdInterface
    {
        public enum ListCmdTypes
        {
            Converters
        }

        private readonly IDictionary<string, IConverter> converters;
        private readonly FluentCommandLineParser parser;
        private readonly CmdCallbacks callbacks;

        public CmdInterface(string[] args)
        {
            converters = new Dictionary<string, IConverter>();
            callbacks = new CmdCallbacks();
            parser = new FluentCommandLineParser();

            converters.Add("markdown2html",
                new Md.Md(new Parser(MdSpecification.GetTagHandlerChain(), new TagToTextTagConverter()),
                    new Renderer(MdSpecification.GetHtmlTagHandlerChain())));

            parser.SetupHelp("?", "help")
                .Callback(text => Console.Write(callbacks.GetHelpInformation()));
            parser.Setup<ListCmdTypes>("list")
                .Callback(text => Console.Write(callbacks.GetAvailableConvertersNames(converters)));
            parser.Setup<string>("converter")
                .Callback(converterName => Console.Write(callbacks.SetConverter(converterName, converters)));
            parser.Setup<string>("output")
                .Callback(outputFilename => callbacks.SetOutputFile(outputFilename));
            parser.Setup<string>("convert")
                .Callback(inputFilename => Console.Write(callbacks.Convert(inputFilename)));

            parser.Parse(args);
        }
    }
}