using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class SupportTypeRepository : GenericRepositori<Tb_SupportType>, ISupportTypeRepository
    {
        private readonly DbContext _db;
        public SupportTypeRepository(DbContext db) : base(db)
        {
            _db = (_db ?? (BaseDbContext)db);
        }        

    }

}
