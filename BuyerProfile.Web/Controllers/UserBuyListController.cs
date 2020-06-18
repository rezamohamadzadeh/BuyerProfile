using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BuyerProfile.Web.Models;
using Marketing.Areas.AdminPanel.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.InterFace;

namespace BuyerProfile.Web.Controllers
{
    [Authorize]
    public class UserBuyListController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public UserBuyListController(IMapper mapper, IUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            var dtValues = SetDataTableRequest();
            int recordsTotal = 0;
            var data = _mapper.Map<List<SellDto>>(_uow.SellRepo.GetAllUserBuy(dtValues.draw, dtValues.length, dtValues.sortColumn, dtValues.sortColumnDirection, dtValues.searchValue, dtValues.pageSize, dtValues.skip, ref recordsTotal));

            dtValues.recordsTotal = recordsTotal;

            return Json(new AjaxResult { draw = dtValues.draw, recordsFiltered = dtValues.recordsTotal, recordsTotal = dtValues.recordsTotal, data = data });
        }

        public ActionResult RateBuy(int id, int rank)
        {
            bool success = true;
            string error = "";

            if (id == 0 || rank == 0)
            {
                success = false;
                error = ErrorMessageForGetInformation;
            }
            else
            {
                try
                {
                    var userBuy = _uow.SellRepo.GetById(id);
                    userBuy.Rank = rank;
                    _uow.SellRepo.Update(userBuy);
                    _uow.Save();
                }
                catch (Exception ex)
                {
                    success = false;
                    error = ex.Message;
                }
            }


            return Json(new { error = error, success = success, pid = id });
        }
    }

}