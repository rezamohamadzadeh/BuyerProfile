using AutoMapper;
using BuyerProfile.Web.Models;
using Common.Extensions;
using Common.Images;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.InterFace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BuyerProfile.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<BaseDbContext> _uow;
        private readonly SignInManager<ApplicationUser> _signInManager;
        #endregion
        #region Ctor
        public HomeController(UserManager<ApplicationUser> userManager,
            IMapper mapper, IUnitOfWork<BaseDbContext> uow,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _uow = uow;
            _signInManager = signInManager;
        }
        #endregion
        #region Actions
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// show dashboard items as ajax json by DateTime parameter
        /// </summary>
        /// <param name="filterValue"></param>
        /// <returns></returns>
        public JsonResult GetDashboardActivities(int filterValue = 0)
        {

            var userPurchases = _mapper.Map<IEnumerable<SellDto>>(_uow.SellRepo.GetPurchasesOnDashboard(5, UserExtention.GetUserMail(User), filterValue));

            var registered = filterValue == 0 ? _uow.SellRepo.Get(d => d.Email == UserExtention.GetUserMail(User) && d.PayStatus == PayStatus.Registered) : _uow.SellRepo.Get(d => d.Email == UserExtention.GetUserMail(User) && d.PayStatus == PayStatus.Registered && d.CreateAt > DateTime.Now.AddDays(-filterValue));

            var RegisteredCount = registered == null ? 0 : registered.Count();

            var purchases = filterValue == 0 ? _uow.SellRepo.Get(d => d.Email == UserExtention.GetUserMail(User) && d.PayStatus != PayStatus.Registered) : _uow.SellRepo.Get(d => d.Email == UserExtention.GetUserMail(User) && d.PayStatus != PayStatus.Registered && d.CreateAt > DateTime.Now.AddDays(-filterValue));

            var SumPurchases = purchases != null ? purchases.Sum(d => d.Price) : 0;

            var CountPurchases = purchases != null ? purchases.Count() : 0;

            return Json(new
            {
                UserPurchases = userPurchases,
                RegisteredCount = RegisteredCount,
                SumPurchases = SumPurchases,
                PurchasesCount = CountPurchases,
            });
        }

        /// <summary>
        /// manage user profile
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> Profile()
        {

            try
            {
                var userDto = _mapper.Map<ProfileDto>(await _userManager.FindByIdAsync(UserExtention.GetUserId(User)));
                if (userDto == null)
                {
                    ViewBag.ErrorMessage = ErrorMessageForGetInformation;
                    return View();
                }
                return View(userDto);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ErrorMessageForGetInformation + " \n " + ex.Message;
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileDto profileDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(profileDto.Id))
                    {
                        ViewBag.ErrorMessage = ErrorMessageForGetInformation;
                        return View(profileDto);
                    }

                    var user = await _userManager.FindByIdAsync(profileDto.Id);

                    if (profileDto.Files != null)
                    {
                        Upload uploader = new Upload();
                        Delete delete = new Delete();
                        if (user.Image != null)
                        {
                            string deletPath = Path.Combine(
                                Directory.GetCurrentDirectory(), "wwwroot/UserProfile", user.Image
                            );
                            delete.DeleteImage(deletPath);
                        }


                        profileDto.Image = Guid.NewGuid().ToString() + Path.GetExtension(profileDto.Files.FileName);
                        string savePath = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/UserProfile", profileDto.Image

                        );


                        string DirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserProfile");
                        await uploader.UploadImage(savePath, DirectoryPath, profileDto.Files);
                        await _signInManager.SignOutAsync();

                    }
                    else
                        profileDto.Image = user.Image;



                    user.Name = profileDto.Name;
                    user.Image = profileDto.Image;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        ViewBag.SuccessMessage = SuccessEditMessage;
                    }
                    else
                        ViewBag.ErrorMessage = ErrorMessage;

                    await _signInManager.RefreshSignInAsync(user);
                    TempData["Image"] = user.Image == null ? "" : user.Image;
                }
                return View(profileDto);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ErrorMessage + " \n " + ex.Message;
                return View(profileDto);
            }

        }
        #endregion



    }
}