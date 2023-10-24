using RefactorThis.Models;
using System.Threading.Tasks;

namespace RefactorThis.Services
{
    public interface IProductService : IBaseService<Product>
    {
        Task DeleteProductAsync(Product product);
    }
}
