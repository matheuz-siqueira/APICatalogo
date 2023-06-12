using APICatalogo.Repositories.Category;
using APICatalogo.Repositories.Product;
using APICatalogo.Repositories.User;

namespace APICatalogo.Repositories;

public interface IUnityOfWork
{
    IProductRepository ProductRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IUserRepository UserRepository { get; }
    Task Commit();
}
