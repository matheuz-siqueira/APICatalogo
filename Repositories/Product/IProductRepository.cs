using APICatalogo.Pagination;

namespace APICatalogo.Repositories.Product;

public interface IProductRepository : IRepository<Models.Product> 
{
    IEnumerable<Models.Product> GetProductsByPrice();
    PagedList<Models.Product> GetProducts(ProductsParameters productsParameters);
}
