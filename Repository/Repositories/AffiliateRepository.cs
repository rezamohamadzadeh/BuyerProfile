using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class AffiliateRepository : GenericRepositori<Tb_Affiliates>, IAffiliateRepository
    {
        private readonly DbContext _db;
        public AffiliateRepository(DbContext db) : base(db)
        {
            _db = (_db ?? (BuyerDbContext)db);
        }


    }
}
