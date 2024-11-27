using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MemeTokenHub.Backoffce.Services.Interfaces;
using Meme.Domain.Models;
using Partners.Management.Web.Models;
using System.Drawing;

namespace Partners.Management.Web.Controllers
{
    [Authorize]
    [Route("memepages/{id}/imageupload")]
    public class ImageUploadController : CustomBaseController
    {
        private readonly IMemePageService _memePageService;
        private readonly IEmailSender _emailSender;
        private readonly IUploadService _uploadService;
        private readonly string allowedImageFileExt = "image/jpeg,image/png, image/webp";

        public ImageUploadController(
            IMemePageService tenantService,
            IEmailSender emailSender,
            IUploadService uploadService)
        {
            _memePageService = tenantService;
            _emailSender = emailSender;
            _uploadService = uploadService;
        }

        private ActionResult RedirectToList()
        {
            return RedirectToAction(nameof(Index), nameof(MemePagesController));
        }

        [HttpGet("single")]
        public async Task<ActionResult> Single(string id, [FromQuery]string section, [FromQuery]string imgType)
        {
            if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(section) || string.IsNullOrEmpty(imgType)) RedirectToList();

            return View(new SingleImageUpload { MemepageId = id, Section = section, ImgType = imgType});
        }

        [HttpPost("single")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Single(string id, [FromForm] SingleImageUpload model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model?.File == null)
                    {
                        return BadRequest("file is null");
                    }
                    var isImageFile = allowedImageFileExt.Contains(model.File.ContentType);

                    if (!isImageFile)
                    {
                        return BadRequest($"{model.File.ContentType} Not allowed");
                    }

                    var size = model.File.Length;
                    if (model.File.Length > 0)
                    {
                        var page = await _memePageService.GetAsync(id);
                        if (page == null) return BadRequest($"Page with id = {id} not found");

                        var fileName = model.File.FileName.ToLowerInvariant();
                        await using var stream = model.File.OpenReadStream();
                        var storagePath = await _uploadService.UploadAsync(id, model.Section!, fileName, stream);

                        // store path
                        page.Images ??= [];
                        page.Images.Add(new PageImageModel { Section = model.Section, ImgType = model.ImgType, StoragePath = storagePath });
                        await _memePageService.UpdateAsync(id, page);
                    }
                }
                return Redirect("/memepages");
            }
            catch
            {
                return View();
            }
        }
    }
}
