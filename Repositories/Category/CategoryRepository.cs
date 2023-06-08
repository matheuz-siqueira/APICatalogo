using APICatalogo.Data;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories.Category;

public class CategoryRepository : Repository<Models.Category>, ICategoryRepository
{
    public CategoryRepository(Context context) : base (context) {}
    public IEnumerable<Models.Category> GetCategoriesProducts()
    {
        return Get().Include(c => c.Products);
    }
}
