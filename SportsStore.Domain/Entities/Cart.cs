using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportsStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Product product, int quantity)
        {
            //gets the current state of cart
            CartLine line = lineCollection
                                    .Where(p => p.Product.ProductID == product.ProductID)
                                    .FirstOrDefault();

            if (line == null) //if adding product first time
            {
                lineCollection.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else //if product is already present we can just increment the qty
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product)
        {
            lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);
        }
        
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Product.Price * e.Quantity);
        }
        
        public void Clear()
        {
            lineCollection.Clear();
        }
        
        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

    }
}
