using DAL.Models;
using Repository.InterFace;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface ISellRepository : IGenericRepository<Tb_Sell>
    {
        List<Tb_Sell> Filter(string draw,
            string length,
            string sortColumn,
            string sortColumnDirection,
            string searchValue,
            int pageSize,
            int skip,
            ref int recordsTotal,
            string Email);

        IEnumerable<Tb_Sell> GetPurchasesOnDashboard(int count, string userMail, int filterValue = 0);
    }

}
