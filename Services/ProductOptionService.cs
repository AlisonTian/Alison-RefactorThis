using Microsoft.Extensions.Logging;
using RefactorThis.Models;

namespace RefactorThis.Services
{
    public class ProductOptionService : BaseService<ProductOption>, IProductOptionService
    {
        public ProductOptionService(ProductDbContext db) : base(db)
        {
        }
    }
}
