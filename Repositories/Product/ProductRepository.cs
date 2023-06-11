using System.Linq.Expressions;
using APICatalogo.Data;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories.Product;

public class ProductRepository : Repository<Models.Product>, IProductRepository
{
    public ProductRepository(Context context) : base (context) {}

    public async Task<PagedList<Models.Product>> GetProducts(ProductsParameters productsParameters)
    {   
        return await PagedList<Models.Product>.ToPagedList(Get().OrderBy(on => on.Name),
                productsParameters.PageNumber, productsParameters.PageSize);
    }

    public async Task<IEnumerable<Models.Product>> GetProductsByPrice()
    {
        return await Get().OrderBy(p => p.Price).ToListAsync();
    }
}
