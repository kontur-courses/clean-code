using System;

namespace ConsoleApplication1.ConnectionHandlers.Containers
{
    public class Connection
    {
        public readonly int FirstItemIndex;
        public readonly int SecondItemIndex;
        public readonly int ConnectionStrength;

        public Connection(int indexFirstItem, int indexSecondItem, int connectionStrength)
        {
            RaiseExceptionIfArgumentsAreIncorrect(indexFirstItem, indexSecondItem, connectionStrength);
            if (indexFirstItem >= indexSecondItem)
            {
                FirstItemIndex = indexSecondItem;
                SecondItemIndex = indexFirstItem;
            }
            else
            {
                FirstItemIndex = indexFirstItem;
                SecondItemIndex = indexSecondItem;
            }
            ConnectionStrength = connectionStrength;
        }

       

        private void RaiseExceptionIfArgumentsAreIncorrect(int indexFirstItem, int indexSecondItem, int connectionStrength)
        {
            if (indexFirstItem == indexSecondItem)
                throw new ArgumentException("Indexes should be different");
            if (indexFirstItem < 0 || indexSecondItem < 0)
                throw new ArgumentException("Indexes should be positive numbers");
            if (connectionStrength <= 0)
                throw new ArgumentException("Connection strength shouble be a positive number");


        }
    }
}
