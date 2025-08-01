namespace DeveloperStore.Sales.Domain
{
    public class Quantity : IEquatable<Quantity>
    {
        public int Value { get; private set; }

        public Quantity(int value)
        {
            if (value <= 0)
            {
                throw new ArgumentException("Quantidade deve ser positiva.", nameof(value));
            }
            Value = value;
        }

        public static Quantity operator +(Quantity q1, Quantity q2) => new Quantity(q1.Value + q2.Value);
        public static Quantity operator -(Quantity q1, Quantity q2)
        {
            if (q1.Value - q2.Value <= 0)
            {
                throw new InvalidOperationException("Não pode reduzir a quantidade para zero ou negativo.");
            }
            return new Quantity(q1.Value - q2.Value);
        }

        public bool Equals(Quantity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Quantity)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}