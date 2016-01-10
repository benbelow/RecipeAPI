using System.Linq;
using RecipeAPI.Models;
using System.Data.Entity;

namespace RecipeAPI.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserById(int userId);
        User GetUserByAccessToken(string accessToken);
        User GetUserByRefreshToken(string refreshToken);
    }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context) { }

        public User GetUserById(int userId)
        {
            return Entities.Where(u => u.UserID == userId).SingleOrDefault();
        }

        public User GetUserByAccessToken(string accessToken)
        {
            return Entities.Where(u => u.AccessToken == accessToken).SingleOrDefault();
        }

        public User GetUserByRefreshToken(string refreshToken)
        {
            return Entities.Where(u => u.RefreshToken == refreshToken).SingleOrDefault();
        }
    }
}