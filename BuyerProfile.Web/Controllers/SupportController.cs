using AutoMapper;
using BuyerProfile.Web.Models;
using Common.Extensions;
using Common.Images;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository.InterFace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BuyerProfile.Web.Controllers
{
    [Authorize]
    public class SupportController : BaseController
    {
        private readonly IUnitOfWork<BaseDbContext> _uow;
        private readonly IMapper _mapper;

        public SupportController(IUnitOfWork<BaseDbContext> uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// show support list in listview with jquery datatabale
        /// </summary>
        /// <returns></returns>
        public IActionResult List()
        {
            try
            {
                var dtValues = SetDataTableRequest();
                int recordsTotal = 0;
                var data = _mapper.Map<List<SupportDto>>(_uow.SupportRepo.Filter(dtValues.draw,
                    dtValues.length,
                    dtValues.sortColumn,
                    dtValues.sortColumnDirection,
                    dtValues.searchValue,
                    dtValues.pageSize,
                    dtValues.skip, ref recordsTotal,UserExtention.GetUserId(User),
                    "SupportType"));

                dtValues.recordsTotal = recordsTotal;

                return Json(new AjaxResult { draw = dtValues.draw, recordsFiltered = dtValues.recordsTotal, recordsTotal = dtValues.recordsTotal, data = data });
            }
            catch (Exception ex)
            {
                return Json("error");
            }

        }

        public IActionResult Create()
        {
            var supportDto = new SupportDto();
            FeachPayTypeCombo();
            return View(supportDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupportDto supportDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (supportDto.Attachment != null)
                    {
                        var File = Guid.NewGuid().ToString() + Path.GetExtension(supportDto.Attachment.FileName);
                        string savePath = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/supportImages", File
                        );
                        string DirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/supportImages");
                        Upload uploader = new Upload();
                        supportDto.File =  Request.Scheme + "://" + Request.Host.Value + "/supportImages/" + File;
                        await uploader.UploadImage(savePath, DirectoryPath, supportDto.Attachment);
                    }

                    var model = _mapper.Map<Tb_Support>(supportDto);
                    model.SenderUserId = UserExtention.GetUserId(User);
                    model.Email = UserExtention.GetUserMail(User);
                    model.SupportPosition = SupportPosition.Pending;
                    await _uow.SupportRepo.InsertAsync(model);

                    await _uow.SaveAsync();
                    return View(supportDto);
                }
                catch (Exception e)
                {
                    return Json(new { error = "The operation failed \n" + e.Message });
                }
            }
            return View(supportDto);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return Json(new { message = ErrorMessageForGetInformation, success = false });
                }
                var support = await _uow.SupportRepo.GetByIdAsync(id);

                if (support.SupportPosition != SupportPosition.Pending)
                {
                    return Json(new { warning = "You cannot delete your support! <br> because this support position has been changed!" });
                }

                _uow.SupportRepo.Delete(support);
                await _uow.SaveAsync();
                Upload uploader = new Upload();
                Delete delete = new Delete();
                if (support.File != null)
                {
                    string deletPath = Path.Combine(
                        Directory.GetCurrentDirectory(), "wwwroot/supportImages", support.File
                    );
                    delete.DeleteImage(deletPath);
                }
                return Json(new { success = SuccessDeleteMessage });

            }
            catch (Exception ex)
            {
                return Json(new { error = ErrorMessage + " " + ex.Message });
            }
        }
        /// <summary>
        /// edit support in modal
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return Json(new { status = 404 });
                }

                var prodcut = await _uow.SupportRepo.GetByIdAsync(id);

                if (prodcut == null)
                {
                    return Json(new { status = 404 });
                }

                var viewModel = _mapper.Map<SupportDto>(prodcut);

                FeachPayTypeCombo();
                return View(viewModel);
            }
            catch (Exception)
            {
                return Json(new { status = 404 });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SupportDto supportDto)
        {
            if (supportDto.Id == 0)
            {
                return Json(new { error = "Not found!" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _uow.SupportRepo.GetByIdAsync(supportDto.Id);

                    if (result.SupportPosition != SupportPosition.Pending)
                    {
                        return Json(new { warning = "You cannot edit your support! <br> because this support position has been changed!" });

                    }

                    if (supportDto.Attachment != null)
                    {
                        Upload uploader = new Upload();
                        Delete delete = new Delete();
                        if (result.File != null)
                        {
                            string deletPath = Path.Combine(
                                Directory.GetCurrentDirectory(), "wwwroot/supportImages", result.File
                            );
                            delete.DeleteImage(deletPath);
                        }


                        string File = Guid.NewGuid().ToString() + Path.GetExtension(supportDto.Attachment.FileName);
                        string savePath = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/supportImages", File
                        );


                        string DirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/supportImages");
                        await uploader.UploadImage(savePath, DirectoryPath, supportDto.Attachment);
                        supportDto.File = Request.Scheme + "://" + Request.Host.Value + "/supportImages/" + File;

                    }
                    else
                        supportDto.File = result.File;


                    result.TypeId = supportDto.TypeId;
                    result.Message = supportDto.Message;
                    result.File = supportDto.File;
                    _uow.SupportRepo.Update(result);
                    await _uow.SaveAsync();
                    return View(supportDto);
                }
                catch (Exception e)
                {
                    return Json(new { error = "The operation failed \n" + e.Message });
                }
            }
            return View(supportDto);
        }

        /// <summary>
        /// feach Support Type in modal view
        /// </summary>
        private void FeachPayTypeCombo()
        {
            try
            {
                var supportTypes = _uow.SupportTypeRepo.Get();

                ViewBag.SupportTypes = new SelectList(supportTypes, "Id", "SupportTypeName"); ;
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = ErrorMessageForGetInformation + e.Message;
            }
        }
    }
}
