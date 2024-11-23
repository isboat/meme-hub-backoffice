using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MemeTokenHub.Backoffce.Services.Interfaces;
using Meme.Domain.Models;

namespace Partners.Management.Web.Controllers
{
    [Authorize]
    [Route("memepages/{id}/aboutsection", Name = "AboutSection")]
    public class AboutSectionController : CustomBaseController
    {
        private readonly IMemePageService _memePageService;
        private readonly IEmailSender _emailSender;

        public AboutSectionController(
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

            model.About ??= new AboutSectionModel();
            model.About.Metadata ??= [];
            return View(model);
        }

        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string id, [FromForm] AboutSectionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var page = await _memePageService.GetAsync(id);
                    if (page == null) RedirectToAction(nameof(Index));

                    page.About = model;
                    await _memePageService.UpdateAsync(id, page);
                }
                return Redirect("/memepages");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet("blankrow")]
        public ActionResult BlankRow()
        {
            return PartialView("EditMetadata", new KeyValuePair<string, string>("testKey", "testvalue"));
        }
    }
}
