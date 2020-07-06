using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.InterFace;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.IRepositories
{
    public interface IUserRepository<TEntity> : IGenericRepository<ApplicationUser>
    {
        ApplicationUser GetUserByName(string userName);

        ApplicationUser GetUserIncludeBusinessOwnerById(string id);

        bool CheckUserEmail(string userName, string id);
    }
}
