using DAL.Models;
using Repository.InterFace;
using System.Collections.Generic;

namespace Repository.IRepositories
{
    public interface ISupportRepository : IGenericRepository<Tb_Support>
    {
        List<Tb_Support> Filter(string draw,
            string length,
            string sortColumn,
            string sortColumnDirection,
            string searchValue,
            int pageSize,
            int skip,
            ref int recordsTotal,            
            string UserId,
            string include = null);
    }
}
