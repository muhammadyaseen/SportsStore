using System.Linq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
namespace SportsStore.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<Product> Products
        {
            get { return context.Products; }
        }

        public void SaveProduct(Product p)
        {
            if (p.ProductID == 0)
            {
                context.Products.Add(p);
            }

            context.SaveChanges();
        }


        public void DeleteProduct(Product p)
        {
            context.Products.Remove(p);
            context.SaveChanges();
        }
    }
}