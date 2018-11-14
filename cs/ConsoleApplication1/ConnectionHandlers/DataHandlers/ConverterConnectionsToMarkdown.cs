using ConsoleApplication1.ConnectionHandlers.Containers;
using ConsoleApplication1.Interfaces;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.ConnectionHandlers.DataHandlers
{
    public class ConverterConnectionsToMarkdown : IConverter<Connection, MarkdownConnection>
    {
        public MarkdownConnection Convert(Connection connection)
        {
            return new MarkdownConnection(connection, GetTypeFromStrengthConnection(connection.ConnectionStrength));
        }

       private MarkdownConnectionType GetTypeFromStrengthConnection(int strengthConnection)
        {
            if (strengthConnection == 1)
                return MarkdownConnectionType.Single;
            return strengthConnection == 2
                ? MarkdownConnectionType.Double
                : MarkdownConnectionType.SingleAndDouble;
        }
    }
}
