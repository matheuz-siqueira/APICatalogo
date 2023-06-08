using System.Linq.Expressions;
using APICatalogo.Data;

namespace APICatalogo.Repositories.Product;

public class ProductRepository : Repository<Models.Product>, IProductRepository
{
    public ProductRepository(Context context) : base (context) {}
    public IEnumerable<Models.Product> GetProductsByPrice()
    {
        return Get().OrderBy(p => p.Price).ToList();
    }
}
