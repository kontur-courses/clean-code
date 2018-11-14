using System;
using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.Interfaces;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.ConnectionHandlers.DataHandlers
{
    public class FilterMeaninglessMdConnection : IFilter<MarkdownConnection>
    {
        private MarkdownConnection previousConnection;
        private MarkdownConnectionType maxPossibleType;

        public MarkdownConnection Filter(MarkdownConnection mdConnection)
        {
            RaiseExceptionIfGivenMdConnectionIsIncorrect(mdConnection);
            ResetLimit(mdConnection);
            var newType = GetNewType(mdConnection);
            UpdateMdLimitsType(newType);
            var newConnection = mdConnection.ChangeType(newType);
            previousConnection = newConnection;
            return previousConnection;
        }

        private void ResetLimit(MarkdownConnection mdConnection)
        {
            if (previousConnection == null || !ContainsInPreviousConnection(mdConnection))
                maxPossibleType = MarkdownConnectionType.SingleAndDouble;
        }

        private bool ContainsInPreviousConnection(MarkdownConnection mdConnection)
        {
            if (previousConnection == null)
                throw new InvalidOperationException("Previous connection is null");
            return mdConnection.FirstIndex >= previousConnection.FirstIndex
                && mdConnection.SecondIndex <= previousConnection.SecondIndex;
        }

        private void UpdateMdLimitsType(MarkdownConnectionType type)
        {
            if (type == MarkdownConnectionType.SingleAndDouble || type == MarkdownConnectionType.Single)
                maxPossibleType = MarkdownConnectionType.None;
            else if (type == MarkdownConnectionType.Double)
                maxPossibleType = MarkdownConnectionType.Single;
        }

        private void RaiseExceptionIfGivenMdConnectionIsIncorrect(MarkdownConnection givenMdConnection)
        {
            if (givenMdConnection == null)
                throw new ArgumentException("Md connection shouldn't be null");
        }

        private MarkdownConnectionType GetNewType(MarkdownConnection mdConnection)
        {
            var currentConnectionType = mdConnection.ConnectionType;
            return currentConnectionType <= maxPossibleType
                ? currentConnectionType
                : maxPossibleType;
        }
    }
}
