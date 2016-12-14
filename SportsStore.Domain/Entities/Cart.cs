using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> _lines = new List<CartLine>();

        public void AddItem(Product product, int quantity)
        {
            var line = _lines.FirstOrDefault(x => x.Product.ProductId == product.ProductId);
            if(line != null)
            {
                line.Quantity += quantity;
            }
            else
            {
                _lines.Add(new CartLine {Product = product, Quantity =  quantity});
            }
        }

        public void RemoveLine(Product product)
        {
            _lines.RemoveAll(x => x.Product.ProductId == product.ProductId);
        }

        public decimal ComputeTotalValue()
        {
            return _lines.Sum(x => x.Product.Price*x.Quantity);
        }

        public void Clear()
        {
            _lines.Clear();
        }

        public IEnumerable<CartLine> Lines => _lines;
    }

    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}