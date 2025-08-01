namespace DeveloperStore.Sales.Domain
{
    public class SaleItem
    {
        public Guid Id { get; private set; }
        public Guid ProductId { get; private set; } 
        public Quantity Quantity { get; private set; } 
        public Money UnitPrice { get; private set; } 
        public Money Discount { get; private set; }
        public Money TotalItemAmount { get; private set; } 
        public bool IsCancelled { get; private set; }

        public Guid SaleId { get; private set; }
        public Sale Sale { get; private set; }

        protected SaleItem() { }

        public SaleItem(Guid productId, Quantity quantity, Money unitPrice)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            UnitPrice = unitPrice ?? throw new ArgumentNullException(nameof(unitPrice));
            IsCancelled = false;

            UpdateQuantity(quantity);
        }

        public void UpdateQuantity(Quantity newQuantity)
        {
            if (IsCancelled)
                throw new InvalidOperationException("Não pode alterar a quantidade de um item cancelado.");

            if (newQuantity.Value > 20)
                throw new InvalidOperationException("Não é possível vender acima de 20 itens idênticos.");

            Quantity = newQuantity;
            ApplyDiscountAndCalculateTotal();
        }

        public void Cancel()
        {
            if (IsCancelled)
            {
                throw new InvalidOperationException("Esse item de venda já está cancelado.");
            }
            IsCancelled = true;
            TotalItemAmount = new Money(0);
            Discount = new Money(0); 
        }

        private void ApplyDiscountAndCalculateTotal()
        {
            Discount = new Money(0);

            decimal baseAmount = Quantity.Value * UnitPrice.Value;

            if (Quantity.Value >= 10 && Quantity.Value <= 20)
            {
                Discount = new Money(baseAmount * 0.20m);
            }
            else if (Quantity.Value >= 4)
            {
                Discount = new Money(baseAmount * 0.10m);
            }

            TotalItemAmount = new Money(baseAmount - Discount.Value);
        }
    }
}