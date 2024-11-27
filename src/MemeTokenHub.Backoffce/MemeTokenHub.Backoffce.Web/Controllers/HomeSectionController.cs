using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MemeTokenHub.Backoffce.Services.Interfaces;
using Meme.Domain.Models;

namespace Partners.Management.Web.Controllers
{
    [Authorize]
    [Route("memepages/{id}/homesection", Name = "HomeSection")]
    public class HomeSectionController : CustomBaseController
    {
        private readonly IMemePageService _memePageService;
        private readonly IEmailSender _emailSender;

        public HomeSectionController(
            IMemePageService tenantService,
            IEmailSender emailSender)
        {
            _memePageService = tenantService;
            _emailSender = emailSender;
        }

        private ActionResult RedirectToList()
        {
            return RedirectToAction(nameof(Index), nameof(MemePagesController));
        }

        [HttpGet("")]
        public async Task<ActionResult> Index(string id)
        {
            if (string.IsNullOrEmpty(id)) RedirectToList();

            var model = await _memePageService.GetAsync(id);
            if (model == null) RedirectToList();

            model.HomeSection ??= new HomeSectionModel();
            model.HomeSection.Metadata ??= [];
            return View(model);
        }

        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string id, [FromForm] HomeSectionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var page = await _memePageService.GetAsync(id);
                    if (page == null) RedirectToAction(nameof(Index));

                    page.HomeSection ??= new HomeSectionModel();
                    page.HomeSection.Tagline = model.Tagline;
                    page.HomeSection.Description = model.Description;
                    page.HomeSection.Title = model.Title;

                    await _memePageService.UpdateAsync(id, page);
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
