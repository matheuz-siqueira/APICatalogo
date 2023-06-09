using APICatalogo.Pagination;

namespace APICatalogo.Repositories.Category;

public interface ICategoryRepository : IRepository<Models.Category> 
{
    PagedList<Models.Category> GetCategories(CategoriesParameters categoriesParameters);
    IEnumerable<Models.Category> GetCategoriesProducts();
}
