using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;
using System.Linq;

namespace Repository.Repositories
{
    public class UserRepository : GenericRepositori<ApplicationUser>, IUserRepository<ApplicationUser>
    {
        private readonly DbContext _db;
        public UserRepository(DbContext db) : base(db)
        {
            _db = (_db ?? (BuyerDbContext)db);
        }

        public ApplicationUser GetUserByName(string userName)
        {
            return Get(q => q.UserName == userName).FirstOrDefault();
        }
        public ApplicationUser GetUserIncludeBusinessOwnerById(string id)
        {
            return Get(q => q.Id == id,null, "BusinessOwner").FirstOrDefault();
        }
        public bool CheckUserEmail(string userName,string id)
        {
            var user = Get(q => q.UserName == userName && q.Id != id).FirstOrDefault();
            if (user != null)
                return true;            
            return false;
        }


    }
}
