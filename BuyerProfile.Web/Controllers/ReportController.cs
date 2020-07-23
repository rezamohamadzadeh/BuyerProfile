using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BuyerProfile.Web.Models;
using Common.Extensions;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.InterFace;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace BuyerProfile.Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<BaseDbContext> _uow;

        public ReportController(IMapper mapper, IUnitOfWork<BaseDbContext> uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        /// <summary>
        /// get report UserBuyPrintPage filter
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IActionResult UserBuyPrintPage(SellReportDto model)
        {
            TempData["StartDate"] = model.StartDate;
            TempData["EndDate"] = model.EndDate;
            TempData["FilterType"] = model.FilterType;
            TempData["SkipDate"] = model.SkipDate;

            return View();
        }

        /// <summary>
        /// get UserPurchases for report
        /// </summary>
        /// <returns></returns>
        public ActionResult UserPurchases()
        {
            try
            {
                var model = new SellReportDto();

                if (TempData["StartDate"] != null)
                {
                    model.StartDate = DateTime.Parse(TempData["StartDate"].ToString());
                }
                if (TempData["EndDate"] != null)
                {
                    model.EndDate = DateTime.Parse(TempData["EndDate"].ToString());
                }
                if (TempData["FilterType"] != null)
                {
                    model.FilterType = (FilterType)(TempData["FilterType"]);
                }
                if (TempData["SkipDate"] != null)
                {
                    model.SkipDate = (bool)(TempData["SkipDate"]);
                }

                StiReport stiReport = new StiReport();

                stiReport.Load(StiNetCoreHelper.MapPath(this, "wwwroot/Reports/UserBuyReport.mrt"));


                IEnumerable<Tb_Sell> userPurchases = null;
                switch (model.FilterType)
                {
                    case FilterType.All:
                        userPurchases = !model.SkipDate ? _uow.SellRepo.Get(d => d.Email == UserExtention.GetUserMail(User) && d.CreateAt >= model.StartDate && d.CreateAt <= model.EndDate) : _uow.SellRepo.Get(d => d.Email == UserExtention.GetUserMail(User));
                        break;
                    case FilterType.OnlyRegister:
                        userPurchases = !model.SkipDate ? _uow.SellRepo.Get(d => d.Email == UserExtention.GetUserMail(User) && d.PayStatus == PayStatus.Registered && d.CreateAt >= model.StartDate && d.CreateAt <= model.EndDate) : _uow.SellRepo.Get(d => d.Email == UserExtention.GetUserMail(User) && d.PayStatus == PayStatus.Registered);
                        break;
                    case FilterType.Sells:
                        userPurchases = !model.SkipDate ? _uow.SellRepo.Get(d => d.Email == UserExtention.GetUserMail(User) && d.PayStatus != PayStatus.Registered && d.CreateAt >= model.StartDate && d.CreateAt <= model.EndDate) : _uow.SellRepo.Get(d => d.Email == UserExtention.GetUserMail(User) && d.PayStatus != PayStatus.Registered);
                        break;
                    default:

                        break;
                }                

                var sumSell = userPurchases.Sum(d => d.Price);

                stiReport["SumBuy"] = sumSell.ToString("N0");

                stiReport.RegData("userPurchasesDT", userPurchases);

                return StiNetCoreViewer.GetReportResult(this, stiReport);
            }
            catch (Exception ex)
            {
                return null;
            }


        }
        /// <summary>
        /// for display report views
        /// </summary>
        /// <returns></returns>

        public IActionResult ViewerEvent()
        {
            return StiNetCoreViewer.ViewerEventResult(this);
        }

        public ActionResult FilterUserPurchases()
        {
            return View();
        }

    }
}