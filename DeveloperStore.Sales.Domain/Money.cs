namespace DeveloperStore.Sales.Domain
{
    public class Money
    {
        public decimal Value { get; private set; }

        public Money(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentException("Money não pode valor negativo.", nameof(value));
            }
            Value = value;
        }
    }
}