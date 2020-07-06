using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Repositories
{
    public class SupportRepository : GenericRepositori<Tb_Support>, ISupportRepository
    {
        private readonly DbContext _db;
        public SupportRepository(DbContext db) : base(db)
        {
            _db = (_db ?? (BaseDbContext)db);
        }


        public List<Tb_Support> Filter(string draw,
            string length,
            string sortColumn,
            string sortColumnDirection,
            string searchValue,
            int pageSize,
            int skip,
            ref int recordsTotal,
            string UserId,
            string include = null)
        {
            IQueryable<Tb_Support> query = _dbset;

            query = query.Where(d => d.SenderUserId == UserId);

            foreach (var item in include.Split(','))
            {
                query = query.Include(item);
            }

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                if (sortColumnDirection == "asc")
                {
                    switch (sortColumn)
                    {
                        case "SupportType":
                            query = query.OrderBy(d => d.SupportType);
                            break;
                        case "Message":
                            query = query.OrderBy(d => d.Message);
                            break;
                        case "SendDateTime":
                            query = query.OrderBy(d => d.SendDateTime);
                            break;
                        case "SupportPosition":
                            query = query.OrderBy(d => d.SupportPosition);
                            break;
                        
                        default:
                            break;
                    }
                }
                if (sortColumnDirection == "desc")
                {
                    switch (sortColumn)
                    {
                        case "SupportType":
                            query = query.OrderBy(d => d.SupportType);
                            break;
                        case "Message":
                            query = query.OrderBy(d => d.Message);
                            break;
                        case "SendDateTime":
                            query = query.OrderBy(d => d.SendDateTime);
                            break;
                        case "SupportPosition":
                            query = query.OrderBy(d => d.SupportPosition);
                            break;

                        default:
                            break;
                    }
                }
            }


            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(m => m.Message.Contains(searchValue));
            }

            recordsTotal = query.Count();

            var result = query.Skip(skip).Take(pageSize).ToList();


            return result;
        }

    }

}
