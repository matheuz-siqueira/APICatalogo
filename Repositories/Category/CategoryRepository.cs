using APICatalogo.Data;
using APICatalogo.Pagination;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories.Category;

public class CategoryRepository : Repository<Models.Category>, ICategoryRepository
{
    public CategoryRepository(Context context) : base (context) {}

    public async Task<PagedList<Models.Category>> GetCategories(CategoriesParameters categoriesParameters)
    {
        return await PagedList<Models.Category>.ToPagedList(Get().OrderBy(on => on.Name),
                categoriesParameters.PageNumber, categoriesParameters.PageSize);
    }

    public async Task<IEnumerable<Models.Category>> GetCategoriesProducts()
    {
        return await Get().Include(c => c.Products).ToListAsync();
    }
}
