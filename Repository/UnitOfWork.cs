using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.InterFace;
using Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork<TEntity> : IUnitOfWork<TEntity> where TEntity : DbContext, IDisposable
    {
        private readonly DbContext _dbContext;
        public UnitOfWork(TEntity dbContext)
        {
            _dbContext = dbContext;
        }
        private UserRepository _UserRepo;
        public UserRepository UserRepo
        {
            get
            {
                if (_UserRepo == null)
                {
                    _UserRepo = new UserRepository(_dbContext);
                }
                return _UserRepo;
            }
        }

        private AffiliateRepository _affiliateRepository;

        public AffiliateRepository AffiliateRepo
        {
            get
            {
                if (_affiliateRepository == null)
                {
                    _affiliateRepository = new AffiliateRepository(_dbContext);
                }
                return _affiliateRepository;
            }
        }

        private SupportRepository _supportRepository;

        public SupportRepository SupportRepo
        {
            get
            {
                if (_supportRepository == null)
                {
                    _supportRepository = new SupportRepository(_dbContext);
                }
                return _supportRepository;
            }
        }
        private SupportTypeRepository _supportTypeRepository;

        public SupportTypeRepository SupportTypeRepo
        {
            get
            {
                if (_supportTypeRepository == null)
                {
                    _supportTypeRepository = new SupportTypeRepository(_dbContext);
                }
                return _supportTypeRepository;
            }
        }


        private SellRepository _sellRepository;

        public SellRepository SellRepo
        {
            get
            {
                if (_sellRepository == null)
                {
                    _sellRepository = new SellRepository(_dbContext);
                }
                return _sellRepository;
            }
        }

        private AffiliateParameterRepository _affiliateParameter;

        public AffiliateParameterRepository AffiliateParameterRepo
        {
            get
            {
                if (_affiliateParameter == null)
                {
                    _affiliateParameter = new AffiliateParameterRepository(_dbContext);
                }
                return _affiliateParameter;
            }
        }


        public void Save()
        {
            _dbContext.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        #region BackUpFromDb
        public bool BackUpFromDb(string path)
        {
            try
            {
                _dbContext.Database.ExecuteSqlRaw("BACKUP DATABASE BaseProj TO DISK = {0}", path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
