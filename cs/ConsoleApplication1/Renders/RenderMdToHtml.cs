using System;
using System.Collections.Generic;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.ConnectionHandlers.DataHandlers;
using ConsoleApplication1.Interfaces;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.Renders
{
    public class RenderMdToHtml : ITextRender
    {
        private bool isFinished;
        private readonly List<TextPart> textParts = new List<TextPart>();

        private readonly ITranslatorDirectedItems<int> translator =
            new TranslatorDirectedItemsToMdSelection(new ItemsConnecter(),
                new ConverterConnectionsToMarkdown(),
                new FilterMeaninglessMdConnection(),
                new MdHandlerConnections());

        private IDirectionChooser<TextType> chooser;
        private ITextConverter<TextPart, MdConvertedItem> converter;

        public RenderMdToHtml(IDirectionChooser<TextType> chooser, ITextConverter<TextPart, MdConvertedItem> converter)
        {
            RaiseIfArgumentsAreIncorrect(chooser, converter);
            this.chooser = chooser;
            this.converter = converter;
        }

        private void RaiseIfArgumentsAreIncorrect(IDirectionChooser<TextType> chooser,
            ITextConverter<TextPart, MdConvertedItem> converter)
        {
            if (chooser == null)
                throw new ArgumentException("Chooser can't be null");
            if (converter == null)
                throw new ArgumentException("Converter can't be null");
        }

        public void AddNextPart(TextPart text)
        {
            RaiseIfAddingIsFinished();
            textParts.Add(text);
            if (text.Type != TextType.End)
                return;
            AddAllSelections();
            isFinished = true;
        }

        private void AddAllSelections()
        {
            for (var index = 0; index < textParts.Count; index++)
            {
                var textPart = textParts[index];
                if (textPart.Type != TextType.SpecialSymbols)
                    continue;
                var directionItem = chooser.GetDirection(GetTextType(index - 1), GetTextType(index + 1));
                translator.AddItem(textPart.Text.Length, directionItem);
            }
        }

        private TextType GetTextType(int index)
        {
            if (index < 0 || index > textParts.Count)
                return TextType.Empty;
            return textParts[index].Type;
        }

        private void RaiseIfAddingIsFinished()
        {
            if (isFinished)
                throw new ArgumentException("Adding is finished! You can not add new parts!");
        }

        private void RaiseIfAddingIsNotFinished()
        {
            if (!isFinished)
                throw new ArgumentException("Adding is not finished! You can not get translated text");
        }

        public string GetTranslatedText()
        {
            RaiseIfAddingIsNotFinished();
            return converter.ConvertText(textParts, translator.ExtractConvertedItems());
        }

        public bool IsTranslationFinished()
            => isFinished;
    }
}
