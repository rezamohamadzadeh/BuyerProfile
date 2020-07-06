using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class AffiliateParameterRepository : GenericRepositori<Tb_AffiliateParameter>, IAffiliateParameterRepository
    {
        private readonly DbContext _db;
        public AffiliateParameterRepository(DbContext db) : base(db)
        {
            _db = (_db ?? (BuyerDbContext)db);
        }


    }
}
