using APICatalogo.Pagination;

namespace APICatalogo.Repositories.Category;

public interface ICategoryRepository : IRepository<Models.Category> 
{
    Task<PagedList<Models.Category>> GetCategories(CategoriesParameters categoriesParameters);
    Task<IEnumerable<Models.Category>> GetCategoriesProducts();
}
