using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using MemeTokenHub.Backoffce.Models;
using MemeTokenHub.Backoffce.Services;
using MemeTokenHub.Backoffce.Services.Interfaces;
using Partners.Management.Web.Models;
using System.Collections.Generic;

namespace Partners.Management.Web.Controllers
{
    [Authorize]
    [Route("memepages/{id}/about")]
    public class AboutPageController : CustomBaseController
    {
        private readonly IMemePageService _memePageService;
        private readonly IEmailSender _emailSender;

        public AboutPageController(
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

            model.About ??= new MemePageAboutModel();
            model.About.Metadata ??= [];
            return View(model);
        }

        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string id, [FromForm] MemePageAboutModel model)
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
