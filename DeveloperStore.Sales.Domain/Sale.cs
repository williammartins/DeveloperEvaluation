namespace DeveloperStore.Sales.Domain
{
    public class Sale
    {
        public Guid Id { get; private set; } 
        public int SaleNumber { get; private set; } 
        public DateTime SaleDate { get; private set; } 
        public Guid CustomerId { get; private set; } 
        public Guid BranchId { get; private set; } 
        public Money TotalAmount { get; private set; } 
        public bool IsCancelled { get; private set; } 

        private readonly List<SaleItem> _items;
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        protected Sale()
        {
            _items = new List<SaleItem>();
        }

        public Sale(int saleNumber, Guid customerId, Guid branchId)
        {
            Id = Guid.NewGuid();
            SaleNumber = saleNumber;
            SaleDate = DateTime.UtcNow;
            CustomerId = customerId;
            BranchId = branchId;
            TotalAmount = new Money(0); 
            IsCancelled = false;
            _items = new List<SaleItem>();
        }

        public void AddItem(SaleItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item), "Item não pode ser null.");

            var existingItem = _items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null) throw new InvalidOperationException($"Produto com ID {item.ProductId} já existe na venda.");

            _items.Add(item);
            CalculateTotalAmount();
        }

        public void RemoveItem(Guid productId)
        {
            var itemToRemove = _items.FirstOrDefault(i => i.ProductId == productId);
            if (itemToRemove == null)
            {
                throw new InvalidOperationException($"Produto com ID {productId} não encontrado na venda.");
            }

            _items.Remove(itemToRemove);
            CalculateTotalAmount(); 
        }

        public void Cancel()
        {
            if (IsCancelled)
            {
                throw new InvalidOperationException("Essa venda já está cancelada.");
            }
            IsCancelled = true;

            foreach (var item in _items)
            {
                item.Cancel();
            }
        }

        public void UpdateItem(Guid productId, Quantity newQuantity)
        {
            var itemToUpdate = _items.FirstOrDefault(i => i.ProductId == productId);
            if (itemToUpdate == null)
            {
                throw new InvalidOperationException($"Produto com ID {productId} não encontrado na venda.");
            }

            itemToUpdate.UpdateQuantity(newQuantity);
            CalculateTotalAmount();
        }

        public void CancelItem(Guid productId)
        {
            var itemToCancel = _items.FirstOrDefault(i => i.ProductId == productId);
            if (itemToCancel == null)
            {
                throw new InvalidOperationException($"Produto com ID {productId} não encontrado na venda.");
            }
            itemToCancel.Cancel();
            CalculateTotalAmount();
        }

        private void CalculateTotalAmount()
        {
            TotalAmount = new Money(_items.Sum(item => item.TotalItemAmount.Value));
        }
    }
}