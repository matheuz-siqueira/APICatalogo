using APICatalogo.Data;
using APICatalogo.Repositories.Category;
using APICatalogo.Repositories.Product;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Repositories;

public class UnityOfWork : IUnityOfWork
{
    private ProductRepository _productRepo; 
    private CategoryRepository _categoryRepo; 

    private Context _context;
    public UnityOfWork([FromServices] Context context)
    {
        _context = context; 
    }
    public IProductRepository ProductRepository
    {
        get
        {
            return _productRepo = _productRepo ?? new ProductRepository(_context);
        }
    }

    public ICategoryRepository CategoryRepository
    {
        get
        {
            return _categoryRepo = _categoryRepo ?? new CategoryRepository(_context);
        }
    }

    public void Commit()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
