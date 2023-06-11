using APICatalogo.Pagination;

namespace APICatalogo.Repositories.Product;

public interface IProductRepository : IRepository<Models.Product> 
{
    Task<IEnumerable<Models.Product>> GetProductsByPrice();
    Task<PagedList<Models.Product>> GetProducts(ProductsParameters productsParameters);
}
