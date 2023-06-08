namespace APICatalogo.Repositories.Category;

public interface ICategoryRepository : IRepository<Models.Category> 
{
    IEnumerable<Models.Category> GetCategoriesProducts();
}
