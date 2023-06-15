using APICatalogo.Data;
using APICatalogo.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories.User;

public class UserRepository : Repository<Models.User>, IUserRepository
{
    public UserRepository([FromServices] Context context) : base(context)
    {}

    public async Task<Models.User> GetByEmail(string email)
    {
        return await Get().AsNoTracking().FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<Models.User> GetProfile(int id)
    {
        return await Get().AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);
    }
}
