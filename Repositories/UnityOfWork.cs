using APICatalogo.Data;
using APICatalogo.Repositories.Category;
using APICatalogo.Repositories.Product;
using APICatalogo.Repositories.User;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Repositories;

public class UnityOfWork : IUnityOfWork
{
    private ProductRepository _productRepo; 
    private CategoryRepository _categoryRepo;   
    private UserRepository _userRepo;
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

    public IUserRepository UserRepository
    {
        get
        {
            return _userRepo = _userRepo ?? new UserRepository(_context); 
        }
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
