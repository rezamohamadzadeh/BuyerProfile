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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BuyerProfile.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<BuyerDbContext> _uow;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(UserManager<ApplicationUser> userManager,
            IMapper mapper, IUnitOfWork<BuyerDbContext> uow,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _uow = uow;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var userSells = _uow.SellRepo.Get(d => d.Email == User.Identity.Name && d.PayStatus != PayStatus.Registered)
                .GroupBy(m => m.ProductName)
                .Select(d => new ProductSellDto { ProductName = d.Key, SellCount = d.Count() })
                .OrderBy(x => x.ProductName).ToList();
            return View(userSells);
        }

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

    }
}