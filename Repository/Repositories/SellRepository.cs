using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Repositories
{
    public class SellRepository : GenericRepositori<Tb_Sell>, ISellRepository
    {
        private readonly DbContext _db;
        public SellRepository(DbContext db) : base(db)
        {
            _db = (_db ?? (BaseDbContext)db);
        }


        public List<Tb_Sell> Filter(string draw,
            string length,
            string sortColumn,
            string sortColumnDirection,
            string searchValue,
            int pageSize,
            int skip,
            ref int recordsTotal,
            string Email)
        {
            IQueryable<Tb_Sell> query = _dbset;

            query = query.Where(d => d.Email == Email);

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                if (sortColumnDirection == "asc")
                {
                    switch (sortColumn)
                    {
                        case "Email":
                            query = query.OrderBy(d => d.Email);
                            break;
                        case "FullName":
                            query = query.OrderBy(d => d.FullName);
                            break;
                        case "ProductName":
                            query = query.OrderBy(d => d.ProductName);
                            break;
                        case "Price":
                            query = query.OrderBy(d => d.Price);
                            break;
                        case "AffiliateCode":
                            query = query.OrderBy(d => d.AffiliateCode);
                            break;
                        case "PayStatus":
                            query = query.OrderBy(d => d.PayStatus);
                            break;
                        case "DiliveryStatus":
                            query = query.OrderBy(d => d.DiliveryStatus);
                            break;
                        case "CreateAt":
                            query = query.OrderBy(d => d.CreateAt);
                            break;

                        default:
                            break;
                    }
                }
                if (sortColumnDirection == "desc")
                {
                    switch (sortColumn)
                    {
                        case "Email":
                            query = query.OrderByDescending(d => d.Email);
                            break;
                        case "FullName":
                            query = query.OrderByDescending(d => d.FullName);
                            break;
                        case "ProductName":
                            query = query.OrderByDescending(d => d.ProductName);
                            break;
                        case "Price":
                            query = query.OrderByDescending(d => d.Price);
                            break;
                        case "AffiliateCode":
                            query = query.OrderByDescending(d => d.AffiliateCode);
                            break;
                        case "PayStatus":
                            query = query.OrderByDescending(d => d.PayStatus);
                            break;
                        case "DiliveryStatus":
                            query = query.OrderByDescending(d => d.DiliveryStatus);
                            break;
                        case "CreateAt":
                            query = query.OrderByDescending(d => d.CreateAt);
                            break;
                        default:
                            break;
                    }
                }
            }


            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(m => m.AffiliateCode == searchValue);
            }

            recordsTotal = query.Count();

            var result = query.Skip(skip).Take(pageSize).ToList();


            return result;
        }

        public IEnumerable<Tb_Sell> GetPurchasesOnDashboard(int count, string userMail, int filterValue = 0)
        {
            IQueryable<Tb_Sell> query = _dbset;


            if (filterValue != 0)
            {
                query = query.Where(d => d.CreateAt > DateTime.Now.AddDays(-filterValue));
            }
            query = query.Where(d => d.Email == userMail);
            query.Take(count).ToList();

            return query;
        }

    }

}
