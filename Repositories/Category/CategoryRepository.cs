using APICatalogo.Data;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories.Category;

public class CategoryRepository : Repository<Models.Category>, ICategoryRepository
{
    public CategoryRepository(Context context) : base (context) {}

    public PagedList<Models.Category> GetCategories(CategoriesParameters categoriesParameters)
    {
        return PagedList<Models.Category>.ToPagedList(Get().OrderBy(on => on.Name),
                categoriesParameters.PageNumber, categoriesParameters.PageSize);
    }

    public IEnumerable<Models.Category> GetCategoriesProducts()
    {
        return Get().Include(c => c.Products);
    }
}
