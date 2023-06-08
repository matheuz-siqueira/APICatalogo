namespace APICatalogo.Repositories.Product;

public interface IProductRepository : IRepository<Models.Product> 
{
    IEnumerable<Models.Product> GetProductsByPrice();
}
