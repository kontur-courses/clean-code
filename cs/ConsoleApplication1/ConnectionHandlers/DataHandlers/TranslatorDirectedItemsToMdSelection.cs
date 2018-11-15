using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.Interfaces;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.ConnectionHandlers.DataHandlers
{
    public class TranslatorDirectedItemsToMdSelection : ITranslatorDirectedItems<int>
    {
        private readonly IConnecter connecter;
        private readonly IConverter<Connection, MarkdownConnection> converter;
        private readonly IFilter<MarkdownConnection> filter;
        private readonly IHandlerConnections<MarkdownConnection, MdConvertedItem> handler;

        public void AddItem(int strength, Direction direction)
        {
            RaiseIfGivenStrengthIsIncorrect(strength);
            connecter.AddItem(strength, direction);
        }

        private IEnumerable<MarkdownConnection> ExtractConnections()
        {
            return connecter.GetSortedConneсtions()
                .Select(x => converter.Convert(x))
                .Select(x => filter.Filter(x));
        }

        public IReadOnlyCollection<MdConvertedItem> ExtractConvertedItems()
        {
            return handler
                .TranslateConnections(ExtractConnections(), connecter.GetResidualStrength())
                .ToArray();
        }

        private void RaiseIfGivenStrengthIsIncorrect(int strength)
        {
            if (strength <= 0)
                throw new ArgumentException("Strength of connection should be a non-negative number");
        }

        private void RaiseIfGivenArgumentsAreIncorrect(IConnecter connecter,
            IConverter<Connection, MarkdownConnection> converter,
            IFilter<MarkdownConnection> filter,
            IHandlerConnections<MarkdownConnection, MdConvertedItem> handler)
        {
            if (connecter == null)
                throw new ArgumentException("Connector should not be a null");
            if (converter == null)
                throw new ArgumentException("Converter should not be a null");
            if (filter == null)
                throw new ArgumentException("Filter should not be a null");
            if (handler == null)
                throw new ArgumentException("Handle should not be a null");
        }

        public TranslatorDirectedItemsToMdSelection(IConnecter connecter, 
            IConverter<Connection, MarkdownConnection> converter, 
            IFilter<MarkdownConnection> filter,
            IHandlerConnections<MarkdownConnection, MdConvertedItem> handler)
        {
            RaiseIfGivenArgumentsAreIncorrect(connecter, converter, filter, handler);
            this.connecter = connecter;
            this.converter = converter;
            this.filter = filter;
            this.handler = handler;
        }
    }
}
