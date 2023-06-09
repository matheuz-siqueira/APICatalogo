using System.Linq.Expressions;
using APICatalogo.Data;
using APICatalogo.Pagination;

namespace APICatalogo.Repositories.Product;

public class ProductRepository : Repository<Models.Product>, IProductRepository
{
    public ProductRepository(Context context) : base (context) {}

    public PagedList<Models.Product> GetProducts(ProductsParameters productsParameters)
    {   
        return PagedList<Models.Product>.ToPagedList(Get().OrderBy(on => on.Name),
                productsParameters.PageNumber, productsParameters.PageSize);
    }

    public IEnumerable<Models.Product> GetProductsByPrice()
    {
        return Get().OrderBy(p => p.Price).ToList();
    }
}
