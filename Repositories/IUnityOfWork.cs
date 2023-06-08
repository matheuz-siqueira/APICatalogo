using APICatalogo.Repositories.Category;
using APICatalogo.Repositories.Product;

namespace APICatalogo.Repositories;

public interface IUnityOfWork
{
    IProductRepository ProductRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    void Commit();
}
