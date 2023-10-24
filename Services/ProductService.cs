using RefactorThis.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace RefactorThis.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(ProductDbContext db) : base(db)
        {
        }

        /// <summary>
        /// Deletes a product and its options.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task DeleteProductAsync(Product product)
        {
            // Use Transaction to make sure the whole transcation is successful. 
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var removeProductOptionsList = await _dbContext.ProductOptions.Where(po => po.ProductId.Equals(product.Id)).ToListAsync();
                    if (removeProductOptionsList.Count > 0)
                    {
                        _dbContext.ProductOptions.RemoveRange(removeProductOptionsList);
                    }
                    _dbSet.Remove(product);
                    await SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
