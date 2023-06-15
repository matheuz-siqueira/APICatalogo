using System.Linq.Expressions;

namespace APICatalogo.Repositories.User;

public interface IUserRepository : IRepository<Models.User>
{
    Task<Models.User> GetByEmail(string email);

    Task<Models.User> GetProfile(int id);
}
