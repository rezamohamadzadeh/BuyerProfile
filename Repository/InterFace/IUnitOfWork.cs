using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.InterFace
{
    public interface IUnitOfWork<TEntity> where TEntity : DbContext
    {
        public SellRepository SellRepo { get; }

        public UserRepository UserRepo { get; }

        public SupportRepository SupportRepo { get; }        

        public AffiliateRepository AffiliateRepo { get; }

        public SupportTypeRepository SupportTypeRepo { get; }

        public AffiliateParameterRepository AffiliateParameterRepo { get; }

        bool BackUpFromDb(string path);

        void Save();

        Task SaveAsync();
    }
}
